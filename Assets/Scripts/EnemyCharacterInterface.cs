using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacterInterface : CharacterInterface
{
    enum State { Wander };

    [Header("Enemy")]
    public float    wanderRadius = 10.0f;

    State               state;
    NavMeshSurface      navMesh;
    Vector3             spawnPos;
    Vector3             pathDestination;
    Vector3             currentTarget;
    NavMeshQueryFilter  navMeshQueryFilter = new NavMeshQueryFilter();
    NavMeshPath         currentPath;
    int                 currentPathPos;

    protected void Start()
    {
        navMesh = GameObject.FindObjectOfType<NavMeshSurface>();
        navMeshQueryFilter.agentTypeID = navMesh.agentTypeID;
        navMeshQueryFilter.areaMask = navMesh.layerMask;

        spawnPos = transform.position;
        pathDestination = currentTarget = spawnPos;
    }

    protected override void ActualMove(Vector3 toMove)
    {
        transform.position += toMove * Time.fixedDeltaTime;
    }

    protected void Update()
    {
        if (character.isDead)
        {
            if (character.timeSinceDeath > 4.0f)
            {
                Destroy(gameObject);
            }
            return;
        }

        switch (state)
        {
            case State.Wander:
                Update_Patrol();
                break;
        }

        float distanceToTarget = Vector3.Distance(currentTarget, transform.position);

        if (currentPath != null)
        {
            // Move towards the current target
            if (distanceToTarget < 0.1f)
            {
                // Next target
                currentPathPos++;
                if (currentPathPos < currentPath.corners.Length)
                {
                    currentTarget = currentPath.corners[currentPathPos];
                }
                else
                {
                    // Done with this path
                    currentTarget = pathDestination = transform.position;
                }
            }
        }

        if (distanceToTarget > 0.1f)
        {
            Vector3 toTarget = (currentTarget - transform.position).x0z();
            toTarget = toTarget.normalized * Mathf.Clamp(toTarget.magnitude, 0.0f, moveSpeed * Time.deltaTime);

            Vector3 dest = transform.position + toTarget;
            
            character.SetDesiredDir(toTarget.xz() * 10.0f);

            deltaPos = toTarget / Time.fixedDeltaTime;
        }
    }
    
    public override void OnDeath()
    {
        character.EnableColliders(false);
        currentPath = null;
        currentTarget = pathDestination = transform.position;
        deltaPos = Vector3.zero;
    }

    void Update_Patrol()
    {
        if (Vector3.Distance(pathDestination, transform.position) < 0.1f)
        {
            Vector3 pos = spawnPos + Random.onUnitSphere.x0z() * wanderRadius;

            SetTarget(pos);
        }
    }

    void SetTarget(Vector3 pos)
    {
        currentPath = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, pos, navMeshQueryFilter, currentPath))
        {
            currentTarget = currentPath.corners[0];
            pathDestination = currentPath.corners[currentPath.corners.Length - 1];
            currentPathPos = 0;
        }
        else
        {
            currentTarget = transform.position;
            currentPath = null;
        }
    }
}
