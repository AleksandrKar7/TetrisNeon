using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessageBoxProfileSelect : MonoBehaviour
{
    public GameObject MessageBox;
    public Text TxtMessage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowMessageBox(string Txt)
    {
        TxtMessage.text = Txt;
        MessageBox.transform.position = new Vector3(0, 0, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выведено диалоговое окно: " + MessageBox.name + " .");
    }

    public void ButtonClick_MBOk()
    {
        MessageBox.transform.position = new Vector3(1500, -350, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Скрыто диалоговое окно: " + MessageBox.name + " .");
    }

    public void ButtonClick_MBQuitFromGame()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил покинуть приложение");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выход из приложения.");

        Application.Quit();
        SceneManager.LoadScene((int)SceneManagerHelper.SceneManagerEnum.MainMenu);
    }
}
