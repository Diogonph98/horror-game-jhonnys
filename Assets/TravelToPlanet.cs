using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Unity.Netcode;
using System;

public class TravelToPlanet : NetworkBehaviour
{
    [SerializeField]
    private string m_SceneName;

    public delegate void DelegateMethod(); // This defines what type of method you're going to call.
    public DelegateMethod methodToCall;

    private PlayerMovement pMove;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        pMove = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsHost) return;
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_SceneName = "MoonScene";
            methodToCall = loadMoonSceneParameters;
            loadNewScene();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_SceneName = "RedPlanetScene";
            methodToCall = loadRedPlanetSceneParameters;
            loadNewScene();
        }
    }


    public void loadNewScene()
    {
        ResetAllClientsPositionsServerRpc();
    }

    [ServerRpc(RequireOwnership =false)]
    private void ResetAllClientsPositionsServerRpc()
    {
        ResetAllClientsPositionsClientRpc();
        var status = NetworkManager.SceneManager.LoadScene(m_SceneName,LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load {m_SceneName} " +
                  $"with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }

    [ClientRpc]
    private void ResetAllClientsPositionsClientRpc()
    {
        loadSceneParameters(methodToCall);
    }

    private void loadSceneParameters(DelegateMethod method)
    {
        method();
        
    }

    private void loadMoonSceneParameters()
    {
        transform.position = new Vector3(0f, 7.5f, 0f);
        transform.GetComponent<PlayerMovement>().gravityLoaded.y = pMove.earthGravity.y / 6;
        transform.GetComponent<PlayerMovement>().speed = pMove.earthSpeed / 1.5f;
        transform.GetComponent<PlayerMovement>().jumpForce = pMove.earthJumpForce* 3f;
        //transform.GetComponent<PlayerMovement>().jumpIterations = pMove.earthJumpIterations * 2;
    }

    private void loadRedPlanetSceneParameters()
    {
        transform.position = new Vector3(0f, 7.5f, 0f);
        pMove.gravityLoaded.y = pMove.earthGravity.y;
        pMove.speed = pMove.earthSpeed;
        pMove.jumpForce = pMove.earthJumpForce;
        //pMove.jumpIterations = pMove.earthJumpIterations;
    }
}
