using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileSelect : MonoBehaviour  //Данный клас используется для объединения всех панелей профилей и манипуляции над отдельными панелями
{
    public ProfilePanel[] ProfilePanels = new ProfilePanel[3];
    private PlayerProfile PP;
    public MessageBoxProfileSelect MBPS;  

    // Use this for initialization
    void Start ()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сцена: " + SceneManager.GetActiveScene().name + " загружена.");

        PP = FindObjectOfType<PlayerProfile>();

        if (PP.profileNum >= 0)
        {
            ProfilePanels[PP.profileNum].ButtonClick_SelectProfile();
        }

        else
        {
            SelectFirsExistenceProfile();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private bool ItExistenceAnyProfile()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на существование созданного профиля.");
        for (int i = 0; i < ProfilePanels.Length; i++)
        {
            string prefix = PlayerPreferencesHelper.GetPrefixProfileKey(i);
            if (PlayerPrefs.GetInt(prefix + PlayerPreferencesHelper.ProfilePrefEnum.ExistenceProfile, 0) == 1)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Есть созданные профили.");
                return true;
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Ни одного созданного профиля не обнаружено.");
        return false;   
    }

    public void SelectFirsExistenceProfile()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск и выбор первого существующего профиля.");
        for (int i = 0; i < ProfilePanels.Length; i++)
        {
            string prefix = PlayerPreferencesHelper.GetPrefixProfileKey(i);
            if (PlayerPrefs.GetInt(prefix + PlayerPreferencesHelper.ProfilePrefEnum.ExistenceProfile, 0) != 0)
            {
                ProfilePanels[i].ButtonClick_SelectProfile();
                return;
            }          
        }
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Ни одного созданного профиля не обнаружено!");
        PP.profileNum = -1;
    }

    public void UnselectedAll()
    {
        for (int i = 0; i < ProfilePanels.Length; i++)
        {
            ProfilePanels[i].UnselectedProfile();
        }
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Cнят фокус со всех профилей.");
    }

    public void ButtonClick_BackToMainMenu()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Возвращение в главное меню.");

        if (PP.profileNum == -1)
        {
            string textMessage = "Профиль не выбран! Пожалуйста, создайте новый профиль или выберите существующий!";

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отказано в возвращение в главное меню: " + textMessage);

            MBPS.ShowMessageBox(textMessage);
            return;
        }
        if (ItExistenceAnyProfile() == false)
        {
            string textMessage = "Не обнаружено существующих профилей! Пожалуйста, создайте новый профиль!";

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отказано в возвращение в главное меню: " + textMessage);

            MBPS.ShowMessageBox(textMessage);
            return;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());
    }

    public void DeleteProfile(int num)
    {
        if(ProfilePanels.Length < num)
        {
            ProfilePanels[num].DeleteProfile();
        }
    }
}
