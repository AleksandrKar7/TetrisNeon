using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleGameMenu : MonoBehaviour {

    public MainMenuHelper MMH;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick_Classic()
    {
        CreateSingleGameHelper(SingleGameHelper.GameModeEnum.Classic);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.SpeedRegulator.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.SpeedRegulator.ToString());
    }

    public void ButtonClick_HardCore()
    {
        //MMH.SetNewVectorCamera(MMH.GetCamMainMenuPos());
        CreateSingleGameHelper(SingleGameHelper.GameModeEnum.HardCore);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.SpeedRegulator.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.SpeedRegulator.ToString());
    }

    public void CreateSingleGameHelper(SingleGameHelper.GameModeEnum GameMode)  //Создает временный объект для хранения выбранной пользователем скорости и режима игры
    {   
        GameObject SingleGameHelper = new GameObject("SingleGameHelper");
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Создан временный объект " + SingleGameHelper.name + " .");

        SingleGameHelper.AddComponent<SingleGameHelper>();
        SingleGameHelper.GetComponent<SingleGameHelper>().SetGameMode(GameMode);
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Задано значение для объекта " + SingleGameHelper.name 
            + " параметр GameMode: " + SingleGameHelper.GetComponent<SingleGameHelper>().GetGameMode());

        DontDestroyOnLoad(SingleGameHelper);
    }

    public void ButtonClick_BackToMainMenu()
    {
        MMH.SetNewVectorCamera(MMH.GetCamMainMenuPos());
    }
}
