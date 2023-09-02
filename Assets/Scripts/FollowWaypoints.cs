using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowWaypoints : MonoBehaviour {

    //Transform goal;
    //public float speed = 2.0f;
    //public float accuracy = 1.0f;
    //public float rotationSpeed = 180.0f;
    GameObject[] wps;
    GameObject currentNode;
    //int currentWP = 0;
    //Graph g;

    public GameObject wpManager;

    NavMeshAgent agent;

    void Start() {
        //Time.timeScale = 5.0f;
        wps = wpManager.GetComponent<WPManager>().waypoints;
        currentNode = wps[0];


        agent = this.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        Goto3();
    }

    public void Goto0() {

        agent.SetDestination(wps[0].transform.position);
    }

    public void Goto1() {

        agent.SetDestination(wps[1].transform.position);
    }

    public void Goto3() {

        agent.SetDestination(wps[3].transform.position);
    }

    public void Goto5() {

        agent.SetDestination(wps[5].transform.position);
    }

    //void ProgressTracker()
    //{
    //    if (g.pathList.Count == 0 || currentWP == g.pathList.Count)
    //        return;
    //    if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead)
    //        return;
    //    if (Vector3.Distance(tracker.transform.position, g.pathList[currentWP].getID().transform.position) < accuracy)
    //        currentWP++;
    //    if (currentWP < g.pathList.Count)
    //    {
    //        goal = g.pathList[currentWP].getID().transform;
    //        Vector3 lookAtGoal = new Vector3(
    //            goal.position.x,
    //            transform.position.y,
    //            goal.position.z);
    //        Vector3 direction = lookAtGoal - this.transform.position;

    //        //transform.rotation = Quaternion.Slerp(
    //        //    this.transform.rotation,
    //        //    Quaternion.LookRotation(direction),
    //        //    Time.deltaTime * rotSpeed);

    //    }
    //    Debug.Log(currentWP);
    //    tracker.transform.LookAt(goal);
    //    tracker.transform.Translate(0, 0, (speed + 0.1f) * Time.deltaTime);
    //}

    void Update()
    {
        //Debug.Log(transform.position);
        if (agent.hasPath && !(agent.pathStatus == NavMeshPathStatus.PathInvalid))
        {
            //Debug.Log(agent.path.corners[0]);
            Vector3 diff = new Vector3(agent.path.corners[1].x , agent.path.corners[1].y, 0) - transform.position;
            var nv = new Vector3(diff.x, diff.y, 0);
            transform.up = nv;
        }        
    }

    //void LateUpdate()
    //{
    //    if (g.pathList.Count == 0 || currentWP == g.pathList.Count) 
    //        return;
    //    currentNode = g.getPathPoint(currentWP);
    //    ProgressTracker();
    //    Debug.Log(currentWP);

    //    // rotate along y so Z looks at target then rotate back resulting X looking at the target
    //    Vector3 vectorToTarget = tracker.transform.position - transform.position;
    //    Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
    //    Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //    transform.Translate(speed * Time.deltaTime, 0, 0);
    //}

    //void LateUpdate() {

    //    if (g.pathList.Count == 0 || currentWP == g.pathList.Count) return;

    //    currentNode = g.getPathPoint(currentWP);

    //    if (Vector3.Distance(g.pathList[currentWP].getID().transform.position, transform.position) < accuracy) {

    //        currentWP++;
    //    }

    //    if (currentWP < g.pathList.Count) {

    //        goal = g.pathList[currentWP].getID().transform;
    //        Vector3 lookAtGoal = new Vector3(
    //            goal.position.x,
    //            transform.position.y,
    //            goal.position.z);

    //        Vector3 direction = lookAtGoal - this.transform.position;

    //        transform.rotation = Quaternion.Slerp(
    //            this.transform.rotation,
    //            Quaternion.LookRotation(direction),
    //            Time.deltaTime * rotSpeed);

    //        transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
    //    }
    //}
}
