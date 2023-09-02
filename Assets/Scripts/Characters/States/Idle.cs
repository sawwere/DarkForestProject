using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        Debug.Log("Idle");
        //animator.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }
        if (Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //animator.ResetTrigger("isIdle");
        base.Exit();
    }
}
