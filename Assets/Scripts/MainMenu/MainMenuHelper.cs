using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHelper : MonoBehaviour {
    public Camera MainCam;

    private Vector3 camMainMenuPos = new Vector3(0, 0, -10);
    public Vector3 GetCamMainMenuPos()
    {
        return camMainMenuPos;
    }

    private Vector3 camSettingMenuPos = new Vector3(0, 900, -10);
    public Vector3 GetCamSettingMenuPos()
    {
        return camSettingMenuPos;
    }

    private Vector3 camSingleGameMenuPos = new Vector3(0, -900, -10);
    public Vector3 GetCamSingleGameMenuPos()
    {
        return camSingleGameMenuPos;
    }

    private Vector3 camOnlineGameMenuPos = new Vector3(1200, 0, -10);
    public Vector3 GetCamOnlineGameMenuPos()
    {
        return camOnlineGameMenuPos;
    }

    private Vector3 camLANGameMenuPos = new Vector3(1200, 900, -10);
    public Vector3 GetCamLANGameMenuPos()
    {
        return camLANGameMenuPos;
    }

    // Use this for initialization
    void Start ()
    {
        MainCam = FindObjectOfType<Camera>(); 
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void SetNewVectorCamera(Vector3 newVec)
    {
        MainCam.transform.position = newVec;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлен новый вектор для камеры: " + MainCam.transform.position.ToString() + " .");
    }

    public Vector3 GetNowVectorCamera()
    {
        return MainCam.transform.position;
    }
}
