using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour {

    public MainMenuHelper MMH;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void ButtonClick_Sound()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.VolumeSetting.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.VolumeSetting.ToString());
    }

    public void ButtonClick_Profile()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.ProfileSelect.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.ProfileSelect.ToString());
    }

    public void ButtonClick_BackToMainMenu()
    {
        MMH.SetNewVectorCamera(MMH.GetCamMainMenuPos());
    }
}
