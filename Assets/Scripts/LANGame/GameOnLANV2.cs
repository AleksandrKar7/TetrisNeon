using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class GameOnLANV2 : NetworkBehaviour
{
    private static int lenght = 4;

    public double gameSpeed = 5;   //скорость падения
    public float stepSize = 25;
    private int shiftFactorFrame = 450;

    private int[] limitStoneHeight = new int[lenght];   //Используется для правильной утановки каменной линии (HardCore Mode)

    private GameObject[] nowTetrominoArr = new GameObject[lenght];
    private GameObject[] previewTetrominoArr = new GameObject[lenght];
    private GameObject[] stoneLineArr = new GameObject[lenght];   //Каменная линия (HardCore Mode)

    private Vector3[] tetrominoSpawnPosition = new Vector3[lenght];  //Место спавна фигуры
    private Vector3[] tetrominoPreviewSpawnPosition = new Vector3[lenght];  //Место спавна для предпросотра фигуры  
    private Vector3[] stoneLineSpawnPosition = new Vector3[lenght];   //Место спавна первой каменной

    private static int gridMaxHeight = 20;  //Высота игрового поля  
    private static int gridMaxWidth = 10;   //Ширина игрового поля  

    public Transform[][,] gridArr = new Transform[lenght][,];

    public string[] idPlayersArr = new string[lenght];

    public bool itRowDelete = false;
    public bool itGamePouse = false;  //Тригер который ставит игру на паузы при вызове меню Esc (Помечено)
    private bool[] itGameStartArr = new bool[lenght];

    private float[] playersPositionsXarr = new float[lenght];
    private float[] playersPositionsYarr = new float[lenght];

    private Vector3[] playerPositionArr = new Vector3[lenght];

    private LANPlayerPrefabGame host;
    public void SetHost(LANPlayerPrefabGame host)
    {
        this.host = host;
    }

    private static string[] tetrominoNameArr = { "Tetromino_Duck"
            , "Tetromino_RDuck"
            , "Tetromino_J"
            , "Tetromino_L"
            , "Tetromino_Long"
            , "Tetromino_Pyramid"
            , "Tetromino_Square"};  //Массив с именами фигур

    private string prefixesTetrominoName = "Prefabs/LANGame/TetrominoHost/";

    public int countPlayer = 0;

    private static int score = 0;

    private static bool[] itGameOverArr = new bool[lenght];
    public int GetScore()
    {
        return score;
    }

    public void UpScore()
    {
        score++;
    }

    public Vector2 RoundVec2(Vector2 pos)   //Округление вектора 
    {
        Vector2 vec = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        return vec;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {     
     
    }

    public void ClearAllVariable()
    {
        for (int i = 0; i < lenght; i++)
        {
            limitStoneHeight[i] = 0;
            nowTetrominoArr[i] = null;
            previewTetrominoArr[i] = null;
            stoneLineArr[i] = null;
            itGameOverArr[i] = false;
            
            for (int j = 0; j < gridArr[i].GetLength(0); j++)
            {
                for(int k = 0; k < gridArr[i].GetLength(1); k++)
                {
                    gridArr[i][j, k] = null;
                }
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Очистка всех переменных на сервере");
    }

    public void StartGame(int Connected, string[] IdPlayers)
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Игра началась");

        for (int i = 0; i < gridArr.Length; i++)
        {
            gridArr[i] = new Transform[gridMaxWidth, gridMaxHeight];
        }

        ClearAllVariable();

        countPlayer = Connected;

        for(int i = 0; i < lenght; i++)
        {
            tetrominoSpawnPosition[i] = new Vector3(i * shiftFactorFrame + 5 * stepSize, 19 * stepSize, -5);
            tetrominoPreviewSpawnPosition[i] = new Vector3(i * shiftFactorFrame + 13f * stepSize, 16 * stepSize, -5);
            stoneLineSpawnPosition[i] = new Vector3 (i * shiftFactorFrame, 0, -5);
        }

        host.InitializeGameVariables((float)gameSpeed, shiftFactorFrame, stepSize);
        for (int i = 0; i < this.idPlayersArr.Length; i++)
        {
            this.idPlayersArr[i] = IdPlayers[i];
            Debug.Log(this.idPlayersArr[i]);
        }
        for (int i = 0; i < countPlayer; i++)
        {
            SpawnNextTetromino(idPlayersArr[i]);
        }
    }

    public void GameOver(int i)
    {
        itGameOverArr[i] = true;
        GameOver(idPlayersArr[i]);
    }

    string NowTetra = "";
    public void SpawnNextTetromino(string IdPlayer)    //Установки следующей фигуры из препромотра на поле, и установка новой фигуры для препросмотра
    {
        int num = GetNumPlayer(IdPlayer);

        string NextTetra = "";
        if (itGameOverArr[num] == false)
        {
            if (!itGameStartArr[num])
            {
                itGameStartArr[num] = true;

                NowTetra = GetRandomTetromino();
                NextTetra = GetRandomTetromino();
                nowTetrominoArr[num] = (GameObject)NetworkManager.Instantiate(Resources.Load(prefixesTetrominoName + NowTetra, typeof(GameObject)), tetrominoSpawnPosition[num], Quaternion.identity);
                nowTetrominoArr[num].GetComponent<TetrominoOnLANV2>().IdPlayer = idPlayersArr[num];
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установленна новая фигура на сервере: " + nowTetrominoArr[num].name + " для " + num + " игрока с id: " + IdPlayer);

                previewTetrominoArr[num] = (GameObject)NetworkManager.Instantiate(Resources.Load(prefixesTetrominoName + NextTetra, typeof(GameObject)), tetrominoPreviewSpawnPosition[num], Quaternion.identity);
                previewTetrominoArr[num].GetComponent<TetrominoOnLANV2>().enabled = false;
                previewTetrominoArr[num].GetComponent<TetrominoOnLANV2>().IdPlayer = idPlayersArr[num];
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлен новая фигура для предпросмотра на сервере: " + nowTetrominoArr[num].name + " для " + num + " игрока с id: " + IdPlayer);
            }
            else
            {
                NextTetra = GetRandomTetromino();

                previewTetrominoArr[num].transform.localPosition = tetrominoSpawnPosition[num];
                nowTetrominoArr[num] = previewTetrominoArr[num];
                nowTetrominoArr[num].GetComponent<TetrominoOnLANV2>().enabled = true;
                nowTetrominoArr[num].GetComponent<TetrominoOnLANV2>().IdPlayer = idPlayersArr[num];
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Фигура из предпросмотра на сервере " + nowTetrominoArr[num].name + " для " + num 
                    + " игрока с id: " + IdPlayer + " активирована и перемещенна на место спавна новой фигур");

                previewTetrominoArr[num] = (GameObject)NetworkManager.Instantiate(Resources.Load(prefixesTetrominoName + NextTetra, typeof(GameObject)), tetrominoPreviewSpawnPosition[num], Quaternion.identity);
                previewTetrominoArr[num].GetComponent<TetrominoOnLANV2>().enabled = false;
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлен новая фигура для предпросмотра на сервере: " + nowTetrominoArr[num].name + " для " + num + " игрока с id: " + IdPlayer);
            }

            host.RpcSpawnNextTetromino(IdPlayer, num, NowTetra, NextTetra, tetrominoSpawnPosition[num], tetrominoPreviewSpawnPosition[num]);
            host.RpcSynchronizedTetromino(IdPlayer, tetrominoSpawnPosition[num], num, Quaternion.identity);
        }
    }

    private string GetRandomTetromino()     //Возвращает случайное имя фигуры из массива "TetrominoNameArr"
    {
        string RandTetraName;

        int RandNumbTetra = UnityEngine.Random.Range(0, tetrominoNameArr.Length);

        RandTetraName = tetrominoNameArr[RandNumbTetra];

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Полученна случайная фигура: " + RandTetraName);

        return RandTetraName;
    }

    private int GetNumPlayer(string IdPlayer)
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получение порядкового номера игрока по id: " + IdPlayer);

        for (int i = 0; i < idPlayersArr.Length; i++)
        {
            if (idPlayersArr[i] == IdPlayer)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получение порядкового номера игрока по id: " + IdPlayer +  " дал результат: " + i);

                return i;
            }
        }
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получение порядкового номера игрока по id: " + IdPlayer + " НЕ дал результатов");

        return -1;
    }

    public void UpdateGrid(TetrominoOnLANV2 tetromino, string IdPlayer)    //заполнение массива grid
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Обновление внутренней информации об " + tetromino.name + " с id: " + IdPlayer + " в игровом поле");

        int num = GetNumPlayer(IdPlayer);
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
                            + " с id: " + IdPlayer + " в ячейке"
                            + " Х: " + x
                            + " Y: " + y);
                    }
                }
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Удалена устаревшая внутренняя информация об " + tetromino.name + " с id: " + IdPlayer + " в игровом поле");

        foreach (Transform mino in tetromino.transform)
        {
            Vector2 pos = RoundVec2(mino.position);
            Vector2 vec = new Vector2(pos.x - num * shiftFactorFrame, pos.y);

            if (ToLowerCoordinates(pos).y < gridMaxHeight && ToLowerCoordinates(vec).x < gridMaxWidth && ToLowerCoordinates(vec).x >= 0)
            {

                gridArr[num][(int)(ToLowerCoordinates(vec).x), (int)(ToLowerCoordinates(vec).y)] = mino;

                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Внесенна новая внутренняя информация об " + tetromino.name 
                     + " с id: " + IdPlayer + " в ячейке"
                     + " Х: " + ((int)vec.x / stepSize)
                     + " Y: " + ((int)vec.y / stepSize));
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Внесенна новая внутренняя информация об " + tetromino.name + " с id: " + IdPlayer + " в игровом поле");
    }

    public Transform GetTransformGridPosition(Vector2 pos, string IdPlayer)
    {
        int num = GetNumPlayer(IdPlayer);
        if (ToLowerCoordinates(pos).y > gridMaxHeight - 1)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отказано в получение внутренней информация "
                +  "с id: " + IdPlayer + " в ячейке"
                + " Х: " + (int)pos.x / stepSize
                + " Y: " + (int)pos.y / stepSize
                + " в игровом поле");

            return null;
        }
        else
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получение внутренней информация" 
                 +  "с id: " + IdPlayer + " в ячейке"
                 + " Х: " + (int)pos.x / stepSize
                 + " Y: " + (int)pos.y / stepSize
                 + " в игровом поле");

            Vector2 vec = new Vector2(pos.x - num * shiftFactorFrame, pos.y);
            return gridArr[num][(int)(ToLowerCoordinates(vec).x), (int)(ToLowerCoordinates(vec).y)];
        }

    }

    public bool OnTheFrame(Vector2 pos, string IdPlayer)     //Проверка "Находится ли объект (фигура) в пределах поля"
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка находится ли объект с координатами " + pos.ToString()
            + " с id: " + IdPlayer + " в игровом поле");

        int num = GetNumPlayer(IdPlayer);
        if ((int)pos.x >= (num * shiftFactorFrame) && (int)pos.x < (num * shiftFactorFrame) + gridMaxWidth * stepSize && (int)pos.y >= 0)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Объект с координатами " + pos.ToString() 
                + " с id: " + IdPlayer + " находится в игровом поле");

            return true;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Объект с координатами " + pos.ToString() 
            + " с id: " + IdPlayer + " НЕ находится в игровом поле");

        return false;
    }

    private bool ItRowFull(int y, string IdPlayer)   //Проверка "Заполнена ли строка?" Заполнена - true, Нет - false
    {
        int num = GetNumPlayer(IdPlayer);
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на заполнена ли " + y + " линия у " + num + " игрока");

        if (y < limitStoneHeight[num])
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Строка " + y + " у " + num + " игрока находится вне поля");

            return false;
        }

        for (int x = 0; x < gridArr[num].GetLength(0); x++)
        {
            if (gridArr[num][x, y] == null)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Строка " + y + " у " + num + " игрока не заполненна");

                return false;
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Строка " + y + " у " + num + " игрока заполненна");

        return true;
    }

    private void DeletePoint(int y, string IdPlayer)     //Удалени ряда, по 1 точке за каждую итерацию цикла
    {
        int num = GetNumPlayer(IdPlayer);
        for (int x = 0; x < gridArr[num].GetLength(0); x++)
        {
            Destroy(gridArr[num][x, y].gameObject);
            gridArr[num][x, y] = null;
        }
    }

    public void DeleteRow(string idPlayer)     //Вызывается при удалении строки
    {   
        int num = GetNumPlayer(idPlayer);

        for (int y = 0; y < gridArr[num].GetLength(1); y++)
        {
            if (ItRowFull(y, idPlayer))
            {
                itRowDelete = true;

                DeletePoint(y, idPlayer);
                AllRowsDown(y + 1, idPlayer);

                SpawnStoneLine(idPlayer);

                host.RpcEnableTetra(idPlayer, num);
                host.RpcDeleteRow(idPlayer, num, y);

                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Удалена " + y + " строка у " + num + " игрока");

                y--;
            }
        }
    }

    private void RowsDown(int y, string IdPlayer)    //Опускает одну строку "y"
    {
        int num = GetNumPlayer(IdPlayer);
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

    private void AllRowsDown(int y, string IdPlayer)     //Опускает сторки вниз начиная начиная с строки "y" 
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Опуск всех строк начиная с " + y + "строки у игрока с id: " + IdPlayer);
        int num = GetNumPlayer(IdPlayer);
        for (int i = y; i < gridArr[num].GetLength(1); i++)
        {
            RowsDown(i, IdPlayer);
        }
    }

    public void SynchronizedTetromino(string IdPlayer, Vector3 pos, Quaternion qu)
    {       
        int num = GetNumPlayer(IdPlayer);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало синхронизации фигуры " + num + " игрока: " 
            + "Коодинаты: " + pos.ToString() + "; "
            + "Угол: " + qu.ToString() + "; ");

        host.RpcSynchronizedTetromino(IdPlayer, pos, num, qu);
    }

    public void SynchronizedEnableTetra(string IdPlayer)
    {
        int num = GetNumPlayer(IdPlayer);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало синхронизации остановки фигуры " + num + " игрока: ");

        host.RpcEnableTetra(IdPlayer, num);
    }

    public void CheckPlayerInputKey(KeyCode key, string IdPlayer)
    {
        int num = GetNumPlayer(IdPlayer);

        if (num < 0)
        {
            return;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": На сервере принят сигнал от " + num + " игрока: " + key.ToString());

        if (itGameOverArr[num] == false)
        {
            if (key == KeyCode.RightArrow || key == KeyCode.D)
            {
                nowTetrominoArr[num].GetComponent<TetrominoOnLANV2>().MoveRight();
            }

            if (key == KeyCode.LeftArrow || key == KeyCode.A)
            {
                nowTetrominoArr[num].GetComponent<TetrominoOnLANV2>().MoveLeft();
            }

            if (key == KeyCode.UpArrow || key == KeyCode.W)
            {
                nowTetrominoArr[num].GetComponent<TetrominoOnLANV2>().Rotate();
            }

            if (key == KeyCode.DownArrow || key == KeyCode.S)
            {
                nowTetrominoArr[num].GetComponent<TetrominoOnLANV2>().MoveDown();
            }
        }
    }

    public bool ItGameOver(TetrominoOnLANV2 tetromino)     //Проверка на проигрыш
    {
        int num = GetNumPlayer(tetromino.IdPlayer);
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на проигрыш для " + num + " игрока");

        for (int x = 0; x < gridMaxWidth; x++)
        {
            foreach (Transform tetramino in tetromino.transform)
            {
                Vector2 vec = RoundVec2(tetramino.position);
                if (ToLowerCoordinates(vec).y > gridMaxHeight - 1)
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на проигрыш дала результат: " + num + " игрок проиграл");

                    return true;
                }
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на проигрыш дала результат: " + num + " игрок НЕ проиграл");
        return false;
    }

    public void GameOver(string idPlayer)  //Вызывается при проигрыше
    {
        int num = GetNumPlayer(idPlayer);
        itGameOverArr[num] = true;
        host.RpcGameOver(idPlayer, num);
        string IdSurvivorPlayer = WhoWins();
        if (IdSurvivorPlayer != null)
        {
            host.GameWin(IdSurvivorPlayer);
        }  
    }

    private string WhoWins()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка есть ли победитель?");

        string IdSurvivorPlayer = null;
        for (int i = 0; i < countPlayer; i++)
        {
            if(itGameOverArr[i] == false && IdSurvivorPlayer == null)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка есть ли победитель дал результат: " + i + " игрок победил");

                IdSurvivorPlayer = idPlayersArr[i];
            }
            else if (itGameOverArr[i] == false && IdSurvivorPlayer != null)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка есть ли победитель дал результат: победителя нет");

                IdSurvivorPlayer = null;
                break;
            }
        }
        return IdSurvivorPlayer;
    }

    public void SpawnStoneLine(string Id)    //Установка каменной линии (HardCore Mode)
    { 
        for (int i = 0; i < idPlayersArr.Length; i++)
        {
            if (idPlayersArr[i] != Id && idPlayersArr[i] != null && idPlayersArr[i] != "")
            {
                UpAllRows(idPlayersArr[i]);
                stoneLineArr[i] = (GameObject)Instantiate(Resources.Load(prefixesTetrominoName + "StoneLine", typeof(GameObject)), stoneLineSpawnPosition[i], Quaternion.identity);
                stoneLineArr[i].GetComponent<TetrominoOnLANV2>().IdPlayer = idPlayersArr[i];              
                limitStoneHeight[i]++;
                host.RpcSpawnStoneLine(Id, i, stoneLineSpawnPosition[i]);

                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Устаноленна " + limitStoneHeight + " каменная линия для " + i + " игрока");
            }
        }
    }

    private void UpAllRows(string Id)    //Поднятие всех объектов для освобождения места каменной линии (HardCore Mode)
    {
        int num = GetNumPlayer(Id);
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

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поднята " + y + "линия для " + num + " игрока");
        }
    }

    public Vector2 ToLowerCoordinates(Vector2 vec)  //Переводит реальные координаты в координаты с шаговом в 1 понятный для массивов
    {
        return new Vector2(vec.x / stepSize, vec.y / stepSize);
    }
}
