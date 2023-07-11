using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class NavMeshMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    public float Speed;

    public float RangeDistance;
    public float StopDistance;

    [ReadOnly] public Transform Target;

    void Start()
    {
        agent.speed = Speed;
        agent.stoppingDistance = StopDistance;
    }

    public void SetTarget(Transform target)
    {
        Target = target;

        SetDestination(target.position);

        if (Vector3.Distance(transform.position, target.position) < RangeDistance)
            Resume();
        else
            Stop();
    }

    public void UpdatePostition()
    {
        if (Target != null)
        {
            SetDestination(Target.position);

            if (Vector3.Distance(transform.position, Target.position) < RangeDistance)
                Resume();
            else
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
    }

    public void Resume()
    {
        agent.isStopped = false;
    }
}
