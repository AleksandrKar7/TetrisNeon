using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessageBoxSingleGameMenu : MonoBehaviour {

    public GameObject MessageBox;
    public Camera MainCamera;
    private Game game;

    // Use this for initialization
    void Start () {
        game = FindObjectOfType<Game>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowMessageBox()
    {
        MessageBox.transform.position = new Vector3(MainCamera.transform.position.x,
            MainCamera.transform.position.y + 12.5f, -2);    

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выведено диалоговое окно: " + MessageBox.name + " .");

        game.SetItGamePouse(true);
    }

    public void ButtonClick_MBResume()
    {
        MessageBox.transform.position = new Vector3(1500, 350, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Скрыто диалоговое окно: " + MessageBox.name + " .");

        game.SetItGamePouse(false);
    }

    public void ButtonClick_MBRestart()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил начать игру заново");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.SingleGame.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.SingleGame.ToString());
    }

    public void ButtonClick_MBRestartWithNewSpeed()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил начать игру заново с новой скоростью");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.SpeedRegulator.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.SpeedRegulator.ToString());
    }

    public void ButtonClick_MBBackToMainMenu()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил вернуться в главное меню");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());
    }
}
