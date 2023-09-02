using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    public Transform player;
    Animator animator;

    State currentState;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        animator = GetComponent<Animator>();

        currentState = new Idle(this.gameObject, agent, animator, player);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
        //agent.SetDestination(target.transform.position);
        if (agent.hasPath && !(agent.pathStatus == NavMeshPathStatus.PathInvalid))
        {
            Vector3 diff = new Vector3(agent.path.corners[1].x, agent.path.corners[1].y, 0) - transform.position;
            var nv = new Vector3(diff.x, diff.y, 0);
            transform.right = nv;
        }
    }
}
