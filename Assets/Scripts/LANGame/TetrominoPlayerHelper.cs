//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Threading;
//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.Networking.NetworkSystem;
//using UnityEngine.SceneManagement;
////using UnityEditor.;


//public class TetrominoPlayerHelper : NetworkBehaviour
//{

//    private static string IdPlayer;
//    public string GetIdPlayer()
//    {
//        return IdPlayer;
//    }
//    private static string NickName;
//    public string GetNickName()
//    {
//        return NickName;
//    }
//    GameObject[][,] PlayerGrid = new GameObject[4][,];
//    GameObject GameScript;

//    // GameObject ChatHelper;
//    float periodSvrRpc = 0.2f; //как часто сервер шлёт обновление картинки клиентам, с.
//    float timeSvrRpcLast = 0;
//    private double gameSpeed = 5;   //скорость падения

//    private int fastDropCoefficient = 4;    //Коефициент ускорения падения при зажатой клавише
//    private double speedFastMoveHorizontal = 0.25;  //Коефициент ускорения горизонтального движения при зажатой клавише

//    float timeLastFall = 0;
//    float timeLastMoveHorizontal = 0;

//    float timeLast;
//    float time = 6f;

//    private GameObject[] NowTetrominoArr = new GameObject[4];
//    private GameObject[] PreviewTetrominoArr = new GameObject[4];
//    private GameObject[] StoneLineArr = new GameObject[4];

//    private static int gridMaxHeight = 20;  //Высота игрового поля  

//    internal void SetGameState(int v)
//    {
//        GameState = 0;
//    }

//    private static int gridMaxWidth = 10;   //Ширина игрового поля

//    public static Transform[][,] gridArr =
//    {
//        new Transform[gridMaxWidth, gridMaxHeight],
//        new Transform[gridMaxWidth, gridMaxHeight],
//        new Transform[gridMaxWidth, gridMaxHeight],
//        new Transform[gridMaxWidth, gridMaxHeight]
//    };

//    private bool[] GameStartArr = new bool[4];

//    private int ShiftFactorFrame = 20;

//    public static Vector3[] PosEnableTetra = new Vector3[4]{
//        new Vector3(-1,-1,-1),
//        new Vector3(-1,-1,-1),
//        new Vector3(-1,-1,-1),
//        new Vector3(-1,-1,-1)
//    };

//    public static string[] IdPlayers = new string[4];

//    public static string[] NickNamePlayers = new string[4];

//    public static Player[] Players = new Player[4];

//    private static bool[] DeleteTetra = new bool[4];

//    private static bool[] AccessDeleteRow = new bool[4];

//    string PrefixesTetrominoName = "Prefabs/TetrominoOnLANPlayer/";

//    private Vector3[] playerPositionArr = new Vector3[4];

//    private static bool[] GameOverArr = new bool[4];

//    private static bool GameOver = false;

//    private static int GameState = 0;

//    NetworkManager NM;

//    string Address;

//    // Use this for initialization
//    void Awake()
//    {
//        //gameHelper = GameObject.FindObjectOfType<GameHelper>();
//        //if (isLocalPlayer)
//        //{
//        //    gameHelper.CurrentPlayer = this;
//        //}
//    }

//    public string GetMassageToSend()
//    {
//        string str = "";
//        //if (this.isServer)
//        //{

//        str = this.GetNickName()
//               + @" " + /*NM.networkAddress*/ Address
//               + @" " + this.GetCountConnection()
//               + @" " + NM.maxConnections
//               + @" " + DateTime.Now.Ticks / 10000;
//        //}
//        //Network.player.ipAddress;
//        //FindObjectOfType<NetworkManager>().
//        return str;
//    }



//    public string GetIPAddress()
//    {
//        string str = "";
//        str = NM.networkAddress;
//        return str;
//    }

//    public string[] GetIdPlayersArr()
//    {
//        return IdPlayers;
//    }

//    public string[] GetNickNamePlayersArr()
//    {
//        return NickNamePlayers;
//    }

//    string pathToAssets;

//    void Start()
//    {
//        foreach (IPAddress ip in Dns.GetHostByName(Dns.GetHostName()).AddressList)
//        {
//            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork /*&& ip.AddressFamily == System.Net.Sockets.AddressFamily.DataLink*/)
//            {
//                Address = ip.ToString();
//                break;
//            }
//        }
//        pathToAssets = Application.dataPath + @"\";
//        Debug.Log(Application.dataPath);
//        //Directory.CreateDirectory(@"C:\Users\Big\Desktop\1\2\3\4\5");
//        if (this.isLocalPlayer)
//        {
//            NM = FindObjectOfType<NetworkManager>();
//            //MassageBox
//            //UnityEditor.EditorUtility.DisplayDialog("Потеряна связь с сервером", "", "Ok");
//            //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": netId " + this.netId);
//            //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": playerControllerId " + this.playerControllerId);

//            GameState = 0;
//            gameSpeed = (11 - gameSpeed) / 10;
//            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": GetInstanceID " + this.GetInstanceID());
//            IdPlayer = this.GetInstanceID().ToString();
//            NickName = PlayerPrefs.GetString("NickName", "Noname");
//            if (NickName == "" || NickName == null)
//            {
//                NickName = "Noname";
//            }
//            CmdCanPlayerConnected(IdPlayer);
//            if (this.isServer)
//            {
//                ClearTempDirectory();
//                CmdSetId(null, null, true);
//                //SendImage(IdPlayer, true);
//            }
//            //if (!this.isServer && this.isLocalPlayer)
//            //{
//            if (!this.isServer)
//            {
//                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Client sendId: " + IdPlayer);
//                CmdSetId(IdPlayer, NickName, false);
//                SendImage(IdPlayer, false);
//            }
//            //}
//            //else if(this.isServer && this.isLocalPlayer)
//            //{
//            //    CmdSetId(null, true);
//            //}
//            //else
//            //{
//            //    //FindObjectOfType<NetworkManager>().maxConnections = 0;
//            //    //CmdSetId(null, true);
//            //    //RpcGetId();
//            //}
//            // CmdSetId(IdPlayer, false);
//            //NetworkManager.singleton.OnStartHost();
//            //NetworkManager.singleton.StartServer();


//            //Debug,Log(Network.ip)
//            //playerId = "" + this.netId;
//            // gameHelper.PlayerConnect(playerId);
//            timeLast = Time.time;


//            //GameState++;
//            // game = (GameObject)NetworkManager.Instantiate(Resources.Load("Prefabs/TetrominoOnLAN/GameScript"));
//        }


//    }

//    private void ClearTempDirectory()  //Test
//    {
//        if (Directory.Exists(Application.dataPath + @"\" + PreficsDirectory))
//        {
//            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + @"\" + PreficsDirectory);
//            //Directory.CreateDirectory(@"C:\Users\Big\Desktop\TestImageStream\" + this.GetIdPlayer());
//            foreach (DirectoryInfo Dr in dirInfo.GetDirectories())
//            {
//                Dr.Delete(true);

//                //Debug.Log(Dir.Name);
//            }

//            foreach (FileInfo Fl in dirInfo.GetFiles())
//            {
//                Fl.Delete();
//            }
//        }
//    }

//    bool GameStart = false;
//    private bool EnableTetra;

//    public void ClearAllVariable()
//    {
//        //       private int[] limitStoneHeight = new int[4];   //Используется для правильной утановки каменной линии (HardCore Mode)

//        //private GameObject[] NowTetrominoArr = new GameObject[4];
//        //private GameObject[] PreviewTetrominoArr = new GameObject[4];
//        //private GameObject[] StoneLine = new GameObject[4];   //Каменная линия (HardCore Mode)
//        for (int i = 0; i < 4; i++)
//        {
//            //limitStoneHeight[i] = 0;
//            NowTetrominoArr[i] = null;
//            PreviewTetrominoArr[i] = null;
//            //StoneLine[i] = null;
//            GameOverArr[i] = false;

//            for (int j = 0; j < gridArr[i].GetLength(0); j++)
//            {
//                for (int k = 0; k < gridArr[i].GetLength(1); k++)
//                {
//                    gridArr[i][j, k] = null;
//                }
//            }
//        }

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        CheckInputKey();
//        //if (this.isServer)
//        //{
//        //    RpcGetId();
//        //}
//        //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": isServer: " + this.isServer + " LocalPlayer " + this.isLocalPlayer + " RpcGetIdBool " + RpcGetIdBool);
//        if (this.isLocalPlayer && !this.isServer && RpcGetIdBool)
//        {
//            //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": RpcGetIdBool " + RpcGetIdBool);
//            //Debug.LogError("Сработал");
//            RpcGetIdBool = false;
//            CmdSetId(this.GetIdPlayer(), this.GetNickName(), false);
//            SendImage(this.GetIdPlayer(), false);
//        }

//        if ((this.isLocalPlayer) && FindObjectOfType<ChatHelper>() != null && chatHelper == null)
//        {
//            chatHelper = GameObject.FindObjectOfType<ChatHelper>();
//            chatHelper.CurrentPlayer = this;

//            CmdSendMassage(NickName + " подключился к лобби");

//            if (this.isServer)
//            {
//                ClearTempDirectory();
//                CmdSetId(null, null, true);
//                //SendImage(IdPlayer, true);
//            }
//        }

