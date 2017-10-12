using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LANMenuEsc : MonoBehaviour {

    public GameObject gObjLANMenuEsc;
    private LANPlayerPrefabGame currentPlayer;
    public void FindCurrentPlayer(/*LANPlayerPrefabGame currentPlayer*/)
    {
        foreach(LANPlayerPrefabGame player in FindObjectsOfType<LANPlayerPrefabGame>())
        {
            if (player.isLocalPlayer)
            {
                currentPlayer = player;
                break;
            }
        }
    }

	// Use this for initialization
	void Start () {
        FindCurrentPlayer();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowMessageBox()
    {
        gObjLANMenuEsc.transform.position = FindObjectOfType<Camera>().transform.position;
        gObjLANMenuEsc.transform.position += new Vector3(0, 0, 5);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выведено диалоговое окно: " + gObjLANMenuEsc.name + " .");
    }

    public void HideMessageBox()
    {
        gObjLANMenuEsc.transform.position = new Vector3(0, -1000, 0);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Скрыто диалоговое окно: " + gObjLANMenuEsc.name + " .");
    }

    public void ButtonClick_Resume()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил продолжить игру");

        currentPlayer.itGamePouse = false;
        HideMessageBox();
    }

    public void ButtonClick_Disconnect()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил покинуть игру");

        NetworkManager NM = FindObjectOfType<NetworkManager>();
        if (currentPlayer.isServer)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Остановка сервера");

            NM.StopHost();
        }
        else
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Остановка клиента");

            NM.StopClient();
        }
    }

    public void ButtonClick_Surrender()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил сдаться");

        currentPlayer.PlayerSurrend();
    }
}
