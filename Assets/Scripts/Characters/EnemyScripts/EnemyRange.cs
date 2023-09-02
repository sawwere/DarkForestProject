using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : EnemyCharacter
{
    public GameObject target;
    public Vector2 targetPoint;

    public Animator animator;

    public GameObject projectilePrefab;

    protected bool hasGrenades;//флаг для проверки текущего режима, true - дальний бой

    //������ �������� ��� �������
    protected string hitAnim;
    private float CalcForce(float x0, float x1)
    {
        float dist = System.Math.Abs(x0 - x1) - 0.5f;
        float D = (float)System.Math.Sqrt(0.43 * 0.43 - 4 * 0.173 * (-2.58 - dist));
        return (-0.43f + D) / (2f * 0.18f);
    }

    protected override void Start()
    {
        base.Start();
        hasGrenades = true;
        hitAnim = "Hit";
    }

    protected override void Update()
    {
        base.Update();
        if (hasGrenades)
        {
            if (hitTimer > 0)
            {
                hitTimer -= Time.deltaTime;
            }
            ResetAfterMissedTarget();
        }
       
    }

    public override void DoDamage(IDamage obj)
    {
        if (target && hitTimer <= 0f)
        {
            animator.Play(hitAnim);
            SpeedResetToZero();
            Invoke(nameof(DoThrow), 1f);
            hitTimer = hitPeriod;
        }
    }

    public virtual void DoThrow()
    {
        if (!target)
            return;
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();
        IMovable b = target.GetComponent<IMovable>();
        
        if (b == null)
            SpeedRestore();

        if (b != null)
        {
            targetPoint.x = System.Math.Abs(transform.position.x - b.GetPosition().x) < 1 ? b.GetPosition().x : b.GetPosition().x + b.GetSpeed() * 1f;
            //Debug.Log(b.GetPosition().x + b.GetSpeed() * 1f);
        }

        projectile.Launch(CalcForce(transform.position.x, targetPoint.x), this.damage, direction, GetLine(), this, isFriend);
    }

    protected virtual void ResetAfterMissedTarget()
    {
        if (!target)
        {
            SpeedRestore();
            target = null;
        }
    }

    public override void ChangeAnimationToWalk()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            animator.Play("Movement");
    }

    public override void ChangeAnimationToIdle()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            animator.Play("Idle");
    }

    public override void ReturnToBase()
    {
        base.ReturnToBase();
        target = null;
        targetPoint = new Vector2(1000, 1000);
    }
}
