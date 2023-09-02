using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK, SLEEP, RUNAWAY
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator animator;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;

    float visibilityDistance = 15.0f;
    float visibilityAngle = 30.0f;
    float shootDist = 3.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        animator = _anim;
        player = _player;
    }

    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public State Process()
    {
        if (stage == EVENT.ENTER)
            Enter();
        if (stage == EVENT.UPDATE)
            Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool IsPlayerBehind()
    {
        Vector3 direction = npc.transform.position - player.position;
        float angle = Vector3.Angle(direction, npc.transform.right);
        if (direction.magnitude < 2
            && angle < 30)
        {
            return true;
        }
        return false;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.right);
        if (direction.magnitude < visibilityDistance
            && angle < visibilityAngle)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < shootDist)
        {
            return true;
        }
        return false;
    }
}
