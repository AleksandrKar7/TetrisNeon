using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LANSearch : MonoBehaviour {

    public LANBroadcastReceiver LANBR;
    public Toggle TogleNotFullServer;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleChange_NotFullServer()
    {
        LANBR.notFullServer = TogleNotFullServer.isOn;
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Измененно состояние сортировки 'Без полных серверов' :"
          + LANBR.notFullServer);
    }

    public void ButtonClick_BackToMainMenu()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: "
           + SceneManagerHelper.SceneManagerEnum.MainMenu.ToString() + " .");

        SceneManager.LoadScene((int)SceneManagerHelper.SceneManagerEnum.MainMenu);
    }
}