//        if (GameState == 1)
//        {
//            if (gameHelper == null)
//            {
//                try
//                {
//                    gameHelper = GameObject.FindObjectOfType<GameHelper>();
//                    gameHelper.CurrentPlayer = this;
//                }
//                catch (Exception e)
//                {
//                    Debug.LogError(e);
//                }
//            }
//            //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Ку");
//            if ((isServer && isLocalPlayer) && /*timeLast + time < Time.time &&*/ GameStart == false /*&& AllPalyersConnected()*/)
//            {
//                //CmdSetId(GetIdPlayer(), true);
//                //RpcGetId();
//                RpcSetCammera(NetworkManager.singleton.numPlayers);
//                SpawnGame();
//                FindObjectOfType<GameOnLANV2>().SetHost(this);

//                FindObjectOfType<GameOnLANV2>().SetIdPlayer(IdPlayers);

//                RpcSetIdAndNickNamePlayersFromClient(IdPlayers, NickNamePlayers);
//                //Debug.Log(Network.connections.Length);

//                FindObjectOfType<GameOnLANV2>().ClearAllVariable();
//                GameOver = false;

//                FindObjectOfType<GameOnLANV2>().StartGame(NetworkManager.singleton.numPlayers);
//                GameStart = true;

//                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": connections " + NetworkManager.singleton.numPlayers);
//            }
//        }
//        if (this.isServer && GameStart == true)
//        {
//        }
//        if (!this.isServer && !this.isLocalPlayer)
//        {

//        }
//        //
//    }

//    public bool GamePouse = false;

//    void OnGUI()    //Меню Esc
//    {
//        if (GamePouse == true)
//        {
//            GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 32;
//            GUI.Box(new Rect(Screen.width / 2 - 150,
//               Screen.height / 2 - 150, 300, 325), "Меню");


//            if (GUI.Button(new Rect(Screen.width / 2 - 100,
//            Screen.height / 2 - 75, 200, 50), "Продолжить"))
//            {
//                //ButtonClickSound.Play();
//                GamePouse = false;
//            }

//            if (GUI.Button(new Rect(Screen.width / 2 - 100,
//            Screen.height / 2, 200, 50), "Сдаться"))
//            {
//                //ButtonClickSound.Play();
//                //SceneManager.LoadScene("SingleGameClassic");
//                GamePouse = false;
//                Cmdsurrender(this.GetIdPlayer());

//            }

//            if (GUI.Button(new Rect(Screen.width / 2 - 100,
//            Screen.height / 2 + 75, 200, 50), "Выйти"))
//            {
//                //ButtonClickSound.Play();
//                //SceneManager.LoadScene("Test Menu");
//                if (this.isServer && this.isLocalPlayer)
//                {
//                    GamePouse = false;
//                    //currentPlayer.RpcStopHost();
//                    this.SetGameState(0);

//                    foreach (string IdPlayer in this.GetIdPlayersArr())
//                    {
//                        if (this.GetIdPlayer() != IdPlayer)
//                        {
//                            this.CmdDisconect(IdPlayer);
//                        }
//                    }
//                    FindObjectOfType<NetworkManager>().StopHost();
//                }
//                else if (!this.isServer && this.isLocalPlayer)
//                {
//                    //CurrentPlayer.CmdSendMassage(CurrentPlayer.GetIdPlayer() + " покинул игру");
//                    this.SetGameState(0);
//                    Cmdsurrender(this.GetIdPlayer());
//                    this.CmdDisconect(this.GetIdPlayer());
//                    //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отклъючение");
//                    //Thread.Sleep(1000);
//                    //FindObjectOfType<NetworkManager>().StopClient();
//                }
//            }
//        }
//    }

//    //[Command (channel = 0)]


//    [ClientRpc(channel = 0)]
//    public void RpcSetCammera(int Connection)
//    {
//        Camera cam = FindObjectOfType<Camera>();
//        if (Connection != 3)
//        {
//            if (cam != null)
//            {
//                cam.transform.position = new Vector3(15, 13, -10);
//                cam.orthographicSize = 18;
//            }
//        }
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcSetIdAndNickNamePlayersFromClient(string[] idPlayers, string[] nickNamePlayers)
//    {
//        IdPlayers = idPlayers;
//        NickNamePlayers = nickNamePlayers;
//    }

//    [Command]
//    public void CmdCanPlayerConnected(string IdPlayer)
//    {
//        if ((FindObjectOfType<NetworkManager>().numPlayers > FindObjectOfType<NetworkManager>().maxConnections) && IdPlayer != null && IdPlayer != "")
//        {
//            RpcDisconect(IdPlayer);
//        }
//        if ((GameState == 1) && IdPlayer != null && IdPlayer != "")
//        {
//            RpcDisconect(IdPlayer);
//        }
//    }

//    public string GetStrListOfPlayers(string[] arr)
//    {
//        //string[] arr = this.GetIdPlayersArr();
//        //Debug.Log( " GetStrListOfPlayers "  + arr.Length);
//        string str = "";
//        for (int i = 0; i < arr.Length; i++)
//        {
//            //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": asddsarqwqerdxz");
//            str += arr[i] + " \n";
//            //ListOfPlayers.text 
//        }
//        //Debug.Log(str);
//        return str;
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcSetListOfPlayer(string str)
//    {
//        ChatHelper.ListOfPlayers.text = str;
//    }

//    [Command(channel = 0)]
//    public void CmdDestroy(string id)
//    {
//        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Уничтожен игрок: " + id);
//    }

//    public override void OnNetworkDestroy()
//    {
//        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Минус this.isServer:" + this.isServer);
//        if (this.isServer)
//        {
//            CmdSetId(null, null, true);
//            SendImage(this.GetIdPlayer(), true);
//            //RpcGetId();
//        }

//        base.OnNetworkDestroy();
//    }

//    public override void OnStopAuthority()
//    {
//        base.OnStopAuthority();
//        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": asdasd Отключился");
//    }

//    public bool RpcGetIdBool = false;

//    [ClientRpc(channel = 0)]
//    public void RpcGetId()
//    {
//        TetrominoPlayerHelper[] TPsH = FindObjectsOfType<TetrominoPlayerHelper>();
//        for (int i = 0; i < TPsH.Length; i++)
//        {
//            TPsH[i].RpcGetIdBool = true;
//        }
//    }

//    public int GetCountConnection()
//    {
//        int count = 0;
//        for (int i = 0; i < IdPlayers.Length; i++)
//        {
//            if (IdPlayers[i] != null && IdPlayers[i] != "")
//            {
//                count++;
//            }
//        }
//        return count;
//    }

//    ChatHelper chatHelper;
//    GameHelper gameHelper;

//    [Command(channel = 0)]
//    public void CmdSendMassage(string message)
//    {
//        RpcSendMassage(message);
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcSendMassage(string message)
//    {
//        //SceneManager.LoadScene("MainMenu");

//        ChatHelper.TextBlock.text += "\n" + message;
//    }

//    [Command(channel = 0)]
//    public void CmdClearChat()
//    {
//        RpcClearChat();
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcClearChat()
//    {
//        ChatHelper.TextBlock.text = "";
//    }

//    [Command]
//    public void CmdPlayersPanel(string[] id, string[] NickName)
//    {
//        //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": CmdListOfPlayers");
//        RpcPlayersPanel(id, NickName);
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcPlayersPanel(string[] id, string[] NickName)
//    {
//        for (int i = 0; i < id.Length; i++)
//        {
//            chatHelper = FindObjectOfType<ChatHelper>();
//            chatHelper.SetPlayerPanel(i, id[i], NickName[i]);
//        }
//        //ChatHelper.ListOfPlayers.text = list;
//    }

//    private bool AllPalyersConnected()
//    {
//        return false;
//    }

//    [Command]
//    public void CmdLoadGame()
//    {
//        RpcLoadGame();
//    }

//    [ClientRpc(channel = 0)]
//    private void RpcLoadGame()
//    {
//        GameState = 1;
//        //foreach(Object obj in FindObjectsOfType<TetrominoPlayerHelper>())
//        //{

//        //    DontDestroyOnLoad(FindObjectOfType<TetrominoPlayerHelper>());
//        //}
//        for (int i = 0; i < FindObjectsOfType<TetrominoPlayerHelper>().Length; i++)
//        {
//            DontDestroyOnLoad(FindObjectsOfType<TetrominoPlayerHelper>()[i]);
//        }

//        SceneManager.LoadScene("Test Scene V2");
//    }

//    public void SpawnGame()
//    {
//        if (this.isServer)
//        {
//            GameScript = (GameObject)NetworkManager.Instantiate(Resources.Load("Prefabs/TetrominoOnLAN/GameScript"));
//        }
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcStartGame()
//    {
//        if (isLocalPlayer)
//        {
//            GameStart = true;
//        }
//    }

//    void CheckInputKey()
//    {
//        if (this.isLocalPlayer /*&& GameStart*/ && GameOver == false && GameState != 0)
//        {
//            if (GamePouse == false)
//            {
//                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
//                {
//                    CmdMove(KeyCode.RightArrow, IdPlayer);
//                    timeLastMoveHorizontal = Time.time;
//                }

//                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
//                {
//                    CmdMove(KeyCode.LeftArrow, IdPlayer);
//                    timeLastMoveHorizontal = Time.time;
//                }

//                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
//                {
//                    CmdMove(KeyCode.UpArrow, IdPlayer);
//                }

//                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
//                {
//                    CmdMove(KeyCode.DownArrow, IdPlayer);
//                    timeLastFall = Time.time;
//                }

//                if ((double)Time.time - timeLastFall >= gameSpeed / fastDropCoefficient && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
//                {
//                    CmdMove(KeyCode.DownArrow, IdPlayer);
//                    timeLastFall = Time.time;
//                }

