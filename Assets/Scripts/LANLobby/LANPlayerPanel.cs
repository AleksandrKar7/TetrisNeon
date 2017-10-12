using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LANPlayerPanel : MonoBehaviour
{
    public int NumPanel;
    public Image ImgAvatar;
    public Text TxtNickName;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetNickName(string nickName)
    {
        TxtNickName.text = nickName;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + "Установлен никнейм для " + NumPanel + " панели! Никнейм: " + TxtNickName.text);
    }

    public void SetAvatar(string pathToAvatar)
    {
        try
        {
            if (File.Exists(pathToAvatar))
            {
                WWW www = new WWW("file://" + pathToAvatar);

                ImgAvatar.sprite = Sprite.Create(((www.texture)),
                             new Rect(1, 1, ((www.texture)).width - 1, ((www.texture)).height - 1),
                             new Vector2());
            }
            else
            {
                Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + "Аватар для " + NumPanel + " панели не найден! Путь к автару: " + pathToAvatar);
                return;
            }
        }
        catch(Exception e)
        {
            Debug.LogError(DateTime.Now.ToString("hh:mm:ss:ffff") + "Ошибка при установке аватара на " + NumPanel + " панели! Путь к автару: " + pathToAvatar + ". " + e.ToString());
        }
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + "Аватар на " + NumPanel + " панели установлен успешно! Путь к автару: " + pathToAvatar);
    }

    public void CleanPanel()
    {
        TxtNickName.text = "-";
        ImgAvatar.sprite = null;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + "Очищена " + NumPanel + " панели!");
    }
}
