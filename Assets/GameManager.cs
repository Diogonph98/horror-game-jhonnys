using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

   
    public TestRelay relay;
    private string lobbyCode;

    [SerializeField] private TMP_Text CodeText;

    public MenuManager menuManager;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.FindFirstObjectByType<MenuManager>();
        StartCoroutine(CheckConditions());
    }

    private IEnumerator CheckConditions()
    {
        yield return new WaitForSeconds(1);
        if (menuManager.isHosting)
        {
            Debug.Log("entered");
            relay.createRelay();
            StartCoroutine(getLobbyCodeInSeconds(3)); ;
        }
        else
        {
            string code = menuManager.LobbyCode;
            relay.joinRelay(code);
        }
    }

    private IEnumerator getLobbyCodeInSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        lobbyCode = relay.CODE;
        CodeText.text = lobbyCode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
