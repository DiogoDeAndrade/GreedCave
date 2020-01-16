using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    enum State { Wander };

    [Header("Enemy")]
    public float    wanderRadius = 10.0f;

    State           state;
    NavMeshAgent    agent;
    Vector3         spawnPos;

    protected override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();

        spawnPos = transform.position;
    }

    protected override void Update()
    {
        switch (state)
        {
            case State.Wander:
                Update_Patrol();
                break;
        }

        lastMoveDir = agent.velocity.xz();
        desiredDir = lastMoveDir * agent.velocity.magnitude;

        base.Update();
    }

    void Update_Patrol()
    {
        if (agent.remainingDistance < 0.1f)
        {
            // New waypoint
            Vector3 pos = spawnPos + Random.onUnitSphere.x0z() * wanderRadius;

            agent.SetDestination(pos);
        }
    }
}
