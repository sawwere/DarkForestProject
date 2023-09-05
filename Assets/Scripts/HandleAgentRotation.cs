using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HandleAgentRotation : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void HandleRotation()
    {
        if (agent.hasPath && !(agent.pathStatus == NavMeshPathStatus.PathInvalid))
        {
            Vector3 diff = new Vector3(agent.path.corners[1].x, agent.path.corners[1].y, 0) - transform.position;
            var nv = new Vector3(diff.x, diff.y, 0);
            transform.right = nv;
            //debugInfoCanvas.transform.rotation = Quaternion.identity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
    }
}
