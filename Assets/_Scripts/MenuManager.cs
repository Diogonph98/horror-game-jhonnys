using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager _instance = null;
    public static MenuManager Instance
    {
        get { return _instance; }
    }


    [Header("Lobby Conditions:")]
    public string LobbyCode;
    public bool isHosting = false;
    [SerializeField] private TMP_InputField codeField;


    [Header("Menu Panels:")]
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject JoinPanel;

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //Application.targetFrameRate = 60;
        MenuManager[] objs = GameObject.FindObjectsByType<MenuManager>(FindObjectsSortMode.InstanceID);

        if (objs.Length > 1)
        {
            for (int i = 0; i < objs.Length - 1; i++)
            {
                Destroy(objs[i].gameObject);
            }
        }
    }

    public void JoinLobbyButton()
    {
        LobbyCode = codeField.text;
        SceneManager.LoadScene(1);
    }

    public void HostButton()
    {
        isHosting = true;
        SceneManager.LoadScene(1);
    }

    public void JoinButton()
    {
        MenuPanel.SetActive(false);
        JoinPanel.SetActive(true);
    }

    public void ReturnButton()
    {
        if(JoinPanel.activeInHierarchy)
        {
            JoinPanel.SetActive(false);
            MenuPanel.SetActive(true);
        }
    }
}