//                if ((double)Time.time - timeLastMoveHorizontal >= speedFastMoveHorizontal && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
//                {
//                    CmdMove(KeyCode.LeftArrow, IdPlayer);
//                    timeLastMoveHorizontal = Time.time;
//                }

//                if ((double)Time.time - timeLastMoveHorizontal >= speedFastMoveHorizontal && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
//                {
//                    CmdMove(KeyCode.RightArrow, IdPlayer);
//                    timeLastMoveHorizontal = Time.time;
//                }
//            }

//            if (Input.GetKeyDown(KeyCode.Escape))
//            {
//                GamePouse = !GamePouse;
//            }
//            //CmdMove(Input.Get)
//        }
//    }

//    [Command(channel = 0)]
//    public void CmdDisconect(string IdPlayer)
//    {
//        RpcDisconect(IdPlayer);
//    }

//    [Command(channel = 0)]
//    public void CmdSetId(string IdPlayer, string NickName, bool rewrite)
//    {
//        Debug.Log(IdPlayer + " " + rewrite);
//        if (rewrite == true)
//        {
//            for (int i = 0; i < IdPlayers.Length; i++)
//            {
//                IdPlayers[i] = null;
//                NickNamePlayers[i] = null;
//            }
//            IdPlayers[0] = this.GetIdPlayer();
//            NickNamePlayers[0] = this.GetNickName();
//            //RpcAllDeleteDirectory();
//            SendImage(this.GetIdPlayer(), rewrite);
//        }

//        if (IdPlayer != null && IdPlayer != "")
//        {
//            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": IdPlayer != null ");
//            for (int i = 0; i < IdPlayers.Length; i++)
//            {
//                Debug.Log(i + ". " + IdPlayers[i]);
//                if (IdPlayers[i] == null || IdPlayers[i] == "")
//                {
//                    IdPlayers[i] = IdPlayer;
//                    NickNamePlayers[i] = NickName;
//                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принято " + IdPlayers[i]);
//                    break;
//                }
//            }
//        }

//        if (this.isServer && rewrite == true)
//        {

//            TetrominoPlayerHelper[] TPsH = FindObjectsOfType<TetrominoPlayerHelper>();
//            for (int i = 0; i < TPsH.Length; i++)
//            {
//                if (TPsH[i].isServer)
//                {
//                    TPsH[i].RpcGetId();
//                }
//            }

//            //chatHelper.CmdSetId();
//            //CmdGetId();
//        }

//        RpcSetIdAndNickNamePlayersFromClient(IdPlayers, NickNamePlayers);
//        RpcPlayersPanel(GetIdPlayersArr(), GetNickNamePlayersArr());
//        // RpcPlayersPanel(id, NickName);
//    }

//    [Command]
//    public void CmdGetId()
//    {
//        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": CmdGetId ++");
//        //CmdSetId(null, true);
//        //RpcGetId();
//    }


//    [ClientRpc(channel = 0)]
//    public void RpcStopHost()
//    {
//        //EditorUtility.DisplayDialog("Потеряна связь с сервером", "Хост остановил игру", "Ok");
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcDisconect(string Id)
//    {
//        if (this.GetIdPlayer() == Id)
//        {
//            FindObjectOfType<NetworkManager>().StopClient();
//        }
//    }

//    [Command(channel = 0)]
//    public void CmdMove(KeyCode key, string IdPlayer)
//    {
//        if (this.isServer && FindObjectOfType<GameOnLANV2>() != null)
//        {
//            // Debug.Log(IdPlayer);
//            FindObjectOfType<GameOnLANV2>().CheckPlayerInputKey(key, IdPlayer);
//        }
//    }

//    //[Command(channel = 0)]
//    //public void CmdMoveDown(string IdPalyer)
//    //{
//    //    if (this.isServer)
//    //    {
//    //        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": MoveDown " + IdPalyer + " GetInstanceID " + this.GetInstanceID());
//    //    }
//    //}

//    //[Command(channel = 0)]
//    //public void CmdMoveLeft(string IdPalyer)
//    //{
//    //    if (this.isServer)
//    //    {
//    //        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": MoveLeft " + IdPalyer + " GetInstanceID " + this.GetInstanceID());
//    //    }
//    //}

//    //[Command(channel = 0)]
//    //public void CmdMoveRight(string IdPalyer)
//    //{
//    //    if (this.isServer)
//    //    {           
//    //        FindObjectOfType<TetrominoOnLANV2>().Rotate();
//    //        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": MoveRight " + IdPalyer + " GetInstanceID " + this.GetInstanceID());
//    //    }
//    //}

//    //[Command(channel = 0)]
//    //public void CmdRotate(string IdPalyer)
//    //{
//    //    if (this.isServer)
//    //    {
//    //        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Rotate " + IdPalyer + " GetInstanceID " + this.GetInstanceID());
//    //    }
//    //}

//    [ClientRpc(channel = 0)]
//    public void RpcCreateFolder()
//    {
//        if (!Directory.Exists(@"C:\Users\Big\Desktop\TestImageStream\" + this.GetIdPlayer()))
//        {
//            Directory.CreateDirectory(@"C:\Users\Big\Desktop\TestImageStream\" + this.GetIdPlayer());
//        }
//    }


//    public string PreficsDirectory = @"\Resources\TempImage\";
//    public string PreficsPlayerImageDirectory = @"\Resources\PlayerImages\";



//    public void SendImage(string IdPlayer, bool rewrite) //Тестовое 
//    {
//        PreficsDirectory = @"Resources\TempImage\";
//        PreficsPlayerImageDirectory = @"Resources\PlayerImages\";
//        string path = Application.dataPath + @"\" + PreficsPlayerImageDirectory;
//        string PlayerImage;
//        //string PlayerImage = PlayerPrefs.GetString("", "Client.png");
//        //if (this.isServer)
//        //{
//        //    PlayerImage = PlayerPrefs.GetString("", "Host.png");
//        //}
//        PlayerImage = PlayerPrefs.GetString("ImagePlayer", "");
//        Debug.Log(PlayerImage);
//        Debug.Log(File.Exists(path + PlayerImage));
//        //File.Open(PreficsPlayerImageDirectory + PlayerImage, FileMode.Open);
//        if (File.Exists(path + PlayerImage))
//        {
//            using (FileStream input = new FileStream(Application.dataPath + @"\" + PreficsPlayerImageDirectory + PlayerImage, FileMode.Open, FileAccess.Read))
//            {
//                byte[] buffer = new byte[packageSize];
//                int count = 0;
//                int[] bufferInt = new int[packageSize];
//                int j = 0;
//                //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Количество пакетов " + (input.Length / packageSize));
//                while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
//                {
//                    j++;
//                    for (int i = 0; i < bufferInt.Length; i++)
//                    {
//                        bufferInt[i] = 0;
//                        bufferInt[i] = buffer[i];
//                    }
//                    //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пакет " + j + " отправен");
//                    if (j == 1 && rewrite == true)
//                    {

//                        //Thread thUploatToHost = new Thread(new ParameterizedThreadStart(CmdUploadToHost));
//                        CmdUploadToHost(IdPlayer, bufferInt, count, input.Length, false, true);
//                    }
//                    else
//                    {
//                        CmdUploadToHost(IdPlayer, bufferInt, count, input.Length, false, false);
//                    }

//                }
//                if (count <= 0)
//                {
//                    CmdUploadToHost(IdPlayer, bufferInt, count, input.Length, true, false);
//                }
//            }
//        }
//    }



//    [Command]
//    public void CmdUploadToHost(string IdPlayer, int[] intArr, int count, long length, bool lastPackage, bool rewrite)
//    {
//        if (this.isServer)
//        {
//            if (!Directory.Exists(Application.dataPath + @"\" + PreficsDirectory + "HostDir"))
//            {
//                Directory.CreateDirectory(Application.dataPath + @"\" + PreficsDirectory + "HostDir");
//            }

//            if (rewrite == true)
//            {
//                DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + @"\" + PreficsDirectory + "HostDir");
//                //Directory.CreateDirectory(@"C:\Users\Big\Desktop\TestImageStream\" + this.GetIdPlayer());
//                foreach (FileInfo File in dirInfo.GetFiles())
//                {
//                    File.Delete();

//                    //Debug.Log(Dir.Name);
//                }
//                RpcClearDirectory();
//            }

//            //FileMode.Append
//            if (IdPlayer != null)
//            {
//                using (FileStream output = new FileStream(Application.dataPath + @"\" + PreficsDirectory + @"HostDir\" + IdPlayer + ".png", FileMode.Append, FileAccess.Write))
//                {
//                    //while(count != 0)
//                    //{      
//                    if (lastPackage == false)
//                    {
//                        //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Пакет " + length + " принят" );
//                        output.Write(ConvertIntArrToByteArr(intArr), 0, count);
//                    }
//                    //if (count == 0 && output.Length != length)
//                    //{

//                    //}
//                    //}
//                    else
//                    {
//                        if (output.Length != length)
//                        {
//                            Debug.LogError("Файл " + output.Name + " поврежден или был отправлен повторно! Не достаточно " + (length - output.Length) + " байт");
//                            //output.Close();
//                            //File.Delete(PreficsDirectory + @"HostDir\"  + IdPlayer + ".png");
//                            string Path = output.Name;
//                            output.Close();
//                            File.Delete(Path);

//                        }
//                        else
//                        {
//                            output.Close();
//                            RpcPlayersPanel(GetIdPlayersArr(), GetNickNamePlayersArr());
//                            CmdSendToClient();
//                        }
//                    }

//                }
//            }
//        }
//    }



