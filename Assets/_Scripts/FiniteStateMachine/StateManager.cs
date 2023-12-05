using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private State currentState;
    public Vector3 forwardVector;
    public GameObject inTarget;

    private void Awake()
    {
        forwardVector = transform.root.transform.forward;
    }

    private void Start()
    {
        currentState.Start();
    }
    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        transform.root.transform.forward = forwardVector; 

        State nextState = currentState?.RunCurrentState();

        if(nextState != null)
        {
            SwitchToTheNextState(nextState);
        }
    }

    public void SwitchToTheNextState(State nextState)
    {
        if(nextState != currentState)
            nextState.Start();
        currentState = nextState;
    }
}
