using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineGameMenu : MonoBehaviour {

    public MainMenuHelper MMH;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonClick_LANGame()
    {
        MMH.SetNewVectorCamera(MMH.GetCamLANGameMenuPos());

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Камера переведена на раздел локальной игры: " + MMH.GetNowVectorCamera().ToString());
    }

    public void ButtonClick_BackToMainMenu()
    {
        MMH.SetNewVectorCamera(MMH.GetCamMainMenuPos());

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Камера переведена на раздел главного меню: " + MMH.GetNowVectorCamera().ToString());
    }
}
