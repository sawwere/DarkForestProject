using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    private int damage;
    private int line;

    Rigidbody2D rigidbody2d;
    EnemyRange parentEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(float force, int damage, int direction, int line, EnemyRange parent, bool is_friend = false)
    {
        //Debug.Log("launch");
        this.line = line;
        rigidbody2d.rotation = 45 * direction;
        //rigidbody2d.AddForce(direction * force, ForceMode2D.Impulse);  
        rigidbody2d.velocity = new Vector2( direction, 1) * force;
        rigidbody2d.angularVelocity = - 90 * (direction);
        this.damage = damage;
        parentEnemy = parent;
        if (is_friend)
            gameObject.layer = LayerMask.NameToLayer("Projectile_Friend");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage obj = collision.gameObject.GetComponent<IDamage>();
        if (obj != null 
            && obj.GetLine() == line
            && (((1 << collision.gameObject.layer) & parentEnemy.aviableHitMask.value) != 0))
        {
            obj.RecieveDamage(damage, DamageType.projectile);
            damage = 0;
            Destroy(gameObject);

        }
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rigidbody2d.rotation)); 
        if (transform.position.magnitude > 100f)
            Destroy(gameObject);
        
    }
}