//    [ClientRpc(channel = 0)]
//    public void RpcClearDirectory()
//    {
//        if (Directory.Exists(Application.dataPath + @"\" + PreficsDirectory + this.GetIdPlayer()))
//        {
//            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + @"\" + PreficsDirectory + this.GetIdPlayer());
//            //Directory.CreateDirectory(@"C:\Users\Big\Desktop\TestImageStream\" + this.GetIdPlayer());
//            foreach (FileInfo File in dirInfo.GetFiles())
//            {
//                File.Delete();

//                //Debug.Log(Dir.Name);
//            }
//        }
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcAllDeleteDirectory() //Тестовое 
//    {
//        if (Directory.Exists(Application.dataPath + @"\" + PreficsDirectory))
//        {
//            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + @"\" + PreficsDirectory);
//            //Directory.CreateDirectory(@"C:\Users\Big\Desktop\TestImageStream\" + this.GetIdPlayer());
//            foreach (DirectoryInfo Dr in dirInfo.GetDirectories())
//            {
//                Dr.Delete(true);

//                //Debug.Log(Dir.Name);
//            }
//        }
//    }

//    [Command]
//    private void CmdSendToClient()
//    {
//        if (this.isServer)
//        {
//            for (int j = 0; j < IdPlayers.Length; j++)
//            {
//                if (File.Exists(Application.dataPath + @"\" + PreficsDirectory + @"HostDir\" + IdPlayers[j] + ".png"))
//                {
//                    using (FileStream input = new FileStream(Application.dataPath + @"\" + PreficsDirectory + @"HostDir\" + IdPlayers[j] + ".png", FileMode.Open, FileAccess.Read))
//                    {
//                        byte[] buffer = new byte[packageSize];
//                        int count = 0;
//                        int[] bufferInt = new int[packageSize];
//                        while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
//                        {
//                            for (int i = 0; i < bufferInt.Length; i++)
//                            {
//                                bufferInt[i] = 0;
//                                bufferInt[i] = buffer[i];
//                            }
//                            //if (count > 0)
//                            //{
//                            RpcSendImage(IdPlayers[j], bufferInt, count, input.Length, false);
//                            //}
//                        }
//                        if (count <= 0)
//                        {
//                            RpcSendImage(IdPlayers[j], bufferInt, count, input.Length, true);
//                        }

//                    }
//                }
//            }
//        }
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcSendImage(string name, int[] intArr, int count, long length, bool lastPackage)
//    {
//        if (!this.isServer)
//        {

//            if (!Directory.Exists(Application.dataPath + @"\" + PreficsDirectory + this.GetIdPlayer()))
//            {
//                Directory.CreateDirectory(Application.dataPath + @"\" + PreficsDirectory + this.GetIdPlayer());
//            }

//            using (FileStream output = new FileStream(Application.dataPath + @"\" + PreficsDirectory + this.GetIdPlayer() + @"\" + name + ".png", FileMode.Append, FileAccess.Write))
//            {
//                //while(count != 0)
//                //{               
//                if (lastPackage == false)
//                {
//                    output.Write(ConvertIntArrToByteArr(intArr), 0, count);
//                }
//                else
//                {
//                    if (output.Length != length)
//                    {
//                        Debug.LogError("Файл " + output.Name + " поврежден или был отправлен повторно! Не достаточно " + (length - output.Length) + " байт");
//                        string Path = output.Name;
//                        output.Close();
//                        File.Delete(Path);
//                        CmdSendToClient();
//                    }
//                    else
//                    {
//                        CmdPlayersPanel(GetIdPlayersArr(), GetNickNamePlayersArr());
//                    }
//                }
//                //if (count == 0 && output.Length != length)
//                //{

//                //}
//                //}
//            }
//        }
//    }

//    int packageSize = 512;

//    private byte[] ConvertIntArrToByteArr(int[] intArr)
//    {
//        byte[] byteArr = new byte[packageSize];
//        for (int i = 0; i < intArr.Length; i++)
//        {
//            byteArr[i] = (byte)intArr[i];
//        }
//        return byteArr;
//    }

//    private int[] ConvertByteArrToIntArr(byte[] byteArr)
//    {
//        int[] intArr = new int[packageSize];
//        for (int i = 0; i < byteArr.Length; i++)
//        {
//            intArr[i] = (int)byteArr[i];
//        }
//        return intArr;
//    }

//    //public void asd()
//    //{
//    //    long lenght = 0;
//    //    //Console.Write((byte)200.0);

//    //    using (FileStream stream = new FileStream("image.png", FileMode.Open, FileAccess.Read))
//    //    {
//    //        byte[] buffer = new byte[512];
//    //        int count = 0;
//    //        lenght = stream.Length;

//    //        using (FileStream output = new FileStream(@"C:\Users\Big\Desktop\TestImageStream\" + this.GetIdPlayer() + ".png", FileMode.Create, FileAccess.Write))
//    //        {
//    //            int i = 0;
//    //            //while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
//    //            //{
//    //            while (true)
//    //            {
//    //                count = stream.Read(buffer, 0, buffer.Length);
//    //                i++;
//    //                if (i != 4 && i != 5)
//    //                {
//    //                    output.Write(buffer, 0, count);
//    //                }

//    //                if (count == 0)
//    //                {
//    //                    output.Close();
//    //                    using (FileStream check = new FileStream(@"C:\Users\student\Desktop\out.png", FileMode.Open, FileAccess.Read))
//    //                    {
//    //                        if (lenght == check.Length)
//    //                        {
//    //                            Console.WriteLine("Файл цел");

//    //                        }
//    //                        if (lenght != check.Length)
//    //                        {
//    //                            Console.WriteLine("Файл поврежден. Не хватает: " + (lenght - check.Length) + " байт");
//    //                            check.Close();
//    //                            File.Delete(@"C:\Users\student\Desktop\out.png");

//    //                            Console.WriteLine("Файл был удален");
//    //                            //check.
//    //                        }
//    //                    }
//    //                    break;
//    //                }
//    //            }
//    //        }
//    //    }
//    //}


//    //public int GetCountPlayer()
//    //{
//    //    for(int i = 0; i < IdPlayers.Length; i++)
//    //    {
//    //        if (IdPlayers[i] == null || IdPlayers[i] == "")
//    //        {
//    //            return i;
//    //        }
//    //    }
//    //    return 1;
//    //}

//    GameObject[] FrameArr = new GameObject[4];
//    int connected = 0;

//    [ClientRpc(channel = 0)]
//    public void RpcSpawnFramePlayer(int Connected)
//    {
//        connected = Connected;
//        for (int i = 0; i < Connected; i++)
//        {
//            playerPositionArr[i] = new Vector3(i * ShiftFactorFrame - 1, -1, 100);
//            // playersPositionsXarr[]
//        }
//        for (int i = 0; i < Connected; i++)
//        {
//            //if(!this.isServer)
//            FrameArr[i] = (GameObject)Instantiate(Resources.Load("Prefabs/Frame"), playerPositionArr[i], Quaternion.identity);

//            //FrameArr[i].GetComponent<FrameScript>().SetNickName("yfuihyfi");
//            //FindObjectOfType<FrameScript>().SetNickName("yfuihyfi");
//            FindObjectOfType<FrameScript>().SetNickName(NickNamePlayers[i]);
//            FindObjectOfType<FrameScript>().SetImage(IdPlayers[i]);
//            //FindObjectOfType<Camera>().orthographicSize = 100;     
//        }
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcRepostInfo(bool[][,] grid)
//    {
//        // Debug.Log();
//        //if (!this.isLocalPlayer && !this.isServer)
//        //{
//        //    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Na-na-na");
//        //}
//        //if (!this.isLocalPlayer && !this.isServer)
//        //{
//        //    for (int k = 0; k < grid.Length; k++)
//        //    {
//        //        for (int i = 0; i < grid[k].GetLength(0); i++)
//        //        {
//        //            for (int j = 0; j < grid[k].GetLength(1); j++)
//        //            {
//        //                if (grid[k][i, j] != false)
//        //                {
//        //                    PlayerGrid[k][i, j] = (GameObject)Instantiate(Resources.Load("Graphics/Cell_Shape/SuperCell"), new Vector2(k + i + 15, j), Quaternion.identity);
//        //                }
//        //            }
//        //        }
//        //    }
//        //}
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcSpawnNextTetromino(string IdPlayer, int num, string NowTetra, string NextTetra)    //Установки следующей фигуры из препромотра на поле, и установка новой фигуры для препросмотра
//    {
//        //int num = GetNumPlayer(IdPlayer);
//        if (!this.isLocalPlayer && !this.isServer)
//        {
//            //Debug.Log(NowTetra);
//            //EnableTetra = true;
//            if (!GameStartArr[num])
//            {
//                GameStart = true;
//                GameStartArr[num] = true;
//                NowTetrominoArr[num] = (GameObject)NetworkManager.Instantiate(Resources.Load(PrefixesTetrominoName + NowTetra, typeof(GameObject)), new Vector2(num * ShiftFactorFrame + 5, 22), Quaternion.identity);
//                NowTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
//                NowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = true;
//                PreviewTetrominoArr[num] = (GameObject)NetworkManager.Instantiate(Resources.Load(PrefixesTetrominoName + NextTetra, typeof(GameObject)), new Vector2(num * ShiftFactorFrame + 14, 18), Quaternion.identity);
//                PreviewTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = false;
//                PreviewTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
//                // NowTetromino.transform.parent = GameObject.Find("PlayerTetrisPrefab(Clone)").GetComponent<Transform>();
//            }
//            else
//            {
//                PreviewTetrominoArr[num].transform.position = new Vector2(num * ShiftFactorFrame + 5, 22);
//                NowTetrominoArr[num] = PreviewTetrominoArr[num];
//                //NowTetrominoArr[num] = (GameObject)NetworkManager.Instantiate(PreviewTetrominoArr[num], new Vector2(num * ShiftFactorFrame + 5, 22), Quaternion.identity);
//                NowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = true;
//                NowTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
//                //NowTetromino.transform.parent = GameObject.Find("PlayerTetrisPrefab(Clone)").GetComponent<Transform>();

