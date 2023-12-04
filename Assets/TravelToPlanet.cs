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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsHost) return;
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_SceneName = "MoonScene";
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
        transform.position = new Vector3(0f, 7f, 0f);
        transform.GetComponent<PlayerMovement>().gravityApplied.y /= 3;
        transform.GetComponent<PlayerMovement>().speed /= 1.5f;
        transform.GetComponent<PlayerMovement>().jumpForce /= 1.5f;
        transform.GetComponent<PlayerMovement>().jumpIterations *= 2;
    }
}
