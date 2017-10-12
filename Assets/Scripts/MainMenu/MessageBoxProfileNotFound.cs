﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageBoxProfileNotFound : MonoBehaviour {

    public GameObject MessageBox;
    public Canvas CanvasMainMenu;
    public bool itShow;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ShowMessageBox()
    {
        itShow = true;
        MessageBox.transform.position = new Vector3(0, 0, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выведено диалоговое окно: " + MessageBox.name + " .");
    }

    public void ButtonClick_OK()
    {
        itShow = false;
        MessageBox.transform.position = new Vector3(1500, -1000, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Скрыто диалоговое окно: " + MessageBox.name + " .");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.ProfileSelect.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.ProfileSelect.ToString());
    }
}