//                PreviewTetrominoArr[num] = (GameObject)Instantiate(Resources.Load(PrefixesTetrominoName + NextTetra, typeof(GameObject)), new Vector2(num * ShiftFactorFrame + 14, 18), Quaternion.identity);
//                PreviewTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = false;
//            }
//            //EnableTetra = false;
//        }
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcSynchronizedTetromino(string IdPlayer, Vector3 pos, int num, Quaternion qu, bool Enable)
//    {
//        if (!this.isLocalPlayer && !this.isServer)
//        {
//            //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": FindObjectOfType<TetrominoPlayer>().enabled " + FindObjectOfType<TetrominoPlayer>().enabled);
//            //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Lenght Arr: " + PosEnableTetra.GetLength(0) + " num:" + num);
//            if (NowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled == true && DeleteTetra[num] == false/* && EnableTetra == false /*&& PosEnableTetra[num].y != pos.y && Seted[num] == true*/)
//            {

//                //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": FindObjectOfType<TetrominoPlayer>().IdPlayer == IdPlayer" + (FindObjectOfType<TetrominoPlayer>().IdPlayer == IdPlayer));
//                if (NowTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer == IdPlayer)
//                {
//                    NowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.position = pos;
//                    NowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.rotation = qu;
//                    RpcUpdateGridPlayer(NowTetrominoArr[num].GetComponent<TetrominoPlayer>(), num, IdPlayer);
//                }
//            }
//            //if (PosEnableTetra[num].y != pos.y && Seted[num] == false)
//            //{
//            //    Seted[num] = false;
//            //}
//            //if(pos.y != PosEnableTetra[num].y)
//            //{
//            //    EnableTetra = false;
//            //}
//            //EnableTetra = false;
//        }
//    }

//    //[ClientRpc(channel = 0)]
//    public void RpcUpdateGridPlayer(TetrominoPlayer tetromino, int num, string IdPlayer)
//    {
//        // int num = GetNumPlayer(IdPlayer);
//        //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Работает снаружи " + num);
//        if (!this.isLocalPlayer && !this.isServer)
//        {
//            //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Работает внутри " + num);
//            if (tetromino.IdPlayer == IdPlayer)
//            {
//                for (int x = 0; x < gridArr[num].GetLength(0); x++)
//                {
//                    for (int y = 0; y < gridArr[num].GetLength(1); y++)
//                    {
//                        if (gridArr[num][x, y] != null)
//                        {
//                            if (gridArr[num][x, y].parent == tetromino.transform)
//                            {
//                                //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Очистка" + num);
//                                //Debug.Log(tetromino.IdPlayer);
//                                gridArr[num][x, y] = null;
//                            }
//                        }
//                    }
//                }
//            }


//            foreach (Transform mino in tetromino.transform)
//            {
//                Vector2 vec = RoundVec2(mino.position);

//                if (vec.y < gridMaxHeight && (vec.x - (num * ShiftFactorFrame)) < gridMaxWidth)
//                {
//                    // Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Vec.x" + vec.x)
//                    //Debug.Log(vec.x - (num * ShiftFactorFrame));
//                    if (vec.y == 0)
//                    {
//                        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Добавление" + num + "    x: " + (vec.x - (num * ShiftFactorFrame)));
//                    }
//                    try
//                    {
//                        gridArr[num][(int)(vec.x - (num * ShiftFactorFrame)), (int)(vec.y)] = mino;
//                    }
//                    catch (IndexOutOfRangeException e)
//                    {
//                        Debug.LogError(e + "    x: " + vec.x + "     y: " + vec.y + "    num: " + num);
//                    }

//                }
//            }
//        }
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcDeleteRow(string IdPlayer, int num, int Row)     //Вызывается при удалении строки
//    {
//        //int num = GetNumPlayer(IdPlayer);
//        //for (int y = 0; y < gridArr[num].GetLength(1); y++)
//        //{
//        //if (itRowFull(y, IdPlayer))
//        //{
//        //itRowDelete = true;
//        //UpdateScore();
//        //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": RpcDeleteRow");
//        //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": isLocalPlayer" + isLocalPlayer);
//        //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": isServer" + isServer);
//        //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": isClient" + isClient);
//        if (!this.isServer && !this.isLocalPlayer)
//        {
//            RpcUpdateGridPlayer(NowTetrominoArr[num].GetComponent<TetrominoPlayer>(), num, "0");
//            //RpcEnableTetra(num);
//            DeleteTetra[num] = true;
//            DeletePoint(Row, IdPlayer, num);
//            AllRowsDown(Row + 1, IdPlayer, num);
//            DeleteTetra[num] = false;
//            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": RpcDeleteRow IsWork");
//        }
//        //if (Game.GameMode == 1)     //Удаление первой каменной линии (HardCore Mode)
//        //{
//        //    DeletePoint(0, IdPlayer);
//        //    AllRowsDown(0 + 1, IdPlayer);
//        //}

//        //y--;

//        // limitStoneHeight--;
//        //}
//    }

//    private void AllRowsDown(int y, string IdPlayer, int num)     //Опускает сторки вниз начиная начиная с строки "y" 
//    {
//        //if (this.isLocalPlayer && !this.isServer && !this.isClient)
//        //{
//        //int num = GetNumPlayer(IdPlayer);
//        for (int i = y; i < gridArr[num].GetLength(1); i++)
//        {
//            RowsDown(i, IdPlayer, num);
//        }
//        //}
//    }

//    private void RowsDown(int y, string IdPlayer, int num)    //Опускает одну строку "y"
//    {
//        //if (this.isLocalPlayer && !this.isServer && !this.isClient)
//        //{
//        //int num = GetNumPlayer(IdPlayer);
//        for (int x = 0; x < gridArr[num].GetLength(0); x++)
//        {
//            if (gridArr[num][x, y] != null)
//            {
//                gridArr[num][x, y - 1] = gridArr[num][x, y];

//                gridArr[num][x, y] = null;

//                gridArr[num][x, y - 1].position += new Vector3(0, -1, 0);
//            }
//        }
//        //}
//    }

//    //[ClientRpc(channel = 0)]
//    public void DeletePoint(int y, string IdPlayer, int num)     //Удалени ряда, по 1 точке за каждую итерацию цикла
//    {
//        //  int num = GetNumPlayer(IdPlayer);
//        //if (this.isLocalPlayer && !this.isServer)
//        //{
//        Debug.Log(y);
//        for (int x = 0; x < gridArr[num].GetLength(0); x++)
//        {
//            try
//            {
//                //if (gridArr[num][x, y].gameObject != null)
//                //{
//                NetworkManager.Destroy(gridArr[num][x, y].gameObject);
//                gridArr[num][x, y] = null;
//                //}

//            }
//            catch (NullReferenceException e)
//            {
//                Debug.LogError(e + "    x: " + x + "     y: " + y + "    num: " + num);
//            }
//        }
//        // }
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcEnableTetra(/*int idPlayer, */int num)
//    {
//        if (!this.isLocalPlayer && !this.isServer)
//        {
//            //EnableTetra = true;
//            RpcUpdateGridPlayer(NowTetrominoArr[num].GetComponent<TetrominoPlayer>(), num, "0");
//            NowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = false;

//            PosEnableTetra[num] = NowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.position;

//            //RpcDeleteRow(idPlayer)
//            //NowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.position = new Vector2(num * ShiftFactorFrame + 5, 22);
//        }
//    }

//    private Vector2 RoundVec2(Vector2 pos)   //Округление вектора 
//    {
//        Vector2 vec = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
//        return vec;
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcGameOver(int num)
//    {
//        if (num < GameOverArr.Length)
//        {
//            GameOverArr[num] = true;
//            if (IdPlayers[num] == IdPlayer)
//            {
//                GameOver = true;
//            }
//        }
//    }

//    [Command(channel = 0)]
//    public void Cmdsurrender(string ID)
//    {
//        //for(int i = 0; i < IdPlayers.Length; i++)
//        //{
//        //    if(IdPlayers[i] == ID)
//        //    {
//        //        RpcGameOver(i);
//        //    }
//        //}
//        FindObjectOfType<GameOnLANV2>().GameOver(ID);
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcGameWin(string Winner)
//    {

//        TetrominoPlayerHelper[] TPsH = FindObjectsOfType<TetrominoPlayerHelper>();
//        for (int i = 0; i < TPsH.Length; i++)
//        {
//            TPsH[i].SetGameState(0);
//            TPsH[i].GameOverEnable();
//            TPsH[i].ClearAllVariable();
//        }

//        SceneManager.LoadScene("ChatTest");
//        GameStart = false;
//        string NameWinner = Winner;
//        for (int i = 0; i < IdPlayers.Length; i++)
//        {
//            if (IdPlayers[i] == Winner)
//            {
//                NameWinner = NickNamePlayers[i];
//                break;
//            }
//        }
//        if (this.isServer && this.isLocalPlayer)
//        {
//            CmdSendMassage("Игрок " + NameWinner + " выиграл");
//        }
//    }

