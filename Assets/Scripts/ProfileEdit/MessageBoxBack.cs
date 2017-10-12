using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessageBoxBack : MonoBehaviour
{
    public GameObject MessageBox;
    public Text TxtHeader;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowMessageBox()
    {
        MessageBox.transform.position = new Vector3(0, 0, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выведено диалоговое окно: " + MessageBox.name + " .");
    }

    public void ButtonClick_MBBack()
    {
        MessageBox.transform.position = new Vector3(1500, 350, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Скрыто диалоговое окно: " + MessageBox.name + " .");
    }

    public void ButtonClick_MBOk()
    {
        MessageBox.transform.position = new Vector3(1500, 350, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Скрыто диалоговое окно: " + MessageBox.name + " .");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.ProfileSelect.ToString());

        SceneManager.LoadScene((int)SceneManagerHelper.SceneManagerEnum.ProfileSelect);
    }
}
