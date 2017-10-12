using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LANPlayerPrefabLobby : NetworkBehaviour
{
    public LANPlayerPrefabHelper LANPPH;
    private LANLobby LANL;
    private NetworkManager NM;
    private LANBroadcastTransmitter LANBT;

    string PathToSaveTempAvatar = @"\Resources\TempImages\";

    // Use this for initialization
    void Start()
    {
        if(isLocalPlayer && Directory.Exists(Application.dataPath + PathToSaveTempAvatar))
        {
            Directory.Delete(Application.dataPath + PathToSaveTempAvatar, true);

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Директория для сохранения временных файлов: " + Application.dataPath + PathToSaveTempAvatar + " удалена");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (NM == null)
        {
            NM = FindObjectOfType<NetworkManager>();
        }

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneManagerHelper.SceneManagerEnum.LANLobby
          && LANL == null)
        {
            LANL = FindObjectOfType<LANLobby>();
            if (isServer && isLocalPlayer)
            {
                GameObject LANBroadcastTransmitter = new GameObject("LANBroadcastTransmitter");
                LANBT = LANBroadcastTransmitter.AddComponent<LANBroadcastTransmitter>();

                LANBT.SetHost(this);
                LANBT.isAlive = true;
            
                if(isLocalPlayer 
                    && isServer)
                {
                    ResetPlayerPanels();
                }

                Debug.Log("Start");
            }
        }
        if (isLocalPlayer && LANL.GetCurrentPlayer() == null)
        {
            LANL.SetCurrentPlayer(this);
        }
    }

    public string GetMassageToSend()
    {
        string str = "";
        str = LANPPH.GetNickName()
               + @"\*/" + Dns.GetHostEntry(Dns.GetHostName()).HostName
               + @"\*/" + NM.numPlayers
               + @"\*/" + NM.maxConnections
               + @"\*/" + (DateTime.Now.Ticks);
        //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": : " + str);

        return str;
    }

    [Command(channel = 0)]
    public void CmdSendChatMessage(string message)
    {
        RpcSendChatMessage(message);
    }

    [ClientRpc(channel = 0)]
    public void RpcSendChatMessage(string message)
    {
        LANL.TxtChat.text += "\n" + message;
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": В чат записано новое сообщение: " + message);
    }

    [Command(channel = 0)]
    public void CmdSetNickName(string idPlayer, string nickName)
    {
        LANPPH.SetNickName(nickName);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят никнейм " + nickName + " от игрока с id: " + idPlayer);
    }

    [Command(channel = 0)]
    public void CmdDownloadAvatar(string idPlayer, int[] intArray, int count, long lenght, bool lastPackage, bool rewrite)
    {
        FindHostLANPlayerPrefabLobby().RpcDownloadAvatar(GetNumPlayer(idPlayer), idPlayer, intArray, count, lenght, lastPackage, rewrite);
        if (lastPackage)
        {
            FindHostLANPlayerPrefabLobby().CmdSetPlayerPanel(idPlayer);
        }
    }

    [ClientRpc(channel = 0)]
    public void RpcDownloadAvatar(int i, string idPlayer, int[] intArray, int count, long lenght, bool lastPackage, bool rewrite)
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят пакет от игрока с id: " + idPlayer);

        if (!Directory.Exists(Application.dataPath + PathToSaveTempAvatar))
        {
            Directory.CreateDirectory(Application.dataPath + PathToSaveTempAvatar);
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Создана директоия для временных файлов: " + Application.dataPath + PathToSaveTempAvatar);
        }

        FileInfo FI = new FileInfo(Application.dataPath + PathToSaveTempAvatar + idPlayer + ".png");
        if ((File.Exists(Application.dataPath + PathToSaveTempAvatar + idPlayer + ".png") && FI.Length == lenght && lastPackage != true))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пакет отклонен т.к. файл уже существует: " + Application.dataPath + PathToSaveTempAvatar);

            return;
        }

        if (idPlayer != null)
        {
            using (FileStream output = new FileStream(Application.dataPath + PathToSaveTempAvatar + idPlayer + ".png", FileMode.Append, FileAccess.Write))
            {
                if (lastPackage == false)
                {
                    output.Write(ConvertIntArrToByteArr(intArray), 0, count);
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Записан пакет байто от игрока с id: " + idPlayer + " в файл: " + Application.dataPath + PathToSaveTempAvatar);
                }
                else
                {
                    if (output.Length != lenght)
                    {
                        Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Файл " + output.Name + " поврежден или был отправлен повторно! Не достаточно " + (lenght - output.Length) + " байт");
                    }
                    output.Close();
                    if (isServer)
                    {
                        Sprite sp = null;
                        try
                        {
                            WWW www = new WWW("file://" + Application.dataPath + PathToSaveTempAvatar + idPlayer + ".png");

                            sp = Sprite.Create(((www.texture)),
                                 new Rect(1, 1, ((www.texture)).width - 1, ((www.texture)).height - 1),
                                 new Vector2());
                        }
                        catch
                        {
                            Debug.LogError(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Ошибка при создании спрайта из: " + Application.dataPath + PathToSaveTempAvatar + idPlayer + ".png");
                        }
                        LANPPH.SetSpriteAvatar(sp);
                    }
                }
            }
        }
    }

    int packageSize = 512;

    private byte[] ConvertIntArrToByteArr(int[] intArr)
    {
        byte[] byteArr = new byte[packageSize];
        for (int i = 0; i < intArr.Length; i++)
        {
            byteArr[i] = (byte)intArr[i];
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Конвертирован целочисленный массив в массив байтов");
        return byteArr;
    }

    public void SendImage(string idPlayer, string pathToFile)
    {
        if (File.Exists(pathToFile))
        {
            using (FileStream input = new FileStream(pathToFile, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[packageSize];
                int count = 0;
                int[] bufferInt = new int[packageSize];
                while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < bufferInt.Length; i++)
                    {
                        bufferInt[i] = 0;
                        bufferInt[i] = buffer[i];
                    }

                    CmdDownloadAvatar(idPlayer, bufferInt, count, input.Length, false, false);

                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отправлен пакет байтов на сервер");
                }
                if (count <= 0)
                {
                    input.Close();
                    CmdDownloadAvatar(idPlayer, bufferInt, count, 0, true, false);

                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отправлен последний пакет байтов на сервер");
                }
            }
        }
    }

    [Command(channel = 0)]
    public void CmdSetIdPlayer(string idPlayer)
    {
        LANPPH.SetIdPlayer(idPlayer);
        LANPPH.FindHostLANPlayerPrefabHelper().FindAndSetLANPlayerArrayElenent();
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": На сервер принято id: " + idPlayer);
    }

    public int GetNumPlayer(string idPlayer)
    {
        LANPlayerPrefabHelper lanpph = LANPPH.FindHostLANPlayerPrefabHelper();
        return lanpph.GetNumById(idPlayer);
    }

    private LANPlayerPrefabLobby FindHostLANPlayerPrefabLobby()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск лобби объекта хоста");

        LANPlayerPrefabLobby[] lANPlayerPrefabLobby = FindObjectsOfType<LANPlayerPrefabLobby>();
        for (int i = 0; i < lANPlayerPrefabLobby.Length; i++)
        {
            if (lANPlayerPrefabLobby[i].isServer && lANPlayerPrefabLobby[i].isLocalPlayer)
            {         
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск лобби объекта хоста дал результат: " + lANPlayerPrefabLobby[i]);

                return lANPlayerPrefabLobby[i];
            }
        }

        Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск лобби объекта хоста НЕ дал результатов! Отработка метода происходит НЕ на машине хоста!");

        return null;
    }

    [Command(channel = 0)]
    public void CmdSetPlayerPanel(string idPlayer)
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принято сообщение на сервер с требованием установить панель для игрока с id:" 
            + idPlayer);

        LANPlayerPrefabLobby Host = FindHostLANPlayerPrefabLobby();
        int i = GetNumPlayer(idPlayer);
        Host.RpcSetPlayerPanel(i, idPlayer, Host.LANPPH.GetLANPlayerArrayElement(i).GetNickName());
    }

    [ClientRpc(channel = 0)]
    private void RpcSetPlayerPanel(int i, string idPlayer, string NickName)
    {
        LANL.playerPanels[i].SetNickName(NickName);
        LANL.playerPanels[i].SetAvatar(Application.dataPath + PathToSaveTempAvatar + idPlayer + ".png");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлена информация для " + i + " панели игрока");
    }

    [Command(channel = 0)]
    public void CmdCleanPlayerPanel(int i)
    {
        LANPlayerPrefabLobby Host = FindHostLANPlayerPrefabLobby();
        Host.RpcCleanPlayerPanel(i);
    }

    [ClientRpc(channel = 0)]
    public void RpcCleanPlayerPanel(int i)
    {
        LANL.playerPanels[i].CleanPanel();
    }

    [Command(channel = 0)]
    public void CmdResendAllImage()
    {
        FindHostLANPlayerPrefabLobby().ResendAllImage();
        FindHostLANPlayerPrefabLobby().ResetPlayerPanels();       
    }

    public void ResendAllImage()
    {
        for (int i = 0; i < LANPlayerPrefabHelper.ArrayLenght; i++)
        {
            if (LANPPH.GetLANPlayerArrayElement(i) != null)
            {
                if (File.Exists(Application.dataPath + PathToSaveTempAvatar + LANPPH.GetLANPlayerArrayElement(i).GetIdPlayer() + ".png"))
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отправка файла всем игрокам: " + Application.dataPath + PathToSaveTempAvatar + LANPPH.GetLANPlayerArrayElement(i).GetIdPlayer() + ".png");

                    LANPPH.GetLANPlayerArrayElement(i).LANPPL.SendImage(LANPPH.GetLANPlayerArrayElement(i).GetIdPlayer(),
                        Application.dataPath + PathToSaveTempAvatar + LANPPH.GetLANPlayerArrayElement(i).GetIdPlayer() + ".png");
                }
            }
        }
    }

    private void OnDestroy()
    {
        if(SceneManager.GetActiveScene().buildIndex == (int)SceneManagerHelper.SceneManagerEnum.LANLobby)
        { 
            FindObjectOfType<LANPlayerPrefabLobby>().CmdOnDestroy();
        }
    }

    [Command(channel = 0)]
    private void CmdOnDestroy()
    {
        if (isServer)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Событие: игрок потерян");

                LANPPH.FindHostLANPlayerPrefabHelper().FindAndSetLANPlayerArrayElenent();
            FindHostLANPlayerPrefabLobby().ResetPlayerPanels();
        }
    }

    private void ResetPlayerPanels()
    {
        for (int i = 0; i < LANPlayerPrefabHelper.ArrayLenght; i++)
        {
            LANPlayerPrefabHelper lanpph = LANPPH.GetLANPlayerArrayElement(i);
            if (lanpph != null)
            {
                CmdSetPlayerPanel(lanpph.GetIdPlayer());
            }
            if (lanpph == null)
            {
                CmdCleanPlayerPanel(i);
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Обновлены значения панелей игроков. Очищены неактуальные панели");
    }

    [Command]
    public void CmdLoadGame()
    {
        RpcLoadGame();
    }

    [ClientRpc(channel = 0)]
    private void RpcLoadGame()
    {
        for (int i = 0; i < FindObjectsOfType<LANPlayerPrefabHelper>().Length; i++)
        {
            DontDestroyOnLoad(FindObjectsOfType<LANPlayerPrefabHelper>()[i]);
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: "
            + SceneManagerHelper.SceneManagerEnum.LANGame.ToString() + " .");

        SceneManager.LoadScene((int)SceneManagerHelper.SceneManagerEnum.LANGame);
    }
}