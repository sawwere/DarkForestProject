using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;
using UnityEngine.Rendering;

[System.Serializable]
public class EnemyCharacter: MonoBehaviour, IDamage, IMovable
{
    public string _name; // имя
    public int price; // цена спавна(сложность врага)
    [SerializeField] private int maxHealth = 20; //макс здоровье
    [SerializeField] public float speed = 1.0f; //скорость передвижения
    protected float speedInit; // для восстановления скорости после остановки
    private int line; //на какой линни ходит враг
    public int damage = 1; //урон
    public float immunityPeriod = 2.0f; // переодичность получения урона
    public float hitPeriod = 5.0f; // переодичность нанесения урона
    [SerializeField] protected int _direction = 1; //направление
    private int currentHealth; //текущее здоровье
    public float immunityTimer; //таймер иммунитета к получению урона
    protected float hitTimer; //таймер нанесения урона
    public float firstHitPeriod = 1.5f; // ����� �� ������� ��������� �����

    public LayerMask aviableHitMask; // store layers where objects can be damaged
    protected bool isFriend;

    public GameObject BloodParticles;
    public GameObject MagicParticles;
    private Vector3 ParticlesSpawnPosition;

    protected float fixDeltaTimer = 0.1f;

    private float oldX = 0f;
    private float newX = 0f;

    public GameObject skull = null;

    private Transform playerPos;
    protected AudioSource source;
    public AudioClip walkSound;
    private const float MINVOL = 0.05f;
    private const float MAXVOL = 0.15f;
    private float DTime = 0f;

    protected Rigidbody2D rigidbody2d;
    [SerializeField] private GameObject resoursePrefab; // какой ресурс может выпасть с врага

    private PlayerController player;

    public int health 
    { 
        get { return currentHealth; } 
        protected set { currentHealth = value; }
    }

    public int direction
    {
        get { return _direction;}
        set { 
            if (Abs(value) == 1) 
            { 
                _direction = value; 
                transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(2 * value, 2, 2); 
            } 
        }
    }

    public float GetSpeed()
    {
        return speed * direction;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SpeedResetToZero()
    {
        speed = 0.0001f; // если поставить скорость = 0, то застыает на месте и перестает что-либо делать
    }

    public void SpeedRestore()
    {
        speed = speedInit;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
#if UNITY_ANDROID
        speed *= 0.9f;
        maxHealth = Max((int)(maxHealth * 0.9), 1);
#endif
        speedInit = speed;
        line = (int)System.Math.Round(transform.position.y);
        rigidbody2d = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        immunityTimer = 0;
        hitTimer = firstHitPeriod;
        transform.localScale = new Vector3(transform.localScale.x * direction, transform.localScale.y, transform.localScale.x);
        aviableHitMask = LayerMask.GetMask("Buildings") | LayerMask.GetMask("NPC_Friend");
        isFriend = false;
        source.volume = 0f;
        source.loop = true;
        source.clip = walkSound;
        ChangeAnimationToIdle();
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //Debug.Log((1000 - transform.position.y * 10));
        transform.GetChild(0).GetComponent<Canvas>().sortingOrder = (int)(990 - transform.position.y * 10);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        IncVolumeToMINVOL();

        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        }
        if (fixDeltaTimer > 0)
        {
            fixDeltaTimer -= Time.deltaTime;
        }
        else
        {
            oldX = newX;// ЗВУКИ ХОДЬБЫ ЗДЕСЬ
            newX = transform.position.x;
            if (Abs(newX - oldX) >= 0.05f)
            {
                ChangeAnimationToWalk();
                if (!source.isPlaying) source.Play();
                SetSoundParams();
            }
            else
            {
                ChangeAnimationToIdle();
                if (source.isPlaying) source.Stop();
            }
            fixDeltaTimer = 0.2f;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        {
            position.x = position.x + Time.deltaTime * speed * direction;
            rigidbody2d.MovePosition(position);
        }

        if (transform.position.y > 2 || transform.position.y < -1)
        {
            Destroy(gameObject);
        }
    }

    void SetSoundParams()
    {
        float posDelta = gameObject.transform.position.x - playerPos.position.x;
        if (Mathf.Abs(posDelta) >= 28)
        {
            source.panStereo = Mathf.Sign(posDelta);
            source.volume = MINVOL;
        }
        else
        {
            source.panStereo = posDelta / 28;
            source.volume = Mathf.Max(MINVOL, MAXVOL - Mathf.Abs(posDelta) / (28 / MAXVOL));
        }
    }

    void IncVolumeToMINVOL()
    {
        DTime += Time.deltaTime;

        if (source.volume < MINVOL && DTime >= 0.2f)
        {
            source.volume += 0.01f;
            DTime = 0f;
            //Debug.Log(source.volume);
        }
    }

    //вызывается при убийстве врага
    public void EnemyKilled()
    {
        Instantiate(resoursePrefab, rigidbody2d.position + Vector2.right * 0.5f, Quaternion.identity);
        Destroy(gameObject);
    }

    virtual public void RecieveDamage(int amount, DamageType damageType)
    {
        if (damageType != DamageType.wall || immunityTimer <= 0)
        {
            CalculateParticlesPosition();
            Instantiate(BloodParticles, ParticlesSpawnPosition, Quaternion.identity);
            currentHealth -= amount;
            if (currentHealth <= 0)
                EnemyKilled();
            if (damageType == DamageType.wall)
                immunityTimer = immunityPeriod;
        }
    }

    private void CalculateParticlesPosition()
    {
        ParticlesSpawnPosition = transform.position;
        ParticlesSpawnPosition.y += 0.85f;
    }

    public virtual void DoDamage(IDamage obj)
    {
        return; // заглушка
    }

    //нанесение урона базе игрока и самоуничтожение
    public void EnterMainBuilding()
    {
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        Destroy(gameObject);
    }

    //инициирует отступление
    public virtual void ReturnToBase()
    {
        direction = transform.position.x < 0 ? -1 : 1;
        SpeedRestore();
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.x);
    }

    public virtual void ChangeAnimationToWalk()
    {
        return;
    }
    
    public virtual void ChangeAnimationToIdle()
    {
        return;
    }

    public int GetLine() 
    {
        return this.line;
    }

    public bool IsFriend()
    {
        return isFriend;
    }


    public void PauseWalkSound()
    {
        source.loop = false;
        source.Pause();
    }

    public void ContinueWalkSound()
    {
        source.loop = true;
        source.Play();
    }

    public void IsFriendMakeTrue()
    {
        isFriend = true;
    }

    protected int GetMaxHealth()
    {
        return maxHealth;
    }
}
