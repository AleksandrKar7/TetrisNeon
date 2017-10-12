using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LANBroadcastTransmitter : MonoBehaviour
{
    private LANPlayerPrefabLobby Host;
    public bool isAlive;
    public double frequency = 1;
    private double lastTick = 0;
    UdpClient client;
    IPEndPoint ep; 

    public void SetHost(LANPlayerPrefabLobby host)
    {
        Host = host;
        client = new UdpClient();
        ep = new IPEndPoint(IPAddress.Broadcast, 7778);
    }
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //isAlive = thSender.IsAlive;
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneManagerHelper.SceneManagerEnum.LANLobby
            && isAlive == true
            && Time.time - lastTick >= frequency)
        {
            Send();
            lastTick = Time.time;
        }
        else if(SceneManager.GetActiveScene().buildIndex != (int)SceneManagerHelper.SceneManagerEnum.LANLobby)
        {
            isAlive = false;
        }    
    }

    private void Send()
    {
        string str = "";

        str = Host.GetMassageToSend();

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сервер отправил сообщение: " + str);

        byte[] buff = Encoding.UTF8.GetBytes(str);
        client.Send(buff, buff.Length, ep);
    }
}
