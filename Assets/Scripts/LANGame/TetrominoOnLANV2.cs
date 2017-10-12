using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TetrominoOnLANV2 : NetworkBehaviour
{
    private GameOnLANV2 game;
    private float stepSize;
    float timeLastFall = 0;
    float timeLastMoveHorizontal = 0;
    private int fastDropCoefficient = 4;    //Коефициент ускорения падения при зажатой клавише
    private double speedFastMoveHorizontal = 0.25;  //Коефициент ускорения горизонтального движения при зажатой клавише

    public int numberPossibleTurns = 4;     //количество возможных позиций для объекта (Позиции меняются при поворотах) 

    public bool itTurn = false;     //используется для фигур с 2 позициями 
    public bool itStone = false;    //Является ли эта линия каменной 

    public string IdPlayer = null;    //используется для разделения собственности

    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<GameOnLANV2>();
        stepSize = game.stepSize;

        timeLastFall = Time.time;
        timeLastMoveHorizontal = Time.time;
        game.gameSpeed = (11 - game.gameSpeed) / 10;
        if (game.gameSpeed == 0)
        {
            game.gameSpeed = 10;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (game.itGamePouse == false)
        {
            CheckInputKey();
            UpdateGridStoneLine();
        }
    }

    private void UpdateGridStoneLine()
    {
        if (itStone == true 
            && enabled == true)
        {
            enabled = false;
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Update");
            game.UpdateGrid(this, this.IdPlayer);
        }
    }

    private void CheckInputKey()    //проверка входящих сигналов  
    {
        if (Time.time - timeLastFall >= game.gameSpeed)
        {         
            MoveDown();

            timeLastFall = Time.time;
        }
    }

    public void Rotate()   //метод поворота объекта (фигуры)
    {
        if (itStone != true)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на поворот для объекта " + this.name
                 + " игрока с id: " + this.IdPlayer);

            if (numberPossibleTurns == 2
                && itTurn == false)
            {
                transform.Rotate(0, 0, -90);
                itTurn = true;
                if (CheckValidPosition())
                {
                    game.UpdateGrid(this, this.IdPlayer);
                }
                else
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поворот для объекта " + this.name + " недопустим! "
                        + " игрока с id: " + this.IdPlayer
                        + "Производится обратный поворот");

                    transform.Rotate(0, 0, 90);
                    itTurn = false;
                }
            }
            else if (numberPossibleTurns == 2 && itTurn == true)
            {
                transform.Rotate(0, 0, 90);
                itTurn = false;
                if (CheckValidPosition())
                {
                    game.UpdateGrid(this, this.IdPlayer);
                }
                else
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поворот для объекта " + this.name + " недопустим! "
                        + " игрока с id: " + this.IdPlayer
                        + "Производится обратный поворот");

                    transform.Rotate(0, 0, -90);
                    itTurn = true;
                }
            }

            if (numberPossibleTurns == 4)
            {
                transform.Rotate(0, 0, -90);

                if (CheckValidPosition())
                {
                    game.UpdateGrid(this, this.IdPlayer);
                }
                else
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поворот для объекта " + this.name + " недопустим! "
                        + " игрока с id: " + this.IdPlayer
                        + "Производится обратный поворот");
                    transform.Rotate(0, 0, 90);
                }
            }

            game.SynchronizedTetromino(this.IdPlayer, this.transform.position, this.transform.rotation);
        }
    }

    public void MoveRight()    //метод движения объекта (фигуры) вправо
    {
        if (itStone != true)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на движение влево для объекта " + this.name
                 + " игрока с id: " + this.IdPlayer);

            transform.position += new Vector3(stepSize, 0, 0);

            if (CheckValidPosition())
            {
                game.UpdateGrid(this, this.IdPlayer);
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поворот для объекта " + this.name + " недопустим! "
                     + " игрока с id: " + this.IdPlayer
                     + "Производится обратный поворот");

                transform.position += new Vector3(-stepSize, 0, 0);
            }

            game.SynchronizedTetromino(this.IdPlayer, this.transform.position, this.transform.rotation);
        }
    }

    public void MoveLeft()     //метод движения объекта (фигуры) влево
    {
        if (itStone != true)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на движение влево для объекта " + this.name
                 + " игрока с id: " + this.IdPlayer);

            transform.position += new Vector3(-stepSize, 0, 0);
            if (CheckValidPosition())
            {
                game.UpdateGrid(this, this.IdPlayer);
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение влево для объекта " + this.name + " недопустимо! "
                   + "Производится обратное движение вправо");

                transform.position += new Vector3(stepSize, 0, 0);
            }
            game.SynchronizedTetromino(this.IdPlayer, this.transform.position, this.transform.rotation);
        }
    }

    public void MoveDown()     //метод движения объекта (фигуры) вниз
    {
        if (itStone != true)
        {
            transform.position += new Vector3(0, -stepSize, 0);
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на движение вниз для объекта " + this.name
                + " игрока с id: " + this.IdPlayer);

            if (CheckValidPosition())
            {
                game.SynchronizedTetromino(this.IdPlayer, this.transform.position, this.transform.rotation);
                game.UpdateGrid(this, this.IdPlayer);        
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение вниз для объекта " + this.name 
                    + " игрока с id: " + this.IdPlayer + " недопустимо! "
                    + "Производится обратное движение вверх и остановка фигуры");

                transform.position += new Vector3(0, stepSize, 0);

                Debug.Log(this);
                game.DeleteRow(this.IdPlayer);

                enabled = false;

                game.SynchronizedTetromino(this.IdPlayer, this.transform.position, this.transform.rotation);

                game.itRowDelete = false;

                game.SpawnNextTetromino(this.IdPlayer);

                if (game.ItGameOver(this))
                {
                    game.GameOver(this.IdPlayer);
                }
            }
        }

    }

    private bool CheckValidPosition()   //проверка на разрешонную позицию
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Началоп проверки на валидность позиции для объекта " + this.name
            + " игрока с id: " + this.IdPlayer);

        foreach (Transform tetramino in transform)
        {
            Vector2 pos = game.RoundVec2(tetramino.position);

            if (game.OnTheFrame(pos, this.IdPlayer) == false)
            {
                return false;
            }

            if (game.GetTransformGridPosition(pos, this.IdPlayer) != null
            && game.GetTransformGridPosition(pos, this.IdPlayer).parent != transform)
            {
                return false;
            }
        }

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на валидность позиции для объекта " + this.name 
            + " игрока с id: " + this.IdPlayer
            + " пройдена успешно");

        return true;
    }

    public Vector2 RoundVec2(Vector2 pos)   //округление вектора
    {
        Vector2 vec = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        return vec;
    }
}
