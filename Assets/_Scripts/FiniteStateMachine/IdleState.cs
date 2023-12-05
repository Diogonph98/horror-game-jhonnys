using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    [SerializeField] private bool canSeeThePlayer = false;
    [SerializeField] private State chaseState;
    [SerializeField] private StateManager stateManager;
    [SerializeField] private float viewAngle = 60f;
    int playerLayerMask;
    Vector3 forwardVector;
    Transform enemy;

    private void Start()
    {
        canSeeThePlayer = false;
        stateManager = transform.parent.GetComponent<StateManager>();
        forwardVector = stateManager.forwardVector;
        enemy = transform.root.transform;
        //viewAngle *= 2;
        playerLayerMask = (1 << 6);
    }
    public override State RunCurrentState()
    {
        if (!canSeeThePlayer)
        {
            float angleDiscrete = viewAngle / 2;
            float angle = -viewAngle;
            int iterations = 5;
            for (int i = 0; i < iterations; i++)
            {
                Vector3 direction = Quaternion.Euler(0, angle, 0) * stateManager.forwardVector;
                Color col = Color.red;
                RaycastHit hit;
                bool playerSeen = Physics.Raycast(transform.position, direction, out hit, 6f, playerLayerMask);
                Debug.DrawRay(transform.position, direction * 6, col);
                if(playerSeen)
                {
                    GameObject playerObjectSeen = hit.transform.gameObject;
                    stateManager.inTarget = playerObjectSeen;
                    canSeeThePlayer = true;
                    break;
                }

                angle += angleDiscrete;

            }
        }


        if (canSeeThePlayer)
            return RestartStateParameters(chaseState);
        else
        {
            return RestartStateParameters(this);
        }
    }

    private State RestartStateParameters(State stateToReturn)
    {
        canSeeThePlayer = false;
        //stateManager.inTarget = null;
        return stateToReturn;
    }
}
