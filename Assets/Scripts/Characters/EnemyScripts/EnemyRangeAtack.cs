using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAtack : MonoBehaviour
{
    EnemyRange parentEnemy;

    private void Start()
    {
        parentEnemy = transform.parent.gameObject.GetComponent<EnemyRange>();
        GetComponent<BoxCollider2D>().offset = new Vector2( 1.5f, 0.2f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var obj = collision.gameObject.GetComponent<IDamage>();
        if (parentEnemy.target)
        {
            parentEnemy.DoDamage(obj);
        }
        else
        {
            if (obj != null 
                && obj.GetLine() == parentEnemy.GetLine()
                && (((1 << collision.gameObject.layer) & parentEnemy.aviableHitMask.value) != 0)
                && collision.transform.position.x * parentEnemy.transform.position.x > 0) // чтобы стрелял только по объектам со своей половины карты
            {
                SetTarget(collision.gameObject);
            }
        }
    }

    public void SetTarget(GameObject obj)
    {
        if (!parentEnemy.target)
        {
            parentEnemy.target = obj;
            parentEnemy.targetPoint = parentEnemy.target.transform.position;
            //Debug.Log("target has been found");
        }
    }
}
