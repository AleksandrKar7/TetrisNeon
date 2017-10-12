using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpeedRegulator : MonoBehaviour {

    public Text TxtSpeedValue;
    private int Speed;
    private int MaxSpeed = 10;
    private int MinSpeed = 0;
    private SingleGameHelper SGH;
	// Use this for initialization
	void Start () {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сцена: " + SceneManager.GetActiveScene().name + " загружена.");

        SGH = FindObjectOfType<SingleGameHelper>();

        Speed = 0;
        TxtSpeedValue.text = Speed.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonClick_UPSpeed()
    {
        if(Speed < MaxSpeed)
        {
            Speed++;
            TxtSpeedValue.text = Speed.ToString();

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Увеличена скорость. " 
                + "Текущее значение: " + Speed.ToString());
            return;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Достигнута максимальная скорось: " + Speed);
    }

    public void ButtonClick_DownSpeed()
    {
        if (Speed > MinSpeed)
        {
            Speed--;
            TxtSpeedValue.text = Speed.ToString();

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Уменьшена скорость. "
                + "Текущее значение: " + Speed.ToString());
            return;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Достигнута минимальная скорось: " + Speed);
    }

    public void ButtonClick_BackToMainMenu()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.MainMenu.ToString() + " .");

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());
    }

    public void ButtonClick_Start()
    {
        SGH.SetGameSpeed(Speed);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.SingleGame.ToString() + " .");

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.SingleGame.ToString());
        //SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());
    }
}
