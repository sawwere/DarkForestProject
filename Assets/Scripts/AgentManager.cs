using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentManager : MonoBehaviour
{
    List<NavMeshAgent> agents = new List<NavMeshAgent>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject aa in a)
        {
            agents.Add(aa.GetComponent<NavMeshAgent>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Debug.Log(mousePos2D);
            {
                foreach (NavMeshAgent a in agents)
                    a.SetDestination(mousePos2D);
            }
        }
    }
}
