using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class ProfileEdit : MonoBehaviour {

    public InputField IFNickname;
    public Text TxtStatusBarNickname;
    public Text TxtStatusBarAvatar;
    public Image ImgAvatar;
    public MessageBoxDataIsNotValid MBDINV;
    public MessageBoxBack MBB;
    private PlayerProfile PP;
    private bool NickNameIsValid = false;
    private bool AvatarIsValid = false;
    // Use this for initialization
    void Start()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сцена: " + SceneManager.GetActiveScene().name + " загружена.");

        PP = FindObjectOfType<PlayerProfile>();
        SetDataForEdit();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetDataForEdit()
    {
        if (PP.GetNickName() != null && PP.GetNickName() != "")
        {
            IFNickname.text = PP.GetNickName();
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлена информация для редактирования: "
                + "Никнейм: " + IFNickname.text);

            СheckInputNickName();
        }

        if (PP.GetSpriteAvatar() != null)
        {
            ImgAvatar.sprite = PP.GetSpriteAvatar();
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлена информация для редактирования: "
                + "Аватар: " + ImgAvatar.sprite.name);

            СheckInputAvatar(PP.GetAvatarPath());
        }
    }

    private bool СheckInputNickName()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начата проверка на валидность Никнеймa: " + IFNickname.text);
        if (IFNickname.text.Length == 0)
        {
            TxtStatusBarNickname.color = Color.red;
            TxtStatusBarNickname.text = "Никнейм не введен";
            NickNameIsValid = false;

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Провалена проверка на валидность Никнеймa: Никнейм " + IFNickname.text + " не введен");

            return false;
        }

        if (IFNickname.text.Length < 4)
        {
            TxtStatusBarNickname.color = Color.red;
            TxtStatusBarNickname.text = "Никнейм слишком короткий";
            NickNameIsValid = false;

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Провалена проверка на валидность Никнеймa: Никнейм " + IFNickname.text + " слишком короткий");

            return false;
        }

        if (IFNickname.text.Length > 16)
        {
            TxtStatusBarNickname.color = Color.red;
            TxtStatusBarNickname.text = "Никнейм слишком длинный";
            NickNameIsValid = false;

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Провалена проверка на валидность Никнеймa: Никнейм " + IFNickname.text + " слишком длинный");

            return false;
        }

        if (Regex.IsMatch(IFNickname.text, @"\W"))
        {
            TxtStatusBarNickname.color = Color.red;
            TxtStatusBarNickname.text = "Никнейм имеет недопустимые символы";
            NickNameIsValid = false;

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Провалена проверка на валидность Никнеймa: Никнейм " + IFNickname.text + " имеет недопустимые символы");


            return false;
        }

        else
        {
            TxtStatusBarNickname.color = Color.green;
            TxtStatusBarNickname.text = "Никнейм в порядке";
            NickNameIsValid = true;

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на валидность Никнеймa: Никнейм " + IFNickname.text + " в порядке");

            return true;
        }
    }

    public void CheckedInputNickNameRunTime()
    {
        СheckInputNickName();
    }

    public void EndInputNickName()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Завершен ввод никнейма");

        if (СheckInputNickName())
        {
            PP.SetNewNickName(IFNickname.text);
        }
    }

    public void ButtonClick_DownloadImage()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Открыт диалог для выбора аватара");

        string path = ShowOpenFileDialog();
        if (СheckInputAvatar(path))
        {
            CopyAndSetAvatar(path);
        }
    }

    private string ShowOpenFileDialog()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Открыт OpenFileDialog для выбора аватара");
        ExtensionFilter[] efs = new ExtensionFilter[1];
        efs[0] = new ExtensionFilter("Image File", "jpg", "jpeg", "png");
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Выберите картинку под аватарку", Application.dataPath, efs, false);

        string pathToFile = "";
        if (paths.Length > 0)
        {
            pathToFile = paths[0];
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Закрытие OpenFileDialog для выбора аватара и получен путь к картинке: " + pathToFile);

        return pathToFile;
    }

    private bool СheckInputAvatar(string pathToFile)
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начата проверка на валидность Аватара: " + pathToFile);

        if (pathToFile.Length == 0)
        {
            AvatarIsValid = false;

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Провалена проверка на валидность Аватара: Путь не указан");

            return false;
        }

        if (!File.Exists(pathToFile))
        {
            TxtStatusBarAvatar.text = "Файл не найден";
            TxtStatusBarAvatar.color = Color.red;
            AvatarIsValid = false;

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Провалена проверка на валидность Аватара: Файл не найден " + pathToFile);

            return false;
        }

        if (File.OpenRead(pathToFile).Length > 1048576 / 2)
        {
            TxtStatusBarAvatar.text = "Файл слишком большой";
            TxtStatusBarAvatar.color = Color.red;
            AvatarIsValid = false;

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Провалена проверка на валидность Аватара: Файл слишком большой " + pathToFile);

            return false;
        }

        TxtStatusBarAvatar.text = "Аватар в порядке";
        TxtStatusBarAvatar.color = Color.green;

        //AvatarIsValid = true;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на валидность Аватара: Аватар в порядке " + pathToFile);

        return true;
    }

    private string pathToSaveAvatar = @"\Resources\Profile\";

    private void CopyAndSetAvatar(string pathToFile)
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начато копирование картинки из" + pathToFile + " во внутренние директории: " + Application.dataPath + pathToSaveAvatar);

        if(!Directory.Exists(Application.dataPath + pathToSaveAvatar))
        {
            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Не обнаружена внутренняя директория: " + Application.dataPath + pathToSaveAvatar);

            Directory.CreateDirectory(Application.dataPath + pathToSaveAvatar);

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Создана внутренняя директория: " + Application.dataPath + pathToSaveAvatar);
        }

        File.Copy(pathToFile, Application.dataPath + pathToSaveAvatar + "Avatar" + PP.profileNum + Path.GetExtension(pathToFile), true);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Завершено копирование. Полный путь к скопированному аватару: " + (Application.dataPath + pathToSaveAvatar + "Avatar" + PP.profileNum + Path.GetExtension(pathToFile)));

        try
        {
            if (File.Exists(Application.dataPath + pathToSaveAvatar + "Avatar" + PP.profileNum + Path.GetExtension(pathToFile)))
            {
                PP.SetNewPathAvatar("Avatar" + PP.profileNum + Path.GetExtension(pathToFile));

                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало установки аватара");

                WWW www = new WWW("file://" + PP.GetAvatarPath());

                Sprite sp = Sprite.Create(((www.texture)),
                     new Rect(1, 1, ((www.texture)).width - 1, ((www.texture)).height - 1),
                     new Vector2());

                ImgAvatar.sprite = sp;

                Resources.UnloadUnusedAssets();

                AvatarIsValid = true;
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Ошибка при попытке установить аватар: " + e.ToString());
            return;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Аватар установлен успешно");
    }

    public void ButtonClick_Save()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начата проверка на возможность сохранения изменений.");

        if (NickNameIsValid == true && AvatarIsValid == true)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на возможность сохранения изменений пройдена успешно");

            PP.SavePlayerProfile();
            SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.ProfileSelect.ToString());
        }

        else
        {
            string Text = "";

            if(NickNameIsValid == false)
            {
                Text = "Никнейм введен не правильно! \n";
            }

            if (AvatarIsValid == false)
            {
                Text += "Аватар не загружен!";
            }

            Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на возможность сохранения изменений проваленна: " + Text + "");

            MBDINV.ShowMessageBox(Text);
        }
    }   

    public void ButtonClick_Back()
    {
        MBB.ShowMessageBox();
    }
}
