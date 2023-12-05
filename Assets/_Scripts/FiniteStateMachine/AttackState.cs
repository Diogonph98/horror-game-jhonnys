using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{

    [SerializeField] private State chaseState;
    [SerializeField] private StateManager stateManager;
    [SerializeField] private bool playerEscaped = false;
    int playerLayerMask;
    private void Start()
    {
        playerLayerMask = (1 << 6);
    }
    public override State RunCurrentState()
    {
        bool playerInRange = Physics.CheckSphere(transform.position, 2.5f, playerLayerMask);

        if(!playerInRange)
        {
            playerEscaped = true;
        }

        if(playerEscaped)
        {
            return RestartStateParameters(chaseState);
        }
        else
        {
            Debug.Log("Attacked!");
            return RestartStateParameters(this);
        }

    }

    private State RestartStateParameters(State stateToReturn)
    {
        playerEscaped = false;
        //stateManager.inTarget = null;
        return stateToReturn;
    }
}
