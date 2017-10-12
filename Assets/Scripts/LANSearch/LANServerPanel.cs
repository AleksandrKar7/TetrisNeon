using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LANServerPanel : MonoBehaviour
{
    private string Name;
    private string IpAddress;
    public string GetIpAddress()
    {
        return IpAddress;
    }
    private int NowCountPlayer;
    private int MaxCountPlayers;
    //private double Ping;

    public GameObject GOServerPanel;
    public Text TxtName;
    public Text TxtCountPlayer;
    public Text TxtPing;
    public Text TxtPassword;

    private NetworkManager NM;

    public double frequency = 1.5;
    private double lastTick = 0;

    Ping p;

    public void InitializePanel(string name, string ipAddress, int nowCountPlayer, int maxCountPlayers, double ping)
    {
        Name = name;
        IpAddress = ipAddress;

        NowCountPlayer = nowCountPlayer;
        MaxCountPlayers = maxCountPlayers;

        p = new Ping(ipAddress);

        TxtName.text = name;
        TxtCountPlayer.text = NowCountPlayer + "/" + MaxCountPlayers;

        TxtPing.text = p.isDone.ToString();
    
        lastTick = Time.time;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Созадан новая панель: "
            + "Имя: " + Name + "; "
            + "IpAddress: " + IpAddress + "; "
            + "Текущее кол-во игроков: " + NowCountPlayer + "; "
            + "Максимальное кол-во игроков: " + MaxCountPlayers + "; "
            + "Пинг: " + TxtPing.text + "; ");
    }

    

    // Use this for initialization
    void Start ()
    {
        NM = FindObjectOfType<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastTick >= frequency)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Самоуничтожение панели с IpAddress: " + IpAddress);

            FindObjectOfType<LANServerBook>().OnDestroyPlayerPanel(IpAddress);
            Destroy(GOServerPanel);
        }
    }

    public void ButtonClick_Join()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало подключения к серверу с IpAddress: " + IpAddress);

        NM.networkAddress = this.IpAddress;
        NM.StartClient();
    }
}
