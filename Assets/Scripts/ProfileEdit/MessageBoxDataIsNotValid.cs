using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxDataIsNotValid : MonoBehaviour {

    public GameObject MessageBox;
    public Text TxtHeader;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowMessageBox(string Text)
    {
        TxtHeader.text = Text;
        MessageBox.transform.position = new Vector3(0, 0, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выведено диалоговое окно: " + MessageBox.name 
            + " с сообщением : "  + TxtHeader.text);
    }

    public void ButtonClick_MBOk()
    {
        MessageBox.transform.position = new Vector3(1500, -350, -2);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Скрыто диалоговое окно: " + MessageBox.name + " .");
    }
}
