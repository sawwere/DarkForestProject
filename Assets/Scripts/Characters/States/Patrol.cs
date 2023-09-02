using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        Debug.Log("Patrol start");
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float dist = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if (dist < lastDist)
            {
                currentIndex = i;
                lastDist = dist;
            }    
        }
        // bc Update increase index by 1
        currentIndex--;
        //animator.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }
        if (IsPlayerBehind())
        {
            nextState = new Runaway(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }
        if (agent.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
                currentIndex++;

            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }
    }

    public override void Exit()
    {
        //animator.ResetTrigger("isWalking");
        base.Exit();
    }
}
