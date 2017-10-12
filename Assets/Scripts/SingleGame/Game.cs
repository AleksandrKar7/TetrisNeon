using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    private MessageBoxSingleGameMenu MBSGM;
    private MessageBoxGameOver MBGO;
    private PlayerProfile PP;

    public Text TxtNowRecord;
    public Text TxtYouBestRecord;
    public Text TxtBestRecord;
    public Text TxtGameMode;
    public Text TxtGameSpeed;

    private static int score = 0;    //Счет
    public int GetScore()
    {
        return score;
    }

    public void UpScore()
    {
        score++;
    }

    private static int bestRecord = 0;  //Лучший рекорд
    public int GetBestRecord()
    {
        return bestRecord;
    }

    private static int youBestRecord = 0;  //Лучший личный  рекорд
    public int GetYouBestRecord()
    {
        return youBestRecord;
    }

    private static int gridMaxHeight = 20;  //Высота игрового поля    
    private static int gridMaxWidth = 10;   //Ширина игрового поля   
    private static Transform[,] grid = new Transform[gridMaxWidth, gridMaxHeight];   //Массив игрового поля
    public Transform[,] GetGrid()
    {
        return grid;
    }

    private SingleGameHelper SGH;  // Является хранилищем для скорости и режима игры
    private double gameSpeed;   //Скорость падения фигуры в Секундах
    public double GetGameSpeed()
    {
        return gameSpeed;
    }

    private int limitStoneHeight = 0;   //Используется для правильной утановки каменной линии (HardCore Mode)
    private SingleGameHelper.GameModeEnum GameMode = SingleGameHelper.GameModeEnum.Classic;    //0-Clasic Mode; 1-HardCore Mode     
    public SingleGameHelper.GameModeEnum GetGameMode()
    {
        return GameMode;
    }

    private bool itRowDelete = false;
    public bool GetItRowDelete()
    {
        return itRowDelete;
    }
    public void SetItRowDelete(bool ird)
    {
        itRowDelete = ird;
    }
    private bool itGamePouse = false;  //Тригер который ставит игру на паузы при вызове меню Esc
    public void SetItGamePouse(bool GP)
    {
        itGamePouse = GP;
    }
    public bool GetItGamePouse()
    {
        return itGamePouse;
    }

    private bool itGameOverBool = false;  //Тригер который ставит игру на паузы при вызове меню Esc
    public void SetItGameOver(bool GO)
    {
        itGameOverBool = GO;
    }
    public bool GetItGameOver()
    {
        return itGameOverBool;
    }

    private static float playerPositionX = 0;   //Позиция игрового Prefab по Оси X (Помечено)
    private static float playerPositionY = 0;   //Позиция игрового Prefab по Оси Y (Помечено)
    public int stepSize = 25;

    //public AudioSource MoveSound;

    private string[] tetrominoNameArr = { "Tetromino_Duck"
            , "Tetromino_RDuck"
            , "Tetromino_J"
            , "Tetromino_L"
            , "Tetromino_Long"
            , "Tetromino_Pyramid"
            , "Tetromino_Square"};      //Массив с именами фигур
    public string[] GetTetrominoNameArr()
    {
        return tetrominoNameArr; 
    }

    private string prefixesTetrominoName = "Prefabs/SingleGame/";

    private bool GameStart = false;
    public bool gameStart
    {
        get { return GameStart; }
        set { GameStart = value; }
    }

    private GameObject NowTetromino;    //Текущая фигура
    public GameObject nowTetromino
    {
        get { return NowTetromino;  }
    }

    private GameObject PreviewTetromino;    //Следующая фигура
    public GameObject previewTetromino
    {
        get { return PreviewTetromino; }
    }

    private GameObject StoneLine;   //Каменная линия (HardCore Mode)
    public GameObject stoneLine
    {
        get { return StoneLine; }
    }

    private Vector2 tetrominoSpawnPosition = new Vector2(5.0f, 20.0f);  //Место спавна фигуры
    private Vector2 tetrominoPreviewSpawnPosition = new Vector2(13f, 18f);  //Место спавна для предпросотра фигуры  
    Vector2 stoneLineSpawnPosition = new Vector2(0f, 0f);   //Место спавна первой каменной линии (HardCore Mode)

    // Use this for initialization
    void Start ()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сцена: " + SceneManager.GetActiveScene().name + " загружена.");

        SGH = FindObjectOfType<SingleGameHelper>();
        MBSGM = FindObjectOfType<MessageBoxSingleGameMenu>();
        MBGO = FindObjectOfType<MessageBoxGameOver>();
        PP = FindObjectOfType<PlayerProfile>();
        score = 0;

        TxtGameMode.text = "Режим: " + SGH.GetGameMode().ToString();
        TxtGameSpeed.text = "Скорость: " + SGH.GetGameSpeed().ToString();

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получена и выведена информация об настройках игры:"
            + " Режим: " + SGH.GetGameMode().ToString() + " ;"
            + " Скорость: " + SGH.GetGameSpeed().ToString());

        Vector2 vec = transform.position;

        playerPositionX = vec.x + stepSize;
        playerPositionY = vec.y + stepSize;

        tetrominoSpawnPosition = new Vector2(playerPositionX + stepSize * 5, playerPositionY + stepSize * 22);
        tetrominoPreviewSpawnPosition = new Vector2(playerPositionX + stepSize * 15f, playerPositionY + stepSize * 16f);
        stoneLineSpawnPosition = new Vector2(playerPositionX, playerPositionY);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Заданы координаты спавна фигур:"
           + " Координаты спавна активной фигуры: " + tetrominoSpawnPosition.ToString() + " ;"
           + " Координаты спавна предпросмотра фигуры: " + tetrominoPreviewSpawnPosition.ToString());

        if (SGH != null)
        {
            gameSpeed = (11 - (double)SGH.GetGameSpeed()) / 10;          
            GameMode = SGH.GetGameMode();
        }
        else
        {
            gameSpeed = 1;
            GameMode = SingleGameHelper.GameModeEnum.Classic;
        }

        SpawnNextTetromino();

        bestRecord = PlayerPrefs.GetInt(PlayerPreferencesHelper.GetBestRecordKey(GameMode, SGH.GetGameSpeed()), 0);

        youBestRecord = PlayerPrefs.GetInt(PlayerPreferencesHelper.GetPersonalBestRecordKey(PP.profileNum
            , GameMode, SGH.GetGameSpeed()), 0);

        TxtBestRecord.text = bestRecord.ToString();
        TxtYouBestRecord.text = youBestRecord.ToString();

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получены и выведена информация об рекордах:"
            + " Общий рекорд: " + TxtYouBestRecord.text + " ;"
            + " Личный рекорд для " + PP.profileNum + "профиля: " + TxtBestRecord.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (itGamePouse == false)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Игра приостановлена.");
                MBSGM.ShowMessageBox();
            }
            else
            {
                MBSGM.ButtonClick_MBResume();
            }
        }
    }

    public void SpawnStoneLine()    //Установка каменной линии (HardCore Mode)
    {    
        UpAllRows();
        StoneLine = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject))
            , stoneLineSpawnPosition, Quaternion.identity);
    
        limitStoneHeight++;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Устаноленна " + limitStoneHeight + " каменная линия.");
    }

    public void UpAllRows()    //Поднятие всех объектов для освобождения места каменной линии (HardCore Mode)
    {
        for (int y = grid.GetLength(1)-2; y > -1; y--)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y + 1] = grid[x, y];

                    grid[x, y] = null;

                    grid[x, y + 1].position += new Vector3(0, stepSize, 0);                 
                }
            }
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поднята " + y + "линия");
        }
    }

    public bool ItRowFull(int y)   //Проверка "Заполнена ли строка?" Заполнена - true, Нет - false
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на заполнена ли " + y + " линия");

        if (y < limitStoneHeight || y >= gridMaxHeight || y < 0)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Строка " + y + " находится вне поля");

            return false;
        }
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            if (grid[x, y] == null)
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Строка " + y + " не заполненна");

                return false;
            }
        }    
        
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Строка " + y + " заполненна");

        return true;
    }

    public void DeleteAllPointsInRow(int y)     //Удалени ряда, по 1 точке за каждую итерацию цикла
    {
        if (y <= gridMaxHeight && y >= 0)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {               
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    public void DeleteRow()     //Вызывается при удалении строки
    {
        for(int y = 0; y < grid.GetLength(1); y++)
        {
            if (ItRowFull(y))
            {
                itRowDelete = true; 
                UpdateScore();

                DeleteAllPointsInRow(y);
                AllRowsDown(y + 1);

                if (GameMode == SingleGameHelper.GameModeEnum.HardCore)     //Удаление первой каменной линии (HardCore Mode)
                {
                    DeleteAllPointsInRow(0);
                    AllRowsDown(0 + 1);
                }

                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Удалена " + y + " строка");

                y--;

                limitStoneHeight--;
            }
        }          
    }

    public void RowsDown(int y)    //Опускает одну строку "y"
    {
        if (y > 0 && y < gridMaxHeight)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] != null)
                {                   
                    grid[x, y - 1] = grid[x, y];

                    grid[x, y] = null;

                    grid[x, y - 1].position += new Vector3(0, -stepSize, 0);
                }
            }
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Опущена " + y + "строка");
        }
    }
    
    public void AllRowsDown(int y)     //Опускает сторки вниз начиная начиная с строки "y" 
    {
       Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Опуск всех строк начиная с " + y + "строки");

        for (int i = y; i < grid.GetLength(1); i++)
        {          
            RowsDown(i);
        }
    }

    public void UpdateGrid(Tetromino tetromino)     //заполнение массива grid
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Обновление внутренней информации об " + tetromino.name + " в игровом поле");

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetromino.transform)
                    {
                        grid[x, y] = null;

                        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Очищена устаревшая внутренняя информация об " + tetromino.name + "в ячейке"
                            + " Х: " + x
                            + " Y: " + y);
                    }
                }
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Удалена устаревшая внутренняя информация об " + tetromino.name + " в игровом поле");

        foreach (Transform mino in tetromino.transform)
        {
            Vector2 vec = RoundVec2(mino.position);

            if (ToLowerCoordinates(vec).y < gridMaxHeight)
            {               
                grid[(int)ToLowerCoordinates(vec).x, (int)ToLowerCoordinates(vec).y] = mino;

                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Внесенна новая внутренняя информация об " + tetromino.name + " в ячейку"
                     + " Х: " + ((int)vec.x / stepSize)
                     + " Y: " + ((int)vec.y / stepSize));
            }
        }
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Внесенна новая внутренняя информация об " + tetromino.name + " в игровом поле");
    }

    public Transform GetTransformGridPosition(Vector2 pos)
    {
        if(ToLowerCoordinates(pos).y > gridMaxHeight - 1 || ToLowerCoordinates(pos).y < 0)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Отказано в получение внутренней информация из ячейки "
                + " Х: " + (int)pos.x / stepSize
                + " Y: " + (int)pos.y / stepSize
                + " в игровом поле");

            return null;
        }
        else
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Получение внутренней информация из ячейки "
                + " Х: " + (int)pos.x / stepSize
                + " Y: " + (int)pos.y / stepSize
                + " в игровом поле");

            return grid[(int)ToLowerCoordinates(pos).x, (int)ToLowerCoordinates(pos).y];
        }   
    }

    public bool OnTheFrame(Vector2 pos) //Проверка "Находится ли объект (фигура) в пределах поля"
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка находится ли объект с координатами " + pos.ToString() + " в игровом поле");

        if (ToLowerCoordinates(pos).x >= 0 && ToLowerCoordinates(pos).x < gridMaxWidth && ToLowerCoordinates(pos).y >= 0)
        {      
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Объект с координатами " + pos.ToString() + " находится в игровом поле");
            return true;
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Объект с координатами " + pos.ToString() + " НЕ находится в игровом поле");
        return false;
    }

    public Vector2 RoundVec2(Vector2 pos)   //Округление вектора 
    {
        Vector2 vec = new Vector2(Mathf.Round(ToLowerCoordinates(pos).x), Mathf.Round(ToLowerCoordinates(pos).y));
        vec.x = vec.x * stepSize;
        vec.y = vec.y * stepSize;
        return vec;
    }

    public void SpawnNextTetromino()    //Установки следующей фигуры из препромотра на поле, и установка новой фигуры для препросмотра
    {
        if (!GameStart)
        {
            GameStart = true;
            NowTetromino = (GameObject)Instantiate(Resources.Load(prefixesTetrominoName + GetRandomTetromino(), typeof(GameObject)), tetrominoSpawnPosition, Quaternion.identity);
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установленна новая фигура: " + NowTetromino.name);

            PreviewTetromino = (GameObject)Instantiate(Resources.Load(prefixesTetrominoName + GetRandomTetromino(), typeof(GameObject)), tetrominoPreviewSpawnPosition, Quaternion.identity);
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлен новая фигура для предпросмотра: " + PreviewTetromino.name);

            PreviewTetromino.GetComponent<Tetromino>().enabled = false;       
        }
        else
        {
            PreviewTetromino.transform.localPosition = tetrominoSpawnPosition;
            NowTetromino = PreviewTetromino;
            NowTetromino.GetComponent<Tetromino>().enabled = true;
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Фигура из предпросмотра " +  NowTetromino.name + " активирована и перемещенна на место спавна новой фигур");


            PreviewTetromino = (GameObject)Instantiate(Resources.Load(prefixesTetrominoName + GetRandomTetromino(), typeof(GameObject)), tetrominoPreviewSpawnPosition, Quaternion.identity);
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Установлен новая фигура для предпросмотра: " + PreviewTetromino.name);

            PreviewTetromino.GetComponent<Tetromino>().enabled = false;
        }     
    }

    public string GetRandomTetromino()     //Возвращает случайное имя фигуры из массива "TetrominoNameArr"
    {
        string RandTetraName;

        int RandNumbTetra = UnityEngine.Random.Range(0, tetrominoNameArr.Length);

        RandTetraName = tetrominoNameArr[RandNumbTetra];

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Полученна случайная фигура: " + RandTetraName);

        return RandTetraName;
    }

    public bool ItGameOver(Tetromino tetromino)     //Проверка на проигрыш
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на проигрыш ");

        for (int x = 0; x < gridMaxWidth; x++)
        {
            foreach (Transform tetramino in tetromino.transform)
            {
                Vector2 vec = RoundVec2(tetramino.position);
                if (ToLowerCoordinates(vec).y > gridMaxHeight - 1)
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Игра проиграна");

                    return true;
                }
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Игра НЕ проиграна");
        return false;
    }

    public void GameOver()  //Вызывается при проигрыше
    {       
        itGameOverBool = true;

        string text = "Результат: " + score;
        if (score > youBestRecord)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Новый личный рекорд для " + PP.profileNum + " профиля: " + score);
            text += "\n" + "Новый личный рекорд!";
            PlayerPrefs.SetInt(PlayerPreferencesHelper.GetPersonalBestRecordKey(PP.profileNum, GameMode, SGH.GetGameSpeed()), score);
        }
        if (score > bestRecord)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Новый общий рекорд: " + score);

            text += "\n" + "Новый общий рекорд!";
            PlayerPrefs.SetInt(PlayerPreferencesHelper.GetBestRecordKey(GameMode, SGH.GetGameSpeed()), score);
        }

        MBGO.ShowMessageBox(text);
    }

    private void UpdateScore()     //Поднятие счета
    {
        UpScore();
        TxtNowRecord.text = score.ToString();     
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Увеличен счет. Текущее значение: " + score);
    }

    public void InitializedGrid()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = null;
            }
        }  

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Массив игрового поля очищен");
    }

    public Vector2 ToLowerCoordinates(Vector2 vec)  //Переводит реальные координаты в координаты с шаговом в 1 понятный для массивов
    {
        return new Vector2(vec.x / stepSize, vec.y / stepSize);
    }
}
