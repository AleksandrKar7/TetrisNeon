using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LANLobby : MonoBehaviour {

    public InputField IFChat;
    public Text TxtChat;
    public LANPlayerPrefabLobby currentPlayer;
    private static int lenght = 3;
    private NetworkManager NM;
    public LANPlayerPanel[] playerPanels = new LANPlayerPanel[lenght];

    float timeNow = 0;
    int iterationLoadGame = 0;
    bool loadGameBool = false;
    int countPlayerAtStart = 0;

    //public 
    public void SetCurrentPlayer(LANPlayerPrefabLobby player)
    {      
        NM = FindObjectOfType<NetworkManager>();
        currentPlayer = player;
    }
    public LANPlayerPrefabLobby GetCurrentPlayer()
    {       
        return currentPlayer;
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сцена: " + SceneManager.GetActiveScene().name + " загружена.");
    }

    // Update is called once per frame
    void Update()
    {
        CheckTime();
    }

    public void SendMessage()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь " + currentPlayer.LANPPH.name
            + " отправил сообщение: '" + IFChat.text + "'");

        currentPlayer.CmdSendChatMessage(currentPlayer.LANPPH.GetNickName() + ": " + IFChat.text);
       
        IFChat.text = "";
    }

    public void ButtonClick_Disconnect()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь " + currentPlayer.LANPPH.name
            + " решил отключиться");
        if (NM != null && currentPlayer.isServer && currentPlayer.isLocalPlayer)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начата остановка сервера");
            NM.StopServer();
        }
        if (NM != null && !currentPlayer.isServer && currentPlayer.isLocalPlayer)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начата остановка клиента");

            NM.StopClient();
        }
        else
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: "
           + SceneManagerHelper.SceneManagerEnum.MainMenu.ToString() + " .");

            SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());
        }
    }

    public void ButtonClick_StartGame()
    {
        if (currentPlayer.isLocalPlayer && currentPlayer.isServer)
        {
            if (NM.numPlayers >= 2 && loadGameBool == false)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Хост начал игру. Начало отсчета до начала игры");

                countPlayerAtStart = NM.numPlayers;
                iterationLoadGame = 0;
                loadGameBool = true;
                currentPlayer.LANPPH.LANPPL.CmdSendChatMessage("Хост начал осчет до начала игры");
                return;
            }
            if(loadGameBool == true)
            {
                iterationLoadGame = 0;
                loadGameBool = false;

                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Хост остановил игру. Отсчет остановлен");

                currentPlayer.LANPPH.LANPPL.CmdSendChatMessage("Хост остановил остчет");
                return;
            }
            else
            {
                Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": В запуске игры отказано! Малое кол-во игроков: " + NM.numPlayers);
                currentPlayer.CmdSendChatMessage("Нужно больше игроков! Для такого случая есть одиночная игра!");
            }
        }
        if(currentPlayer.isLocalPlayer && !currentPlayer.isServer)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + currentPlayer.LANPPH.GetNickName() + " готов к игре");

            currentPlayer.CmdSendChatMessage(currentPlayer.LANPPH.GetNickName() + " готов к игре");
        }       
    }

    private void CheckTime()
    {
        if (loadGameBool == true && currentPlayer.isServer && countPlayerAtStart != NM.numPlayers)
        {
            iterationLoadGame = 0;
            loadGameBool = false;

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отсчет остановлен. Измененно количество игроков в лобби");

            currentPlayer.LANPPH.LANPPL.CmdSendChatMessage("Отсчет остановлен");
            return;
        }
        if (loadGameBool == true && Time.time - timeNow >= 1 && currentPlayer.isServer)
        {
            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + "Матч начнется через " + (3 - iterationLoadGame));

            currentPlayer.LANPPH.LANPPL.CmdSendChatMessage("Матч начнется через " + (3 - iterationLoadGame));
            
            if (iterationLoadGame == 3)
            {
                currentPlayer.CmdLoadGame();
            }
            iterationLoadGame++;
            timeNow = Time.time;
        } 
    }
}