//    public void GameOverEnable()
//    {
//        GameState = 0;
//        GameOver = false;
//        for (int i = 0; i < GameOverArr.Length; i++)
//        {
//            GameOverArr[i] = false;
//            GameStartArr[i] = false;
//        }
//    }

//    [ClientRpc(channel = 0)]
//    public void RpcSpawnStoneLine(string Id, int num)
//    {
//        if (!this.isLocalPlayer && !this.isServer)
//        {
//            for (int i = 0; i < connected; i++)
//            {
//                if (IdPlayers[i] != Id && i != num)
//                {
//                    UpAllRows(Id, i);
//                    StoneLineArr[i] = (GameObject)Instantiate(Resources.Load("Prefabs/TetrominoOnLANPlayer/StoneLine", typeof(GameObject)), new Vector3(i * ShiftFactorFrame, 0, 0), Quaternion.identity);
//                    StoneLineArr[i].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
//                    RpcUpdateGridPlayer(StoneLineArr[i].GetComponent<TetrominoPlayer>(), i, StoneLineArr[i].GetComponent<TetrominoPlayer>().IdPlayer);
//                    StoneLineArr[i].GetComponent<TetrominoPlayer>().enabled = false;
//                }
//            }
//        }
//    }

//    private void UpAllRows(string Id, int num)    //Поднятие всех объектов для освобождения места каменной линии (HardCore Mode)
//    {
//        //int num = GetNumPlayer(Id);
//        for (int y = gridArr[num].GetLength(1) - 2; y > -1; y--)
//        {
//            for (int x = 0; x < gridArr[num].GetLength(0); x++)
//            {
//                if (gridArr[num][x, y] != null)
//                {
//                    gridArr[num][x, y + 1] = gridArr[num][x, y];

//                    gridArr[num][x, y] = null;

//                    gridArr[num][x, y + 1].position += new Vector3(0, +1, 0);
//                }
//            }
//        }
//    }
//    ///////////////////////////////////////////////////////////////////////////////////////////////


//    // GameHelper gameHelper;
//    //  private static string IdPlayer;
//    //  public string GetIdPlayer()
//    //  {
//    //      return IdPlayer;
//    //  }
//    //  GameObject[][,] PlayerGrid = new GameObject[4][,];
//    //  GameObject GameScript;
//    // // GameObject ChatHelper;
//    //  float periodSvrRpc = 0.2f; //как часто сервер шлёт обновление картинки клиентам, с.
//    //  float timeSvrRpcLast = 0;
//    //  private double gameSpeed = 5;   //скорость падения

//    //  private int fastDropCoefficient = 4;    //Коефициент ускорения падения при зажатой клавише
//    //  private double speedFastMoveHorizontal = 0.25;  //Коефициент ускорения горизонтального движения при зажатой клавише

//    //  float timeLastFall = 0;
//    //  float timeLastMoveHorizontal = 0;

//    //  float timeLast;
//    //  float time = 6f;

//    //  private GameObject[] NowTetrominoArr = new GameObject[4];
//    //  private GameObject[] PreviewTetrominoArr = new GameObject[4];

//    //  private static int gridMaxHeight = 20;  //Высота игрового поля  
//    //  private static int gridMaxWidth = 10;   //Ширина игрового поля

//    //  public static Transform[][,] gridArr =
//    //{
//    //      new Transform[gridMaxWidth, gridMaxHeight],
//    //      new Transform[gridMaxWidth, gridMaxHeight],
//    //      new Transform[gridMaxWidth, gridMaxHeight],
//    //      new Transform[gridMaxWidth, gridMaxHeight]
//    //  };

//    //  private bool[] GameStartArr = new bool[4];

//    //  private int ShiftFactorFrame = 20;

//    //  public static Vector3[] PosEnableTetra = new Vector3[4]{
//    //      new Vector3(-1,-1,-1),
//    //      new Vector3(-1,-1,-1),
//    //      new Vector3(-1,-1,-1),
//    //      new Vector3(-1,-1,-1)
//    //  };

//    //  public static string[] IdPlayers = new string[4];

//    //  private static bool[] DeleteTetra = new bool[4];

//    //  private static bool[] AccessDeleteRow = new bool[4];

//    //  string PrefixesTetrominoName = "Prefabs/TetrominoOnLANPlayer/";

//    //  private Vector2[] playerPositionArr = new Vector2[4];

//    //  private static bool[] GameOverArr = new bool[4];

//    //  private static bool GameOver = false;

//    //  private static int GameState = 0;

//    //  // Use this for initialization
//    //  void Awake()
//    //  {
//    //      //gameHelper = GameObject.FindObjectOfType<GameHelper>();
//    //      //if (isLocalPlayer)
//    //      //{
//    //      //    gameHelper.CurrentPlayer = this;
//    //      //}
//    //  }

//    //  GameHelper gameHelper;

//    //  void Start() {
//    //      if (this.isLocalPlayer)
//    //      {
//    //          //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": netId " + this.netId);
//    //          //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": playerControllerId " + this.playerControllerId);
//    //          gameSpeed = (11 - gameSpeed) / 10;
//    //          Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": GetInstanceID " + this.GetInstanceID());
//    //          IdPlayer = this.GetInstanceID().ToString();
//    //          CmdSetId(IdPlayer);

//    //          //Debug,Log(Network.ip)
//    //          //playerId = "" + this.netId;
//    //          // gameHelper.PlayerConnect(playerId);
//    //          timeLast = Time.time;
//    //          // game = (GameObject)NetworkManager.Instantiate(Resources.Load("Prefabs/TetrominoOnLAN/GameScript"));
//    //          GameState++;
//    //      }

//    //      if ((this.isLocalPlayer) && GameState == 0)
//    //      {
//    //          chatHelper = GameObject.FindObjectOfType<ChatHelper>();
//    //          chatHelper.CurrentPlayer = this;
//    //          CmdSendMassage(IdPlayer + " поключился к лобби");
//    //          //chatHelper.SendMessage
//    //          //SpawnChatHelper();
//    //      }

//    //      if((this.isLocalPlayer) && GameState == 1)
//    //      {
//    //          gameHelper = GameObject.FindObjectOfType<GameHelper>();
//    //          //gameHelper.CurrentPlayer = this;
//    //          //if(this.isLocalPlayer)
//    //          //{
//    //              gameHelper.SetIdPlayers(IdPlayers);
//    //          //}
//    //          gameHelper.AddPlayer(this);
//    //      }
//    //  }

//    //  bool GameStart = false;
//    //  private bool EnableTetra;

//    //  // Update is called once per frame
//    //  void Update () {
//    //      //CheckInputKey();

//    //      //if (GameState == 1)
//    //      //{
//    //      //    //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Ку");
//    //      //    if ((isServer && isLocalPlayer) && timeLast + time < Time.time && GameStart == false /*&& AllPalyersConnected()*/)
//    //      //    {
//    //      //        SpawnGame();
//    //      //        FindObjectOfType<GameOnLANV2>().SetHost(this);

//    //      //        FindObjectOfType<GameOnLANV2>().SetIdPlayer(IdPlayers);
//    //      //        //Debug.Log(Network.connections.Length);

//    //      //        FindObjectOfType<GameOnLANV2>().StartGame(NetworkManager.singleton.numPlayers);
//    //      //        GameStart = true;

//    //      //        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": connections " + NetworkManager.singleton.numPlayers);
//    //      //    }
//    //      //}
//    //      //if (this.isServer && GameStart == true)
//    //      //{
//    //      //    //if (timeSvrRpcLast + periodSvrRpc < Time.time)
//    //      //    ////Если пора, то выслать координаты всем моим аватарам
//    //      //    //{
//    //      //    //    //Debug.Log(FindObjectOfType<GameOnLANV2>().Repost());
//    //      //    //    bool[][,] grid = new bool[3][,];
//    //      //    //   // grid = FindObjectOfType<GameOnLANV2>().Repost();
//    //      //    //   // Debug.Log(grid[0][5,15]);
//    //      //    //    //RpcRepostInfo(grid);
//    //      //    //    //SendMessage("RpcRepostInfo", FindObjectOfType<GameOnLANV2>().Repost());

//    //      //    //    timeSvrRpcLast = Time.time;
//    //      //    //}
//    //      //}
//    //      //if(!this.isServer && !this.isLocalPlayer)
//    //      //{

//    //      //}
//    //  }

//    //  private void SpawnChatHelper()
//    //  {
//    //      if (this.isServer)
//    //      {
//    //          //ChatHelper = (GameObject)NetworkManager.Instantiate(Resources.Load("Prefabs/TetrominoOnLAN/ChatHelper"));
//    //          //ChatHelper = FindObjectOfType<ChatHelper>().GetComponent<GameObject>();
//    //          //ChatHelper.GetComponent<ChatHelper>
//    //          //chatHelper = FindObjectOfType<ChatHelper>().gameObject;
//    //          //ChatHelper.GetComponent<ChatHelper>().SendMessage

//    //      }
//    //  }

//    //  //[ClientRpc(channel = 0)]
//    //  //public void RpcSendMassage()
//    //  //{

//    //  //}
//    //  ChatHelper chatHelper;

//    //  [Command]
//    //  public void CmdSendMassage(string message)
//    //  {
//    //      RpcSendMassage(message);
//    //  }

//    //  [ClientRpc(channel = 0)]
//    //  public void RpcSendMassage(string message)
//    //  {
//    //      //SceneManager.LoadScene("MainMenu");

//    //      ChatHelper.TextBlock.text += "\n" + message;
//    //  }

//    //  private bool AllPalyersConnected()
//    //  {
//    //      return false;
//    //  }

