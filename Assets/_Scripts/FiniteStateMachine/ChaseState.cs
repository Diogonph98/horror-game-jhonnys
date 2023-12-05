using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField]private bool isInRange = false;
    [SerializeField]private bool playerOutOfRange = false;
    [SerializeField]private State attackState;
    [SerializeField]private State idleState;
    int playerLayerMask;

    [SerializeField] private StateManager stateManager;
    [SerializeField] private float speed;

    private Rigidbody rb;

    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
        stateManager = transform.parent.GetComponent<StateManager>();
        playerLayerMask = (1 << 6);
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerOutOfRange = true;
        }
    }
    public override State RunCurrentState()
    {
        Vector3 directionToRun = (stateManager.inTarget.transform.position - transform.position).normalized;
        directionToRun.y = 0;
        rb.velocity = directionToRun * speed*10 * Time.fixedDeltaTime;
        stateManager.forwardVector = directionToRun;


        bool playerInRange = Physics.CheckSphere(transform.position, 1.5f,playerLayerMask);

        if(playerInRange)
        {
            isInRange = true;
        }

        if (isInRange)
        {
            return RestartStateParameters(attackState);
        }
        else if(playerOutOfRange)
        {
            return RestartStateParameters(idleState);
        }
        else
        {
            return RestartStateParameters(this);
        }

        
    }

    private State RestartStateParameters(State stateToReturn)
    {
        isInRange = false;
        playerOutOfRange = false;
        //stateManager.inTarget = null;
        return stateToReturn;
    }
}
