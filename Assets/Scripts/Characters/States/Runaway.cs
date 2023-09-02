using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Runaway : State
{
    public Runaway(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.RUNAWAY;
        agent.speed = 4;
    }

    public override void Enter()
    {
        //animator.SetTrigger("isRunning");
        Debug.Log("Escape");
        agent.isStopped = false;
        agent.SetDestination(GameEnvironment.Singleton.SafePoint.transform.position);
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            nextState = new Idle(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //animator.ResetTrigger("isRunning");
        base.Exit();
    }
}
