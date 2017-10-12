using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LANBroadcastReceiver : MonoBehaviour
{
    public bool isAlive;
    public double frequency = 0.25;
    private double lastTick = 0;
    IPEndPoint ep;
    IPEndPoint SendEP;
    UdpClient client;
    UdpClient clientSend;
    public LANServerBook lanServerBook;

    public bool notFullServer = false;
    public bool notServerWithPassword = false;

    // Use this for initialization
    void Start ()
    {
        ep = new IPEndPoint(IPAddress.Any, 7778);
        client = new UdpClient(ep);

        SendEP = new IPEndPoint(IPAddress.Broadcast, 7778);
        clientSend = new UdpClient();
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        string search = "Search";

        byte[] buffer = Encoding.UTF8.GetBytes(search);
        clientSend.Send(buffer, buffer.Length, SendEP);

        IPEndPoint remoteServer = null;
        byte[] buff = client.Receive(ref remoteServer);
        string str;
        str = (Encoding.UTF8.GetString(buff, 0, buff.Length));

        if (str != null && str != "")
        {
            String[] words = str.Split(new string[] { @"\*/" }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length <= 1)
            {
                return;
            }

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + "Принято сообщение из Broadcast"
                + "Name: " + words[0]
                + " \n IPAddress: " + words[1]
                + " \n Count Player: " + words[2]
                + " \n Max Count Player: " + words[3]
                + " \n Ping: " + (long.Parse(words[4]) - (DateTime.Now.Ticks)) / 10000 + "мс"
                + " \n TimeServer " + long.Parse(words[4])
                + " \n DateTime.Now.Ticks / 10000 " + DateTime.Now.Ticks);

            if (notFullServer && (int.Parse(words[2]) == int.Parse(words[3])))
            {
                return;
            }

            lanServerBook.DataForVerification(words);
        }

        lastTick = Time.time;       
    }
}
