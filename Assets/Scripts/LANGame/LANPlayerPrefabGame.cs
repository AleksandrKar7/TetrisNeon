using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LANPlayerPrefabGame : NetworkBehaviour
{
    //public GameObject gObjMenuLANGame;
    private static int lenght =  4;

    public LANPlayerPrefabHelper LANPPH;
    private GameOnLANV2 game;
    private NetworkManager NM;
    private string idPlayer;

    float timeLastFall = 0;
    float timeLastMoveHorizontal = 0;
    private int fastDropCoefficient = 4;    //Коефициент ускорения падения при зажатой клавише
    private double speedFastMoveHorizontal = 0.25;  //Коефициент ускорения горизонтального движения при зажатой клавише

    public bool itGamePouse = false;
    private static bool itGameOver = false;
    private static bool[] itGameOverArr = new bool[4];

    private bool[] itGameStartArr = new bool[lenght];

    private GameObject[] nowTetrominoArr = new GameObject[lenght];
    private GameObject[] previewTetrominoArr = new GameObject[lenght];
    private GameObject[] stoneLineArr = new GameObject[lenght];

    private static int gridMaxWidth = 10;   //Ширина игрового поля
    private static int gridMaxHeight = 20;  //Высота игрового поля  

    public Transform[][,] gridArr = new Transform[lenght][,];

    private string prefixTetrominoName = "Prefabs/LANGame/TetrominoPlayer/";

    private static bool[] itDeleteTetraArr = new bool[lenght];

    private float gameSpeed = 0;
    private float shiftFactorFrame = 0;
    private float stepSize = 0;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < gridArr.Length; i++)
        {
            gridArr[i] = new Transform[gridMaxWidth, gridMaxHeight];
        }

        NM = FindObjectOfType<NetworkManager>();
        idPlayer = LANPPH.GetIdPlayer();
        game = null;
    }

    public void InitializeGameVariables(float gameSpeed, float shiftFactorFrame, float stepSize)
    {
        RpcInitializeGameVariables(gameSpeed, shiftFactorFrame, stepSize);
    }

    [ClientRpc(channel = 0)]
    public void RpcInitializeGameVariables(float gameSpeed, float shiftFactorFrame, float stepSize)
    {
        this.gameSpeed = gameSpeed;
        this.shiftFactorFrame = shiftFactorFrame;
        this.stepSize = stepSize;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Инициализированы игровые переменные: "
            + "Скорость игры: " + gameSpeed + "; "
            + "Отступ мужду полями: " + shiftFactorFrame + "; "
            + "Шаг фигур: " + stepSize + ". ");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneManagerHelper.SceneManagerEnum.LANGame
            && game == null 
            && isLocalPlayer 
            && isServer)
        {
            RpcClearAllVariable();

            SpawnGame();
            string[] idPlayers = new string[lenght];
            string[] nickNames = new string[lenght];
            for (int i = 0; i < idPlayers.Length; i++)
            {
                if (LANPPH.GetLANPlayerArrayElement(i) != null)
                {
                    idPlayers[i] = LANPPH.GetLANPlayerArrayElement(i).GetIdPlayer();
                    nickNames[i] = LANPPH.GetLANPlayerArrayElement(i).GetNickName();
                    string pathToAvatar = Application.dataPath + @"/Resources/TempImages/" + idPlayers[i] + ".png";
                    RpcInitializeFrame(i, pathToAvatar, nickNames[i]);
                }
            }
            RpcSetSizeCamera(NM.numPlayers);
            game.StartGame(NM.numPlayers, idPlayers);
        }

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneManagerHelper.SceneManagerEnum.LANGame
            && isLocalPlayer)
        {
            CheckInputKey();
        }
    }

    [ClientRpc(channel = 0)]
    public void RpcInitializeFrame(int i, string pathToAvatar, string nickName)
    {
        FindObjectOfType<LANGame>().playerFramesArray[i].SetActive(true);
        FindObjectOfType<LANGame>().playerFramesArray[i].InitializeFramePlayerPanel(pathToAvatar, nickName);
    }

    [ClientRpc(channel = 0)]
    public void RpcSetSizeCamera(int countPlayer)
    {
        if(countPlayer == 3)
        {
            FindObjectOfType<Camera>().transform.position = new Vector3(625, 275, -10);
            FindObjectOfType<Camera>().orthographicSize = 550;

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отрегулирован размер камеры под 3 игроков. "
                + " Размер камеры: " + FindObjectOfType<Camera>().orthographicSize
                + " Координаты: " + FindObjectOfType<Camera>().transform.position.ToString());
        }
    }

    [ClientRpc(channel = 0)] 
    public void RpcClearAllVariable()
    {
        for (int i = 0; i < lenght; i++)
        {
            itGamePouse = false;

            itDeleteTetraArr[i] = false;
            itGameStartArr[i] = false;
            nowTetrominoArr[i] = null;
            previewTetrominoArr[i] = null;
            stoneLineArr[i] = null;
            itGameOverArr[i] = false;

            for (int j = 0; j < gridArr[i].GetLength(0); j++)
            {
                for (int k = 0; k < gridArr[i].GetLength(1); k++)
                {
                    gridArr[i][j, k] = null;
                }
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Очистка всех переменных на клиенте");
    }

    private LANPlayerPrefabGame FindHostLANPlayerPrefabGame()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск игрового объекта хоста");

        LANPlayerPrefabGame[] lANPlayerPrefabGame = FindObjectsOfType<LANPlayerPrefabGame>();
        for (int i = 0; i < lANPlayerPrefabGame.Length; i++)
        {
            if (lANPlayerPrefabGame[i].isServer 
                && lANPlayerPrefabGame[i].isLocalPlayer)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск игрового объекта хоста");

                return lANPlayerPrefabGame[i];
            }
        }

        Debug.LogWarning(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поиск игрового объекта хоста НЕ дал результатов! Отработка метода происходит НЕ на машине хоста!");

        return null;
    }

    private void SpawnGame()
    {
        GameObject go = new GameObject("GameScript");
        game = go.AddComponent<GameOnLANV2>();
        game.SetHost(this);
    }

    void CheckInputKey()
    {
        if (this.isLocalPlayer 
            && itGameOver == false)
        {
            if (itGamePouse == false)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) 
                    || Input.GetKeyDown(KeyCode.D))
                {
                    CmdMove(KeyCode.RightArrow, idPlayer);

                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": От игрока с id: " + idPlayer 
                        + "на сервер отправлен сигнал на движение вправо ");

                    timeLastMoveHorizontal = Time.time;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) 
                    || Input.GetKeyDown(KeyCode.A))
                {
                    CmdMove(KeyCode.LeftArrow, idPlayer);

                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": От игрока с id: " + idPlayer
                       + "на сервер отправлен сигнал на движение влево ");

                    timeLastMoveHorizontal = Time.time;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) 
                    || Input.GetKeyDown(KeyCode.W))
                {
                    CmdMove(KeyCode.UpArrow, idPlayer);

                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": От игрока с id: " + idPlayer
                       + "на сервер отправлен сигнал на поворот ");
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) 
                    || Input.GetKeyDown(KeyCode.S))
                {
                    CmdMove(KeyCode.DownArrow, idPlayer);

                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": От игрока с id: " + idPlayer
                       + "на сервер отправлен сигнал на движение вниз ");

                    timeLastFall = Time.time;
                }

                if ((double)Time.time - timeLastMoveHorizontal >= speedFastMoveHorizontal
                    && (Input.GetKey(KeyCode.LeftArrow)
                    || Input.GetKey(KeyCode.A)))
                {
                    CmdMove(KeyCode.LeftArrow, idPlayer);

                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": От игрока с id: " + idPlayer
                        + "на сервер отправлен сигнал на движение влево ");

                    timeLastMoveHorizontal = Time.time;
                }

                if ((double)Time.time - timeLastMoveHorizontal >= speedFastMoveHorizontal 
                    && (Input.GetKey(KeyCode.RightArrow) 
                    || Input.GetKey(KeyCode.D)))
                {
                    CmdMove(KeyCode.RightArrow, idPlayer);

                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": От игрока с id: " + idPlayer
                        + "на сервер отправлен сигнал на движение вправо ");

                    timeLastMoveHorizontal = Time.time;
                }

                if((double)Time.time - timeLastMoveHorizontal >= speedFastMoveHorizontal
                    && (Input.GetKey(KeyCode.DownArrow)
                    || Input.GetKey(KeyCode.S)))
                {
                    CmdMove(KeyCode.DownArrow, idPlayer);

                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": От игрока с id: " + idPlayer
                        + "на сервер отправлен сигнал на движение вправо ");

                    timeLastMoveHorizontal = Time.time;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                itGamePouse = !itGamePouse;
                if (itGamePouse)
                {
                    FindObjectOfType<LANMenuEsc>().FindCurrentPlayer();
                    FindObjectOfType<LANMenuEsc>().ShowMessageBox();
                }
                else
                {
                    FindObjectOfType<LANMenuEsc>().HideMessageBox();
                }
            }
        }
    }

    [Command(channel = 0)]
    public void CmdMove(KeyCode key, string IdPlayer)
    {
        if (FindHostLANPlayerPrefabGame().game != null)
        {
            FindHostLANPlayerPrefabGame().game.CheckPlayerInputKey(key, IdPlayer);
        }
    }

    [ClientRpc(channel = 0)]
    public void RpcSpawnNextTetromino(string IdPlayer, int num, string NowTetra, string NextTetra, Vector3 nowTetrominoSpawnPosition, Vector3 previewTetrominoSpawnPosition)    //Установки следующей фигуры из препромотра на поле, и установка новой фигуры для препросмотра
    {
        if (!this.isLocalPlayer 
            && !this.isServer)
        {
            if (itGameStartArr[num] == false)
            {
                itGameStartArr[num] = true;

                nowTetrominoArr[num] = (GameObject)Instantiate(Resources.Load(prefixTetrominoName + NowTetra, typeof(GameObject))
                    , nowTetrominoSpawnPosition
                    , Quaternion.identity);

                nowTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
                nowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = true;
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установленна новая фигура на клиенте: " + nowTetrominoArr[num].name + " для " + num + " игрока с id: " + IdPlayer);

                previewTetrominoArr[num] =  (GameObject)Instantiate(Resources.Load(prefixTetrominoName + NextTetra, typeof(GameObject))
                    , previewTetrominoSpawnPosition
                    , Quaternion.identity);

                previewTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = false;
                previewTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлен новая фигура для предпросмотра на клиенте: " + nowTetrominoArr[num].name + " для " + num + " игрока с id: " + IdPlayer);

            }
            else
            {
                previewTetrominoArr[num].transform.position = nowTetrominoSpawnPosition;
                nowTetrominoArr[num] = previewTetrominoArr[num];
                
                nowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = true;
                nowTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer = IdPlayer;
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Фигура из предпросмотра на клиенте " + nowTetrominoArr[num].name + " для " + num
                      + " игрока с id: " + IdPlayer + " активирована и перемещенна на место спавна новой фигур");

                previewTetrominoArr[num] = (GameObject)Instantiate(Resources.Load(prefixTetrominoName + NextTetra, typeof(GameObject))
                    , previewTetrominoSpawnPosition
                    , Quaternion.identity);

                previewTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = false;
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлен новая фигура для предпросмотра на клиенте: " + nowTetrominoArr[num].name + " для " + num + " игрока с id: " + IdPlayer);
            }
        }
    }

    [ClientRpc(channel = 0)]
    public void RpcSynchronizedTetromino(string IdPlayer, Vector3 pos, int num, Quaternion qu)
    {
        if (!this.isLocalPlayer 
            && !this.isServer)
        {          
            if (nowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled == true && itDeleteTetraArr[num] == false)
            {              
                if (nowTetrominoArr[num].GetComponent<TetrominoPlayer>().IdPlayer == IdPlayer)
                {
                    nowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.position = pos;
                    nowTetrominoArr[num].GetComponent<TetrominoPlayer>().transform.rotation = qu;
                    UpdateGridPlayer(nowTetrominoArr[num].GetComponent<TetrominoPlayer>(), num, IdPlayer);
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Синхронизация фигуры с сервером на клиенте: " + nowTetrominoArr[num].name + " для " + num + " игрока с id: " + IdPlayer);
                }
            }
        }
    }

    public void UpdateGridPlayer(TetrominoPlayer tetromino, int num, string idPlayer)
    {
        if (!this.isLocalPlayer 
            && !this.isServer)
        {
            if (tetromino.IdPlayer == idPlayer)
            {
                for (int x = 0; x < gridArr[num].GetLength(0); x++)
                {
                    for (int y = 0; y < gridArr[num].GetLength(1); y++)
                    {
                        if (gridArr[num][x, y] != null)
                        {
                            if (gridArr[num][x, y].parent == tetromino.transform)
                            {
                                gridArr[num][x, y] = null;

                                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Очищена устаревшая внутренняя информация об " + tetromino.name
                                    + " с id: " + tetromino.IdPlayer + " в ячейке"
                                    + " Х: " + x
                                    + " Y: " + y);
                            }
                        }
                    }
                }
            }

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Удалена устаревшая внутренняя информация об " + tetromino.name + " с id: " + tetromino.IdPlayer + " в игровом поле");

            foreach (Transform mino in tetromino.transform)
            {
                Vector2 pos = RoundVec2(mino.position);
                Vector2 vec = new Vector2(pos.x - num * shiftFactorFrame, pos.y);

                if (ToLowerCoordinates(pos).y < gridMaxHeight && ToLowerCoordinates(vec).x < gridMaxWidth && ToLowerCoordinates(vec).x >= 0)
                {
                    try
                    {
                        gridArr[num][(int)((pos.x - num * shiftFactorFrame) / stepSize), (int)(pos.y / stepSize)] = mino;

                        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Внесенна новая внутренняя информация об " + tetromino.name
                            + " с id: " + tetromino.IdPlayer + " в ячейке"
                            + " Х: " + ((int)pos.x / stepSize)
                            + " Y: " + ((int)pos.y / stepSize));
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Внесенна новая внутренняя информация об " + tetromino.name + " с id: " + tetromino.IdPlayer + " в игровом поле");
    }

    public Vector2 RoundVec2(Vector2 pos)   //Округление вектора 
    {
        Vector2 vec = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        return vec;
    }

    [ClientRpc(channel = 0)]
    public void RpcEnableTetra(string idPlayer, int num)
    {
        if (!this.isLocalPlayer 
            && !this.isServer)
        {
            UpdateGridPlayer(nowTetrominoArr[num].GetComponent<TetrominoPlayer>(), num, idPlayer);
            nowTetrominoArr[num].GetComponent<TetrominoPlayer>().enabled = false;
        }
    }

    [ClientRpc(channel = 0)]
    public void RpcDeleteRow(string idPlayer, int num, int row)     //Вызывается при удалении строки
    {       
        if (!this.isServer)
        {
            UpdateGridPlayer(nowTetrominoArr[num].GetComponent<TetrominoPlayer>(), num, idPlayer);
            //RpcEnableTetra(num);
            itDeleteTetraArr[num] = true;
            DeletePoint(row, idPlayer, num);
            AllRowsDown(row + 1, idPlayer, num);
            itDeleteTetraArr[num] = false;
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": RpcDeleteRow IsWork");
        }
    }

    private void AllRowsDown(int y, string IdPlayer, int num)     //Опускает сторки вниз начиная начиная с строки "y" 
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Опуск всех строк начиная с " + y + "строки у игрока с id: " + IdPlayer);

        for (int i = y; i < gridArr[num].GetLength(1); i++)
        {
            RowsDown(i, IdPlayer, num);
        }
    }

    private void RowsDown(int y, string IdPlayer, int num)    //Опускает одну строку "y"
    {
        for (int x = 0; x < gridArr[num].GetLength(0); x++)
        {
            if (gridArr[num][x, y] != null)
            {
                gridArr[num][x, y - 1] = gridArr[num][x, y];

                gridArr[num][x, y] = null;

                gridArr[num][x, y - 1].position += new Vector3(0, -stepSize, 0);
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Опущена " + y + "строка у " + num + " игрока");
    }

    public void DeletePoint(int y, string IdPlayer, int num)     //Удалени ряда, по 1 точке за каждую итерацию цикла
    {
        for (int x = 0; x < gridArr[num].GetLength(0); x++)
        {
            try
            {
                NetworkManager.Destroy(gridArr[num][x, y].gameObject);
                gridArr[num][x, y] = null;
            }
            catch (NullReferenceException e)
            {
                Debug.LogError(" Объект для удаления не обнаружен: " + e.ToString());
            }
        }
    }

    [ClientRpc(channel = 0)]
    public void RpcGameOver(string idPlayer, int num)
    {
        if (num < itGameOverArr.Length)
        {
            itGameOverArr[num] = true;
            if (LANPPH.GetIdPlayer() == idPlayer)
            {
                itGameOver = true;
            }
        }
    }

    [ClientRpc(channel = 0)]
    public void RpcLoadLobby()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: "
            + SceneManagerHelper.SceneManagerEnum.LANLobby.ToString() + " .");

        SceneManager.LoadScene((int)SceneManagerHelper.SceneManagerEnum.LANLobby); 
    }

    public void GameWin(string idWinner)
    {
        LANPPH.SetWinner(idWinner);
        RpcLoadLobby();
    }

    [ClientRpc(channel = 0)]
    public void RpcSpawnStoneLine(string Id, int num, Vector3 stoneLineSpawnPossition)
    {
        if (!this.isServer)
        {
            UpAllRows(Id, num);
            stoneLineArr[num] = (GameObject)Instantiate(Resources.Load(prefixTetrominoName + "StoneLine", typeof(GameObject)), stoneLineSpawnPossition, Quaternion.identity);
            stoneLineArr[num].GetComponent<TetrominoPlayer>().IdPlayer = Id;
            UpdateGridPlayer(stoneLineArr[num].GetComponent<TetrominoPlayer>(), num, stoneLineArr[num].GetComponent<TetrominoPlayer>().IdPlayer);
            stoneLineArr[num].GetComponent<TetrominoPlayer>().enabled = false;

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Устаноленна каменная линия у клиента для " + num + " игрока");
        }
    }

    private void UpAllRows(string Id, int num)    //Поднятие всех объектов для освобождения места каменной линии (HardCore Mode)
    {
        for (int y = gridArr[num].GetLength(1) - 2; y > -1; y--)
        {
            for (int x = 0; x < gridArr[num].GetLength(0); x++)
            {
                if (gridArr[num][x, y] != null)
                {
                    gridArr[num][x, y + 1] = gridArr[num][x, y];

                    gridArr[num][x, y] = null;

                    gridArr[num][x, y + 1].position += new Vector3(0, +stepSize, 0);
                }
            }

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поднята " + y + "линия у клиента для " + num + " игрока");
        }
    }

    public void PlayerSurrend()
    {
        if (isLocalPlayer)
        {
            CmdSurrender(LANPPH.GetIdPlayer());
        }
    }

    [Command(channel = 0)]
    public void CmdSurrender(string idPlayer)
    {
        FindHostLANPlayerPrefabGame().Surrender(idPlayer);
    }

    private void Surrender(string idPlayer)
    {
        int num = LANPPH.GetNumById(idPlayer);
        itGameOverArr[num] = true;
        game.GameOver(num);
    }

    public Vector2 ToLowerCoordinates(Vector2 vec)  //Переводит реальные координаты в координаты с шаговом в 1 понятный для массивов
    {
        return new Vector2(vec.x / stepSize, vec.y / stepSize);
    }
}
