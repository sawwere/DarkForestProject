using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClickedEvent : MonoBehaviour
{
    private PlayerController player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            EnemyCharacter enemy;
            if (hit.transform != null 
                && (enemy = hit.transform.gameObject.GetComponent<EnemyCharacter>()) 
                && (hit.collider.transform.gameObject.name.CompareTo("ClickZone") == 0))
            {
                //Debug.Log(hit.collider.transform.gameObject.name);
            }
        }
    }
}
