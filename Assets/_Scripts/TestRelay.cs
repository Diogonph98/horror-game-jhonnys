using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System;

public class TestRelay : MonoBehaviour
{

    //public TMP_InputField input_field;
    public string CODE; 






    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        
    }

    public async void createRelay()
    {
        Debug.Log("creating relay");
        string joinCode = "";
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);

            Debug.Log("here");
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);


            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            CODE = joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }



    }

    public async void joinRelay(string codeInputted)
    {
        string joinCode = codeInputted;
        //input_field.DeactivateInputField(true);

        try
        {
            Debug.Log("Joining Relay with: " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
            //CODE = "";


        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

}