//    //  [Command]
//    //  public void CmdLoadGame()
//    //  {        
//    //      RpcLoadGame();
//    //  }

//    //  [ClientRpc(channel = 0)]
//    //  private void RpcLoadGame()
//    //  {
//    //      GameState = 1;
//    //      //foreach(Object obj in FindObjectsOfType<TetrominoPlayerHelper>())
//    //      //{

//    //      //    DontDestroyOnLoad(FindObjectOfType<TetrominoPlayerHelper>());
//    //      //}
//    //      for(int i = 0; i < FindObjectsOfType<TetrominoPlayerHelper>().Length; i++)
//    //      {
//    //          DontDestroyOnLoad(FindObjectsOfType<TetrominoPlayerHelper>()[i]);
//    //      }

//    //      SceneManager.LoadScene("Test Scene V2");
//    //  }

//    //  public void SpawnGame()
//    //  {
//    //      if (this.isServer)
//    //      {
//    //          GameScript = (GameObject)NetworkManager.Instantiate(Resources.Load("Prefabs/TetrominoOnLAN/GameScript"));
//    //      }
//    //  }

//    //  [ClientRpc(channel = 0)]
//    //  public void RpcStartGame()
//    //  {
//    //      if (isLocalPlayer)
//    //      {
//    //          GameStart = true;
//    //      }
//    //  }

//    //  //void CheckInputKey()
//    //  //{
//    //  //    if (this.isLocalPlayer /*&& GameStart*/ && GameOver == false)
//    //  //    {
//    //  //        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
//    //  //        {
//    //  //            CmdMove(KeyCode.RightArrow, IdPlayer);
//    //  //            timeLastMoveHorizontal = Time.time;
//    //  //        }

//    //  //        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
//    //  //        {
//    //  //            CmdMove(KeyCode.LeftArrow, IdPlayer);
//    //  //            timeLastMoveHorizontal = Time.time;
//    //  //        }

//    //  //        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
//    //  //        {
//    //  //            CmdMove(KeyCode.UpArrow, IdPlayer);
//    //  //        }

//    //  //        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
//    //  //        {
//    //  //            CmdMove(KeyCode.DownArrow, IdPlayer);
//    //  //            timeLastFall = Time.time;
//    //  //        }

//    //  //        if ((double)Time.time - timeLastFall >= gameSpeed / fastDropCoefficient && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
//    //  //        {
//    //  //            CmdMove(KeyCode.DownArrow, IdPlayer);
//    //  //            timeLastFall = Time.time;
//    //  //        }

//    //  //        if ((double)Time.time - timeLastMoveHorizontal >= speedFastMoveHorizontal && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
//    //  //        {
//    //  //            CmdMove(KeyCode.LeftArrow, IdPlayer);
//    //  //            timeLastMoveHorizontal = Time.time;
//    //  //        }

//    //  //        if ((double)Time.time - timeLastMoveHorizontal >= speedFastMoveHorizontal && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
//    //  //        {
//    //  //            CmdMove(KeyCode.RightArrow, IdPlayer);
//    //  //            timeLastMoveHorizontal = Time.time;
//    //  //        }

//    //  //        //CmdMove(Input.Get)
//    //  //    }
//    //  //}

//    //  [Command(channel = 0)]
//    //  public void CmdSetId(string IdPlayer)
//    //  {
//    //      for(int i = 0; i < IdPlayers.Length; i++)
//    //      {
//    //          if(IdPlayers[i] == null)
//    //          {
//    //              IdPlayers[i] = IdPlayer;
//    //              Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принято");
//    //              break;
//    //          }
//    //      }     
//    //  }

//    //  [Command(channel = 0)]
//    //  public void CmdMove(KeyCode key, string IdPlayer)
//    //  {
//    //      if (this.isServer)
//    //      {
//    //         // Debug.Log(IdPlayer);
//    //          FindObjectOfType<GameOnLANV2>().CheckPlayerInputKey(key, IdPlayer);
//    //      }
//    //  }

//    //  //[Command(channel = 0)]
//    //  //public void CmdMoveDown(string IdPalyer)
//    //  //{
//    //  //    if (this.isServer)
//    //  //    {
//    //  //        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": MoveDown " + IdPalyer + " GetInstanceID " + this.GetInstanceID());
//    //  //    }
//    //  //}

//    //  //[Command(channel = 0)]
//    //  //public void CmdMoveLeft(string IdPalyer)
//    //  //{
//    //  //    if (this.isServer)
//    //  //    {
//    //  //        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": MoveLeft " + IdPalyer + " GetInstanceID " + this.GetInstanceID());
//    //  //    }
//    //  //}

//    //  //[Command(channel = 0)]
//    //  //public void CmdMoveRight(string IdPalyer)
//    //  //{
//    //  //    if (this.isServer)
//    //  //    {           
//    //  //        FindObjectOfType<TetrominoOnLANV2>().Rotate();
//    //  //        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": MoveRight " + IdPalyer + " GetInstanceID " + this.GetInstanceID());
//    //  //    }
//    //  //}

//    //  //[Command(channel = 0)]
//    //  //public void CmdRotate(string IdPalyer)
//    //  //{
//    //  //    if (this.isServer)
//    //  //    {
//    //  //        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Rotate " + IdPalyer + " GetInstanceID " + this.GetInstanceID());
//    //  //    }
//    //  //}

//    //  [ClientRpc(channel = 0)]
//    //  public void RpcSpawnFramePlayer(int Connected)
//    //  {
//    //      //for (int i = 0; i < Connected; i++)
//    //      //{
//    //      //    playerPositionArr[i] = new Vector2(i * ShiftFactorFrame - 1, -1);
//    //      //    // playersPositionsXarr[]
//    //      //}
//    //      //for (int i = 0; i < Connected; i++)
//    //      //{
//    //      //    GameObject.Instantiate(Resources.Load("Prefabs/Frame"), playerPositionArr[i], Quaternion.identity);
//    //      //}
//    //      if (this.isLocalPlayer)
//    //      {
//    //          gameHelper.SpawnFramePlayer(Connected);
//    //      }
//    //  }

//    //  [ClientRpc(channel = 0)]
//    //  public void RpcRepostInfo(bool[][,] grid)
//    //  {
//    //     // Debug.Log();
//    //      //if (!this.isLocalPlayer && !this.isServer)
//    //      //{
//    //      //    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Na-na-na");
//    //      //}
//    //      //if (!this.isLocalPlayer && !this.isServer)
//    //      //{
//    //      //    for (int k = 0; k < grid.Length; k++)
//    //      //    {
//    //      //        for (int i = 0; i < grid[k].GetLength(0); i++)
//    //      //        {
//    //      //            for (int j = 0; j < grid[k].GetLength(1); j++)
//    //      //            {
//    //      //                if (grid[k][i, j] != false)
//    //      //                {
//    //      //                    PlayerGrid[k][i, j] = (GameObject)Instantiate(Resources.Load("Graphics/Cell_Shape/SuperCell"), new Vector2(k + i + 15, j), Quaternion.identity);
//    //      //                }
//    //      //            }
//    //      //        }
//    //      //    }
//    //      //}
//    //  }

//    //  [ClientRpc(channel = 0)]
//    //  public void RpcSpawnNextTetromino(string IdPlayer, int num, string NowTetra, string NextTetra)    //Установки следующей фигуры из препромотра на поле, и установка новой фигуры для препросмотра
//    //  {
//    //      //int num = GetNumPlayer(IdPlayer);
//    //      if (!this.isLocalPlayer && !this.isServer)
//    //      {
//    //          ////Debug.Log(NowTetra);
//    //          ////EnableTetra = true;
//    //          //if (!GameStartArr[num])
//    //          //{
//    //          //    GameStart = true;
//    //          //    GameStartArr[num] = true;
//    //          //    NowTetrominoArr[num] = (GameObject)NetworkManager.Instantiate(Resources.Load(PrefixesTetrominoName + NowTetra, typeof(GameObject)), new Vector2(num * ShiftFactorFrame + 5, 22), Quaternion.identity);
//    //          //    NowTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
//    //          //    NowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = true;
//    //          //    PreviewTetrominoArr[num] = (GameObject)NetworkManager.Instantiate(Resources.Load(PrefixesTetrominoName + NextTetra, typeof(GameObject)), new Vector2(num * ShiftFactorFrame + 14, 18), Quaternion.identity);
//    //          //    PreviewTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = false;
//    //          //    PreviewTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
//    //          //    // NowTetromino.transform.parent = GameObject.Find("PlayerTetrisPrefab(Clone)").GetComponent<Transform>();
//    //          //}
//    //          //else
//    //          //{
//    //          //    PreviewTetrominoArr[num].transform.position = new Vector2(num * ShiftFactorFrame + 5, 22);
//    //          //    NowTetrominoArr[num] = PreviewTetrominoArr[num];
//    //          //    //NowTetrominoArr[num] = (GameObject)NetworkManager.Instantiate(PreviewTetrominoArr[num], new Vector2(num * ShiftFactorFrame + 5, 22), Quaternion.identity);
//    //          //    NowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = true;
//    //          //    NowTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
//    //          //    //NowTetromino.transform.parent = GameObject.Find("PlayerTetrisPrefab(Clone)").GetComponent<Transform>();

//    //          //    PreviewTetrominoArr[num] = (GameObject)Instantiate(Resources.Load(PrefixesTetrominoName + NextTetra, typeof(GameObject)), new Vector2(num * ShiftFactorFrame + 14, 18), Quaternion.identity);
//    //          //    PreviewTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = false;
//    //          //}
//    //          //EnableTetra = false;
//    //          gameHelper.SpawnNextTetromino(IdPlayer, num, NowTetra, NextTetra);
//    //      }
//    //  }

