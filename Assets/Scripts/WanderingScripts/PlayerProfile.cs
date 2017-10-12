using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour {

    public int profileNum;

    private bool Primary = false;

    //private PlayerPreferencesHelper PPrefs = new PlayerPreferencesHelper();

    private string nickName;
    public string GetNickName()
    {
        return nickName;
    }
    public void SetNewNickName(string NickName)
    {
        nickName = NickName;
    }

    private string avatarPath;
    public string GetAvatarPath()
    {
        return avatarPath;
    }

    public void SetNewPathAvatar(string Path)
    {
        string prefixPath = PlayerPreferencesHelper.GetPrefixAvatarPath();
        avatarPath = prefixPath + Path;
    }
    
    private Sprite SpriteAvatar;
    public Sprite GetSpriteAvatar()
    {     
        return SpriteAvatar;
    }

    // Use this for initialization
    void Start () {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Временное хранилище для профиля активировано");
        
        profileNum = PlayerPrefs.GetInt(PlayerPreferencesHelper.ProfilePrefEnum.CurrentProfile.ToString(), -1);

        string prefix = PlayerPreferencesHelper.GetPrefixProfileKey(profileNum);

        if (PlayerPrefs.GetInt(prefix + PlayerPreferencesHelper.ProfilePrefEnum.ExistenceProfile, 0) == 0)
        {
            profileNum = -1;
        }

        DontDestroyOnLoad(this);

        if (FindObjectsOfType<PlayerProfile>().Length == 1)
        {
            Primary = true;
        }

        if(Primary == false)
        {
            Destroy(this.gameObject);
        }    
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializePlayerProfile(int ProfileNum)
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало инициализации временного хранилища для " + ProfileNum + " профиля");

        Resources.UnloadUnusedAssets();

        string prefix = PlayerPreferencesHelper.GetPrefixProfileKey(ProfileNum);
        string prefixPath = PlayerPreferencesHelper.GetPrefixAvatarPath();

        profileNum = ProfileNum;
        nickName = PlayerPrefs.GetString(prefix + PlayerPreferencesHelper.ProfilePrefEnum.NickName, "Noname");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Никнейм для инициализации временного хранилища " 
            + ProfileNum + "профиля успешно получен: " + nickName);

        avatarPath = PlayerPrefs.GetString(prefix + PlayerPreferencesHelper.ProfilePrefEnum.Avatar, prefixPath + "NoImage.png");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Путь к аватару для инициализации временного хранилища " 
            + ProfileNum + "профиля успешно получен: " + avatarPath);

        try
        {
            if (File.Exists(avatarPath))
            {
                WWW www = new WWW("file://" + avatarPath);

                SpriteAvatar = Sprite.Create(((www.texture)),
                             new Rect(1, 1, ((www.texture)).width - 1, ((www.texture)).height - 1),
                             new Vector2());

                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Спрайт автара " + ProfileNum + " профиля успешно создан");

            }
            else
            {
                Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Ошибка инициализации временного хранилища профлия " + ProfileNum + ": аватар не найден!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Ошибка инициализации временного хранилища профлия " + ProfileNum + ": " + e.ToString());
            return;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Инициализация временного хранилища для " + ProfileNum + " профиля успешно завершена");

    }

    public void PreparingForCreationPlayerProfile(int ProfileNum)
    {
        profileNum = ProfileNum;
        nickName = null;
        avatarPath = null;

        SpriteAvatar = null;
        Resources.UnloadUnusedAssets();

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Очищено временное хранилище для создания " + ProfileNum + " профиля");
    }


    public void SavePlayerProfile()
    {
        Resources.UnloadUnusedAssets();

        string prefix = PlayerPreferencesHelper.GetPrefixProfileKey(profileNum);

        PlayerPrefs.SetInt(prefix + PlayerPreferencesHelper.ProfilePrefEnum.ExistenceProfile, 1);
        PlayerPrefs.SetString(prefix + PlayerPreferencesHelper.ProfilePrefEnum.NickName, nickName);
        PlayerPrefs.SetString(prefix + PlayerPreferencesHelper.ProfilePrefEnum.Avatar, avatarPath);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сохранены изменения для " + profileNum + " профиля");
    }

    public void DeletePlayerProfile(int profileNum)
    {     
        Resources.UnloadUnusedAssets();

        string prefix = PlayerPreferencesHelper.GetPrefixProfileKey(profileNum);

        File.Delete(PlayerPrefs.GetString(prefix + PlayerPreferencesHelper.ProfilePrefEnum.Avatar, null));

        PlayerPrefs.SetInt(prefix + PlayerPreferencesHelper.ProfilePrefEnum.ExistenceProfile, 0);
        PlayerPrefs.SetString(prefix + PlayerPreferencesHelper.ProfilePrefEnum.NickName, null);
        PlayerPrefs.SetString(prefix + PlayerPreferencesHelper.ProfilePrefEnum.Avatar, null);
        PlayerPrefs.SetInt(prefix + PlayerPreferencesHelper.ProfilePrefEnum.RecordClassic, 0);
        PlayerPrefs.SetInt(prefix + PlayerPreferencesHelper.ProfilePrefEnum.RecordHardCore, 0);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Профиль" + profileNum + " удален");
    }
}
