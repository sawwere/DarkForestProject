using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Rabbit : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    float targetSpeed = 0.0f;

    Vector3 wanderTarget = Vector3.zero;
    public GameObject jitterCircle;
    GameObject jitter;


    bool isHiding = false;
    Vector3 spawnPoint;

    Canvas debugInfoCanvas;

    void StopHiding()
    {
        debugInfoCanvas.GetComponentInChildren<TMP_Text>().text = "WANDERING";
        isHiding = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        debugInfoCanvas = GetComponentInChildren<Canvas>();
        

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        targetSpeed = agent.speed;

        jitter = Instantiate(jitterCircle);
        wanderTarget = gameObject.transform.position;

        StopHiding();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - transform.position;

        float relativeHeading = Vector3.Angle(transform.right, transform.TransformVector(target.transform.right));
        float toTarget = Vector3.Angle(transform.right, transform.TransformVector(targetDir));
        if ((toTarget > 90 && relativeHeading < 20) || targetSpeed < 0.01f)
        { 
            Seek(target.transform.position);
            return;
        }
        float lookAhead = targetDir.magnitude/(agent.speed + targetSpeed);
        Seek(target.transform.position + target.transform.right * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + targetSpeed);
        Flee(target.transform.position + target.transform.right * lookAhead);
    }

    void Wander()
    {
        float wanderRadius = 1f;
        float wanderDistance = 12.0f;
        float wanderJitter = 0.001f;
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f) * wanderJitter, 0);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(wanderDistance, 0.0f, 0.0f);
        Vector3 targetWorld = transform.TransformVector(targetLocal) + spawnPoint;

        Debug.DrawLine(transform.position, targetWorld, Color.red);
        jitter.transform.position = targetWorld;
        Seek(targetWorld);
    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        for (int i = 0; i < WorldObstacles.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = WorldObstacles.Instance.GetHidingSpots()[i].transform.position 
                - target.transform.position;
            Vector3 hidePos = WorldObstacles.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 3; ;

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Seek(chosenSpot);
    }

    void CleverHide()
    { 
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDirection = Vector3.zero;
        GameObject chosenGameObject = WorldObstacles.Instance.GetHidingSpots()[0];
        for (int i = 0; i < WorldObstacles.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = WorldObstacles.Instance.GetHidingSpots()[i].transform.position
                - target.transform.position;
            Vector3 hidePos = WorldObstacles.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 3;

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDirection = hideDir;
                chosenGameObject = WorldObstacles.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }
        Ray2D backRay = new Ray2D(chosenSpot, -chosenDirection.normalized);
        float distance = 20.0f;
        var cc = Physics2D.Raycast(chosenSpot, backRay.direction, distance);
        Vector3 hidePosition = new Vector3(cc.point.x, cc.point.y, 0.0f);

        Debug.DrawRay(hidePosition, backRay.direction, Color.blue);
        Debug.DrawLine(transform.position, hidePosition, Color.red);

        Seek(hidePosition + chosenDirection.normalized);
    }

    public bool CanSeeTarget()
    {
        RaycastHit2D[] info = new RaycastHit2D[2];
        Vector3 toTarget = target.transform.position - transform.position;
        float lookAngle = Vector3.Angle(transform.right, toTarget);
        int count = GetComponent<Collider2D>().Raycast(toTarget.normalized, info, 10.0f);
        if (count > 0 && lookAngle < 60)
        {
            if (info[0].transform.gameObject.tag == "Player" || info[0].transform.gameObject.tag == "AggressiveAI")
            {
                return true;
            }
        }
        return false;
    }

    public bool CanSeeMe()
    {
        RaycastHit2D[] info = new RaycastHit2D[2];
        Vector3 toMe = transform.position - target.transform.position;
        float lookAngle = Vector3.Angle(target.transform.right, toMe);
        int count = target.GetComponent<Collider2D>().Raycast(toMe.normalized, info, 10.0f);
        if (count > 0 && lookAngle < 60)
        {
            return true;
        }
        return false;
    }

    void HandleRotation()
    {
        if (agent.hasPath && !(agent.pathStatus == NavMeshPathStatus.PathInvalid))
        {
            Vector3 diff = new Vector3(agent.path.corners[1].x, agent.path.corners[1].y, 0) - transform.position;
            var nv = new Vector3(diff.x, diff.y, 0);
            transform.right = nv;
            debugInfoCanvas.transform.rotation = Quaternion.identity;
        }
    }

    bool TargetInRange()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 10.0f)
            return true;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHiding)
        {
            Debug.Log($"{CanSeeTarget()} {CanSeeMe()}");
            
            if (CanSeeTarget() && CanSeeMe())
            {
                debugInfoCanvas.GetComponentInChildren<TMP_Text>().text = "HIDING";
                CleverHide();
                isHiding = true;
                Invoke("StopHiding", 6.0f);
            }
            else
            {
                Wander();
            }
        }
            
        HandleRotation();
    }
}
