using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessageBoxGameOver : MonoBehaviour {

    public GameObject MessageBox;
    public Text TxtHeader;
    public Camera MainCamera;

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ShowMessageBox(string message)
    {
        TxtHeader.text = message;
        MessageBox.transform.position = new Vector3(MainCamera.transform.position.x,
            MainCamera.transform.position.y, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выведено диалоговое окно: " + MessageBox.name + " .");
    }

    public void ButtonClick_MBRestart()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил начать заново");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.SingleGame.ToString() + " .");

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.SingleGame.ToString());
    }

    public void ButtonClick_MBBackToMainMenu()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил вернуться в главное меню");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.MainMenu.ToString() + " .");

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());
    }
}
