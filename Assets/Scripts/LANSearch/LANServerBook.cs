using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LANServerBook : MonoBehaviour {

    public GameObject[] serverBook = new GameObject[256];
    private string ServerPanelPath = "Prefabs/LANSearch/ServerPanel";

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DataForVerification(String[] array)
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получена сообщение о сервере. Начало обработки сообщения");

        if (array[1] != null && array[1] != "")
        {
            int i = FindPanelByIpAddress(array[1]);
            if (i == -1) 
            {
                int j = FindFirstEmptyPanel();
                if (j == -1)
                {
                    return;
                }
                else
                {
                    AddServerPanel(j, array);
                }
            }
            else
            {
                serverBook[i].GetComponent<LANServerPanel>().InitializePanel(array[0], array[1], int.Parse(array[2]), int.Parse(array[3]), double.Parse(array[4]));
            }
        }
    }

    private int FindPanelByIpAddress(string Address)
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск существующей панели по IPAddress: " + Address);
        for (int i = 0; i < serverBook.Length; i++)
        {
            if (serverBook[i] != null)
            {
                if (serverBook[i].GetComponent<LANServerPanel>().GetIpAddress() == Address)
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск существующей панели по IPAddress: " + Address + " дал результат: это" + i + " панель");

                    return i;
                }
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск существующей панели по IPAddress: " + Address + " не дал результатов");
        return -1;
    }

    private int FindFirstEmptyPanel()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск первой пустой ячейки для панели сервера");

        for (int i = 0; i < serverBook.Length; i++)
        {
            if(serverBook[i] == null)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск первой пустой ячейки для панели сервера дал резултат: " + i);

                return i;
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск первой пустой ячейки для панели сервера не дал результатов");
        return -1;
    }

    public void AddServerPanel(int j, String[] array)
    {
        serverBook[j] = (GameObject)Instantiate(Resources.Load(ServerPanelPath, typeof(GameObject))
            , new Vector3(0, 0, 0), Quaternion.identity);
        serverBook[j].transform.parent = this.gameObject.transform;
        serverBook[j].transform.position = new Vector3(125, (125 - 50 * j), 0);
        serverBook[j].GetComponent<LANServerPanel>().InitializePanel(array[0], array[1], int.Parse(array[2]), int.Parse(array[3]), double.Parse(array[4]));     
    }

    public void OnDestroyPlayerPanel(string ipAddress)
    {
        int i = FindPanelByIpAddress(ipAddress);
        serverBook[i] = null;
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Очистак ячейки от ссылки на несуществующую панель " + i);

        bool NotFound = true;
        do
        {
            NotFound = true;
            for (int j = 0; j < serverBook.Length - 1; j++)
            {
                if (serverBook[j] == null && serverBook[j + 1] != null)
                {
                    NotFound = false;
                    serverBook[j] = serverBook[j + 1];
                    serverBook[j + 1] = null;
                    serverBook[j].transform.position = new Vector3(125, (125 - 50 * j), 0);
                }
            }
        } while (NotFound == false);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Заполнен созданый пробел после удаления панели");
    }
}
