using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{
    public int profileNum;
    public GameObject GameObjectProfilePanel;
    public GameObject BtnProfileNotExist;
    public Text TxtProfileNickName;
    public Image ImgProfileAvatar;
    public MessageBoxDeleteProfile MBDP;
    public ProfileSelect PS;
    private PlayerProfile PP;

    private bool Selected = false;
    public bool GetSelected()
    {
        return Selected;
    }

    // Use this for initialization
    void Start()
    {
        PP = FindObjectOfType<PlayerProfile>();
        string prefix = PlayerPreferencesHelper.GetPrefixProfileKey(profileNum);
        if (PlayerPrefs.GetInt(prefix + PlayerPreferencesHelper.ProfilePrefEnum.ExistenceProfile, 0) != 0)
        {
            BtnProfileNotExist.SetActive(false);
        }

        InitializedPanel();     
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitializedPanel()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Заполнение панели " + profileNum + " профиля");
      
        string prefix = PlayerPreferencesHelper.GetPrefixProfileKey(profileNum);
        string prefixPath = PlayerPreferencesHelper.GetPrefixAvatarPath();
       
        TxtProfileNickName.text = PlayerPrefs.GetString(prefix + PlayerPreferencesHelper.ProfilePrefEnum.NickName, "Noname");
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получен и установлен никнейм для " + profileNum + " профиля:" + TxtProfileNickName.text);

        string avatarPath = PlayerPrefs.GetString(prefix + PlayerPreferencesHelper.ProfilePrefEnum.Avatar, prefixPath + "NoImage.png");
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получен путь к аватару для " + profileNum + " профиля");

        try
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Попытка установки аватара для " + profileNum + " профиля");
            if (File.Exists(avatarPath))
            {
                WWW www = new WWW("file://" + avatarPath);

                ImgProfileAvatar.sprite = Sprite.Create(((www.texture)),
                             new Rect(1, 1, ((www.texture)).width - 1, ((www.texture)).height - 1),
                             new Vector2());
            }
            else
            {
                ImgProfileAvatar = null;

                Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Аватар " + profileNum + " профиля не обнаружен");
                return;
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Не удалось загрузить картинку при инициализации предпросмота " + profileNum + " профиля: " + e.ToString());
            return;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Аватар для " + profileNum + " панели предпосмотра профиля установлен успешно");
    }

    public void ButtonClick_ProfileNotExist() 
    {
        PP.PreparingForCreationPlayerProfile(profileNum);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.ProfileEdit.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.ProfileEdit.ToString());
    }

    public void ButtonClick_SelectProfile()
    {
        if(PP == null)
        {
            PP = FindObjectOfType<PlayerProfile>();
        }
        PS.UnselectedAll();
        Selected = true;
        GameObjectProfilePanel.GetComponent<Image>().color = new Color(0, 0.45f, 0);
        PP.InitializePlayerProfile(profileNum);

        PlayerPrefs.SetInt(PlayerPreferencesHelper.ProfilePrefEnum.CurrentProfile.ToString(), profileNum);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователем выбран " + profileNum + " профиль");
    }

    public void UnselectedProfile()
    {
        Selected = false;
        GameObjectProfilePanel.GetComponent<Image>().color = new Color(0, 0.0f, 0);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Выбор снят с " + profileNum + " профиля");
    }

    public void ButtonClick_EditProfile()
    {
        PP.InitializePlayerProfile(profileNum);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: " 
            + SceneManagerHelper.SceneManagerEnum.ProfileEdit.ToString());

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.ProfileEdit.ToString());
    }

    public void ButtonClick_DeleteProfile()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пользователь решил удалить " + profileNum + " профиль");

        MBDP.ShowMessageBox(profileNum);
    }

    public void DeleteProfile()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало удаления " + profileNum + " профиля");

        TxtProfileNickName.text = null;
        ImgProfileAvatar.sprite = null;
        BtnProfileNotExist.SetActive(true);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Очищена панель " + profileNum + " профиля");

        PP.DeletePlayerProfile(profileNum);
        if (Selected)
        {
            PS.SelectFirsExistenceProfile();
        }    

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Удален " + profileNum + " профиль");
    }
}
