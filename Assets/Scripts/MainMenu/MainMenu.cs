using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public MainMenuHelper MMH;
    public Text NickNamePreviev;
    public Image AvatarPreviev;
    public MessageBoxProfileNotFound MBPNF;
    private Sound sound;
    private PlayerProfile PP;
    public static bool ifFirst = false;
	// Use this for initialization
	void Start ()
    {
        if (ifFirst == false)
        {
            Debug.Log("Запуск приложения " + DateTime.Now.ToLongDateString() + " : " + DateTime.Now.ToLongTimeString());
            ifFirst = true;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сцена: " + SceneManager.GetActiveScene().name + " загружена.");

        ButtonMouse bm = FindObjectOfType<ButtonMouse>();
        Cursor.SetCursor(bm.noActiveCursor, new Vector2(0, 0), CursorMode.ForceSoftware);

        sound = FindObjectOfType<Sound>();
        sound.InitializedSound();
        int profileNum = PlayerPrefs.GetInt(PlayerPreferencesHelper.ProfilePrefEnum.CurrentProfile.ToString(), -1);

        PP = FindObjectOfType<PlayerProfile>();
        if (profileNum >= 0)
        {
            PP.InitializePlayerProfile(profileNum);
            FillingProfilePreview();
        }
        else
        {
            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Профиль не обнаружен!");

            MBPNF.ShowMessageBox();
        }
        

        Destroy(FindObjectOfType<SingleGameHelper>());
    }

    // Update is called once per frame
    void Update ()
    {

    }


    public void FillingProfilePreview()
    {
        if (PP.profileNum != -1)
        {
            try
            {
                AvatarPreviev.sprite = PP.GetSpriteAvatar();
                NickNamePreviev.text = PP.GetNickName();
            }
            catch(Exception e)
            {
                Debug.LogError("Ошибка при заполнения окна предпросмотра профиля: " + e.ToString());
                return;
            }
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Заполнено окно предпросмотра профиля: "
            + " NickName: " + NickNamePreviev.text + " ;"
            + " Avatar: " + AvatarPreviev.sprite.ToString() + " .");
        }
    }

    public void ButtonClick_SingleGame()
    {
        MMH.SetNewVectorCamera(MMH.GetCamSingleGameMenuPos());

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Камера переведена на раздел одиночной игры: " + MMH.GetNowVectorCamera().ToString() + " .");
    }

    public void ButtonClick_OnlineGame()
    {
        MMH.SetNewVectorCamera(MMH.GetCamOnlineGameMenuPos());

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Камера переведена на раздел онлайн игры: " + MMH.GetNowVectorCamera().ToString() + " .");
    }

    public void ButtonClick_Setting()
    {       
        MMH.SetNewVectorCamera(MMH.GetCamSettingMenuPos());

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Камера переведена на раздел настроек игры: " + MMH.GetNowVectorCamera().ToString() + " .");
    }

    public void ButtonClick_ProfilePreview()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.ProfileSelect.ToString() + " .");

        SceneManager.LoadScene((int)SceneManagerHelper.SceneManagerEnum.ProfileSelect);
    }

    public void ButtonClick_Exit()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выход из приложения.");

        Application.Quit();
    }
}
