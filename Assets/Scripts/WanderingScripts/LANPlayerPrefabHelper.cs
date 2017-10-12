using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LANPlayerPrefabHelper : NetworkBehaviour
{
    public LANPlayerPrefabLobby LANPPL;
    private PlayerProfile PP;

    public bool ItServer;
    public bool ItLocalPlayer;
    public bool ItClient;

    public static int ArrayLenght = 3;
    private /*public*/ LANPlayerPrefabHelper[] LANPPHArray = new LANPlayerPrefabHelper[ArrayLenght];
    public LANPlayerPrefabHelper GetLANPlayerArrayElement(int i)
    {
        if (i < ArrayLenght)
        {
            return LANPPHArray[i];
        }
        return null;
    }
    public void FindAndSetLANPlayerArrayElenent()
    {
        if (isLocalPlayer && isServer)
        {
            LANPlayerPrefabHelper[] lANPPLArray = FindObjectsOfType<LANPlayerPrefabHelper>();

            for (int i = 0; i < LANPPHArray.Length; i++)
            {
                LANPPHArray[i] = null;
            }

            for (int i = lANPPLArray.Length - 1; i >= 0; i--)
            {
                int j = lANPPLArray.Length - 1 - i;
                LANPPHArray[j] = lANPPLArray[i];
            }
        }
    }

    public LANPlayerPrefabHelper FindHostLANPlayerPrefabHelper()
    {
        LANPlayerPrefabHelper[] lANPlayerPrefabHelper = FindObjectsOfType<LANPlayerPrefabHelper>();
        for (int i = 0; i < lANPlayerPrefabHelper.Length; i++)
        {
            if (lANPlayerPrefabHelper[i].isServer && lANPlayerPrefabHelper[i].isLocalPlayer)
            {
                return lANPlayerPrefabHelper[i];
            }
        }
        return null;
    }


    private /*public*/ string IdPlayer;
    public string GetIdPlayer()
    {
        return IdPlayer;
    }
    public void SetIdPlayer(string idPlayer)
    {
        if(!isLocalPlayer)
            IdPlayer = idPlayer;
    }

    private /*public*/ string NickName;
    public string GetNickName()
    {
        return NickName;
    }
    public void SetNickName(string nickName)
    {
        if (!isLocalPlayer)
            NickName = nickName;
    }

    private string PathToAvatar;
    public string GetPathToAvatar()
    {
        //if (!isLocalPlayer)
        return PathToAvatar;
    }

    /*private*/ public Sprite SpriteAvatar;
    public Sprite GetSpriteAvatar()
    {
        return SpriteAvatar;
    }
    public void SetSpriteAvatar(Sprite spriteAvatar)
    {
        SpriteAvatar = spriteAvatar;
    }

    // Use this for initialization
    void Start()
    {
        ItServer = isServer;
        ItLocalPlayer = isLocalPlayer;
        ItClient = isClient;
        if (isLocalPlayer)
        {
            PP = FindObjectOfType<PlayerProfile>();
            IdPlayer = this.GetInstanceID().ToString();
            NickName = PP.GetNickName();
            PathToAvatar = PP.GetAvatarPath();

            LANPPL.CmdSendChatMessage("Игрок " + NickName + " подключился к лобби");

            LANPPL.CmdSetIdPlayer(IdPlayer);
            LANPPL.CmdSetNickName(IdPlayer, NickName);
            LANPPL.SendImage(IdPlayer, PathToAvatar);

            LANPPL.CmdSetPlayerPanel(IdPlayer);

            LANPPL.CmdResendAllImage();
        }
    }

    public int GetNumById(string idPlayer)
    {
        for (int i = 0; i < LANPlayerPrefabHelper.ArrayLenght; i++)
        {
            if (LANPPHArray[i] != null)
            {
                if (LANPPHArray[i].GetIdPlayer() == idPlayer)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    string idPlayerWinner;
    bool itNewWinner;

    public void SetWinner(string idPlayer)
    {
        itNewWinner = true;
        idPlayerWinner = idPlayer;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят id победителя: " + idPlayerWinner);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneManagerHelper.SceneManagerEnum.LANLobby
            && SceneManager.GetActiveScene().isLoaded == true
            && isLocalPlayer 
            && isServer
            && itNewWinner)
        {
            LANPPL.CmdSendChatMessage("Игрок " + GetLANPlayerArrayElement(GetNumById(idPlayerWinner)).GetNickName() + " победил");
            itNewWinner = false;
        }
    }
}
