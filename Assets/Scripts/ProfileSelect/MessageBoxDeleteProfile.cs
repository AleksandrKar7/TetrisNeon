using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoxDeleteProfile : MonoBehaviour
{
    private int currentProfilePanel;
    public GameObject MBDPObject;
    public ProfilePanel[] PPArray = new ProfilePanel[3];

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowMessageBox(int currentProfile)
    {
        MBDPObject.transform.position = new Vector3(0, 0, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выведено диалоговое окно: " + MBDPObject.name + " .");

        currentProfilePanel = currentProfile;
    }

    public void ButtonClick_MBDPCancel()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь отказался от удаления " + currentProfilePanel + " профиля.");

        MBDPObject.transform.position = new Vector3(1500, -350, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Скрыто диалоговое окно: " + MBDPObject.name + " .");
    }

    public void ButtonClick_MBDPOk()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь согласился удалить " + currentProfilePanel + " профиль.");

        if (currentProfilePanel < PPArray.Length)
        {
            PPArray[currentProfilePanel].DeleteProfile();
        }

        MBDPObject.transform.position = new Vector3(1500, -350, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Скрыто диалоговое окно: " + MBDPObject.name + " .");
    }
}
