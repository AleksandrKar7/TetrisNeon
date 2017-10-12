using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LANGameMenu : MonoBehaviour {
    public MainMenuHelper MMH;
    private NetworkManager NM;
    // Use this for initialization
    void Start()
    {
        NM = FindObjectOfType<NetworkManager>();
        NM.networkAddress = Dns.GetHostEntry(Dns.GetHostName()).HostName;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick_CreateLobby()
    {
        NM.maxConnections = 3;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало создания лобби на " + NM.maxConnections + " игроков."
            + " И начало загрузки сцены сцены: " + SceneManagerHelper.SceneManagerEnum.LANLobby.ToString());

        NM.StartHost();
    }

    public void ButtonClick_SearchLobby()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.LANSearch.ToString());

        SceneManager.LoadScene((int)SceneManagerHelper.SceneManagerEnum.LANSearch);
    }

    public void ButtonClick_BackToMainMenu()
    {
        MMH.SetNewVectorCamera(MMH.GetCamOnlineGameMenuPos());

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Камера переведена на раздел главного меню: " + MMH.GetNowVectorCamera().ToString());
    }
}