//    //  [ClientRpc(channel = 0)]
//    //  public void RpcSynchronizedTetromino(string IdPlayer, Vector3 pos , int num, Quaternion qu, bool Enable)
//    //  {
//    //      if (!this.isLocalPlayer && !this.isServer)
//    //      {
//    //          //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": FindObjectOfType<TetrominoPlayer>().enabled " + FindObjectOfType<TetrominoPlayer>().enabled);
//    //          //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Lenght Arr: " + PosEnableTetra.GetLength(0) + " num:" + num);
//    //          //if (NowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled == true && DeleteTetra[num] == false/* && EnableTetra == false*/ /*&& PosEnableTetra[num].y != pos.y && Seted[num] == true*/)
//    //          //{

//    //          //    //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": FindObjectOfType<TetrominoPlayer>().IdPlayer == IdPlayer" + (FindObjectOfType<TetrominoPlayer>().IdPlayer == IdPlayer));
//    //          //    if (NowTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer == IdPlayer)
//    //          //    {
//    //          //        NowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.position = pos;
//    //          //        NowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.rotation = qu;
//    //          //        RpcUpdateGridPlayer(NowTetrominoArr[num].GetComponent<TetrominoPlayer>(), num, IdPlayer);
//    //          //    }
//    //          //}
//    //          //if (PosEnableTetra[num].y != pos.y && Seted[num] == false)
//    //          //{
//    //          //    Seted[num] = false;
//    //          //}
//    //          //if(pos.y != PosEnableTetra[num].y)
//    //          //{
//    //          //    EnableTetra = false;
//    //          //}
//    //          //EnableTetra = false;
//    //          gameHelper.SynchronizedTetromino(IdPlayer, pos, num, qu, Enable);
//    //      }
//    //  }

//    //  //[ClientRpc(channel = 0)]
//    //  public void RpcUpdateGridPlayer(TetrominoPlayer tetromino, int num, string IdPlayer)
//    //  {
//    //      // int num = GetNumPlayer(IdPlayer);
//    //      //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Работает снаружи " + num);
//    //      if (!this.isLocalPlayer && !this.isServer)
//    //      {
//    //          gameHelper.UpdateGridPlayer(tetromino, num, IdPlayer);
//    //          ////Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Работает внутри " + num);
//    //          //if (tetromino.IdPlayer == IdPlayer)
//    //          //{
//    //          //    for (int x = 0; x < gridArr[num].GetLength(0); x++)
//    //          //    {
//    //          //        for (int y = 0; y < gridArr[num].GetLength(1); y++)
//    //          //        {
//    //          //            if (gridArr[num][x, y] != null)
//    //          //            {
//    //          //                if (gridArr[num][x, y].parent == tetromino.transform)
//    //          //                {
//    //          //                    //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Очистка" + num);
//    //          //                    //Debug.Log(tetromino.IdPlayer);
//    //          //                    gridArr[num][x, y] = null;
//    //          //                }
//    //          //            }
//    //          //        }
//    //          //    }
//    //          //}


//    //          //foreach (Transform mino in tetromino.transform)
//    //          //{
//    //          //    Vector2 vec = RoundVec2(mino.position);

//    //          //    if (vec.y < gridMaxHeight && (vec.x - (num * ShiftFactorFrame)) < gridMaxWidth)
//    //          //    {
//    //          //        // Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Vec.x" + vec.x)
//    //          //        //Debug.Log(vec.x - (num * ShiftFactorFrame));
//    //          //        if (vec.y == 0)
//    //          //        {
//    //          //            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Добавление" + num + "    x: " + (vec.x - (num * ShiftFactorFrame)));
//    //          //        }
//    //          //        try
//    //          //        {
//    //          //            gridArr[num][(int)(vec.x - (num * ShiftFactorFrame)), (int)(vec.y)] = mino;
//    //          //        }
//    //          //        catch (IndexOutOfRangeException e)
//    //          //        {
//    //          //            Debug.LogError(e + "    x: " + vec.x + "     y: " + vec.y + "    num: " + num);
//    //          //        }

//    //          //    }
//    //          //}
//    //      }
//    //  }

//    //  [ClientRpc(channel = 0)]
//    //  public void RpcDeleteRow(string IdPlayer, int num, int Row)     //Вызывается при удалении строки
//    //  {
//    //      //int num = GetNumPlayer(IdPlayer);
//    //      //for (int y = 0; y < gridArr[num].GetLength(1); y++)
//    //      //{
//    //      //if (itRowFull(y, IdPlayer))
//    //      //{
//    //      //itRowDelete = true;
//    //      //UpdateScore();
//    //      //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": RpcDeleteRow");
//    //      //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": isLocalPlayer" + isLocalPlayer);
//    //      //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": isServer" + isServer);
//    //      //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": isClient" + isClient);
//    //      if (!this.isServer && !this.isLocalPlayer)
//    //      {
//    //          //RpcUpdateGridPlayer(NowTetrominoArr[num].GetComponent<TetrominoPlayer>(), num, "0");
//    //          ////RpcEnableTetra(num);
//    //          //DeleteTetra[num] = true;
//    //          //DeletePoint(Row, IdPlayer, num);
//    //          //AllRowsDown(Row + 1, IdPlayer, num);
//    //          //DeleteTetra[num] = false;
//    //          //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": RpcDeleteRow IsWork");
//    //          gameHelper.DeleteRow(IdPlayer, num, Row);
//    //      }
//    //          //if (Game.GameMode == 1)     //Удаление первой каменной линии (HardCore Mode)
//    //          //{
//    //          //    DeletePoint(0, IdPlayer);
//    //          //    AllRowsDown(0 + 1, IdPlayer);
//    //          //}

//    //          //y--;

//    //          // limitStoneHeight--;
//    //      //}
//    //  }

//    //  //private void AllRowsDown(int y, string IdPlayer, int num)     //Опускает сторки вниз начиная начиная с строки "y" 
//    //  //{
//    //  //    //if (this.isLocalPlayer && !this.isServer && !this.isClient)
//    //  //    //{
//    //  //        //int num = GetNumPlayer(IdPlayer);
//    //  //        for (int i = y; i < gridArr[num].GetLength(1); i++)
//    //  //        {
//    //  //            RowsDown(i, IdPlayer, num);
//    //  //        }
//    //  //    //}
//    //  //}

//    //  //private void RowsDown(int y, string IdPlayer, int num)    //Опускает одну строку "y"
//    //  //{
//    //  //    //if (this.isLocalPlayer && !this.isServer && !this.isClient)
//    //  //    //{
//    //  //        //int num = GetNumPlayer(IdPlayer);
//    //  //        for (int x = 0; x < gridArr[num].GetLength(0); x++)
//    //  //        {
//    //  //            if (gridArr[num][x, y] != null)
//    //  //            {
//    //  //                gridArr[num][x, y - 1] = gridArr[num][x, y];

//    //  //                gridArr[num][x, y] = null;

//    //  //                gridArr[num][x, y - 1].position += new Vector3(0, -1, 0);
//    //  //            }
//    //  //        }
//    //  //    //}
//    //  //}

//    //  ////[ClientRpc(channel = 0)]
//    //  //public void DeletePoint(int y, string IdPlayer, int num)     //Удалени ряда, по 1 точке за каждую итерацию цикла
//    //  //{
//    //  //    //  int num = GetNumPlayer(IdPlayer);
//    //  //    //if (this.isLocalPlayer && !this.isServer)
//    //  //    //{
//    //  //    Debug.Log(y);
//    //  //    for (int x = 0; x < gridArr[num].GetLength(0); x++)
//    //  //    {
//    //  //        try
//    //  //        {
//    //  //            //if (gridArr[num][x, y].gameObject != null)
//    //  //            //{
//    //  //                NetworkManager.Destroy(gridArr[num][x, y].gameObject);
//    //  //                gridArr[num][x, y] = null;
//    //  //            //}

//    //  //        }
//    //  //        catch(NullReferenceException e)
//    //  //        {
//    //  //            Debug.LogError(e + "    x: " + x + "     y: " + y + "    num: " + num);
//    //  //        }
//    //  //    }
//    //  //    // }
//    //  //}

//    //  [ClientRpc(channel = 0)]
//    //  public void RpcEnableTetra(/*int idPlayer, */int num)
//    //  {
//    //      if(!this.isLocalPlayer && !this.isServer)
//    //      {

//    //          gameHelper.EnableTetra(num);
//    //          //EnableTetra = true;
//    //          //RpcUpdateGridPlayer(NowTetrominoArr[num].GetComponent<TetrominoPlayer>(), num, "0");
//    //          //NowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = false;

//    //          //PosEnableTetra[num] = NowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.position;

//    //          //RpcDeleteRow(idPlayer)
//    //          //NowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.position = new Vector2(num * ShiftFactorFrame + 5, 22);
//    //      }
//    //  }

//    //  private Vector2 RoundVec2(Vector2 pos)   //Округление вектора 
//    //  {
//    //      Vector2 vec = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
//    //      return vec;
//    //  }

//    //  [ClientRpc(channel = 0)]
//    //  public void RpcGameOver(int num)
//    //  {
//    //      //GameOverArr[num] = true;
//    //      //if(IdPlayers[num] == IdPlayer)
//    //      //{
//    //      //    GameOver = true;
//    //      //}
//    //      gameHelper.GameOver(num);
//    //  }
//}
