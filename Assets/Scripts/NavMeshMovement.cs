using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class NavMeshMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    public float Speed;
    public float MoveSpeedAnimationFactor = 1;

    public float RangeDistance;
    public float StopDistance;

    [ReadOnly] public Transform Target;

    [ReadOnly] public bool isStopped;
    [ReadOnly] public bool reachedTarget;

    void Start()
    {
        agent.speed = Speed;
        agent.stoppingDistance = StopDistance;
    }

    public void SetTarget(Transform target)
    {
        Target = target;

        if (!agent.enabled) return;

        SetDestination(target.position);

        float distance = Vector3.Distance(transform.position, Target.position);

        if (distance < RangeDistance)
            Resume();
        else
            Stop();

        if (distance < StopDistance)
        {
            reachedTarget = true;
            Stop();
        }
        else
            reachedTarget = false;
    }

    public void UpdatePostition()
    {
        if (!agent.enabled) return;
        if (Target != null)
        {
            SetDestination(Target.position);

            float distance = Vector3.Distance(transform.position, Target.position);

            if (distance < RangeDistance)
                Resume();
            else
                Stop();

            if (distance < StopDistance)
            {
                reachedTarget = true;
                Stop();
            }
            else
                reachedTarget = false;
        }
        else
        {
            reachedTarget = false;
            Stop();
        }

    }

    public void SetDestination(Vector3 destination)
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            NavMeshPath path = ComputePath(destination);
            if (path.status == NavMeshPathStatus.PathInvalid)
                agent.SetDestination(destination);
            else
                agent.SetPath(path);
        }
    }

    public NavMeshPath ComputePath(Vector3 toGo)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(agent.transform.position, toGo, NavMesh.AllAreas, path);
        return path;
    }

    public Vector3 GetVelocity()
    {
        return agent.velocity;
    }

    public void Disable()
    {
        agent.enabled = false;
    }

    public void Enable()
    {
        agent.enabled = true;
    }

    public void Stop()
    {
        agent.isStopped = true;
        isStopped = true;
    }

    public void Resume()
    {
        agent.isStopped = false;
        isStopped = false;
    }
}
