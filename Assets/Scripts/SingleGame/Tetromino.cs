using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {
    private Game game;
    public int stepSize = 25;
    float timeLastFall = 0;
    float timeLastMoveHorizontal = 0;
    private double gameSpeed = 1; //скорость падения
    private int fastDropCoefficient = 4; //Коефициент ускорения падения при зажатой клавише
    private double speedFastMoveHorizontal = 0.25; //Коефициент ускорения горизонтального движения при зажатой клавише

    public int numberPossibleTurns = 4;  //количество возможных позиций для объекта (Позиции меняются при поворотах) 

    public bool itTurn = false; //используется для фигур с 2 позициями 
    public bool itStone = false; //Является ли эта линия каменной 

    // Use this for initialization
    void Start () {
        game = FindObjectOfType<Game>();
        timeLastFall = Time.time;
        timeLastMoveHorizontal = Time.time;
        gameSpeed = game.GetGameSpeed();
        stepSize = game.stepSize;      
    }
 
    // Update is called once per frame
    void Update () {
        if (game.GetItGamePouse() == false && game.GetItGameOver() == false)
        {
            CheckInputKey();
            UpdateGridStoneLine();
        }
    }

    private void UpdateGridStoneLine()
    {
        if (itStone == true)
        {
            enabled = false;
            game.UpdateGrid(this);
        }
    }

    private void CheckInputKey()    //проверка входящих сигналов  
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на движение вправо для объекта " + this.name);

            MoveRight();

            if (CheckValidPosition())
            {
                game.UpdateGrid(this);
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение вправо для объекта " + this.name + " недопустимо! "
                    + "Производится обратное движение влево");

                MoveLeft();
            }

            timeLastMoveHorizontal = Time.time;
        }

        if ((double)Time.time - timeLastMoveHorizontal >= speedFastMoveHorizontal && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на движение вправо для объекта " + this.name);

            MoveRight();

            if (CheckValidPosition())
            {
                game.UpdateGrid(this);
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение вправо для объекта " + this.name + " недопустимо! "
                    + "Производится обратное движение влево");

                MoveLeft();
            }

            timeLastMoveHorizontal = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на движение влево для объекта " + this.name);

            MoveLeft();

            if (CheckValidPosition())
            {
                game.UpdateGrid(this);
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение влево для объекта " + this.name + " недопустимо! "
                    + "Производится обратное движение вправо");

                MoveRight();
            }

            timeLastMoveHorizontal = Time.time;
        }

        if ((double)Time.time - timeLastMoveHorizontal >= speedFastMoveHorizontal && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на движение влево для объекта " + this.name);

            MoveLeft();

            if (CheckValidPosition())
            {
                game.UpdateGrid(this);
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение влево для объекта " + this.name + " недопустимо! "
                    + "Производится обратное движение вправо");

                MoveRight();
            }

            timeLastMoveHorizontal = Time.time;
        }


        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на поворот для объекта " + this.name);

            Rotate();

            if (CheckValidPosition())
            {
                game.UpdateGrid(this);
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поворот для объекта " + this.name + " недопустим! "
                    + "Производится обратный поворот");

                UnRotate();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Time.time - timeLastFall >= gameSpeed)
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на движение вниз для объекта " + this.name);

            MoveDown();

            if (CheckValidPosition())
            {
                game.UpdateGrid(this);
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение вниз для объекта " + this.name + " недопустимо! "
                    + "Производится обратное движение вверх и остановка фигуры");

                MoveUp();
                EndMoveTetromino();
            }

            timeLastFall = Time.time;         
        }

        if ((double)Time.time - timeLastFall >= gameSpeed / fastDropCoefficient && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на движение вниз для объекта " + this.name);

            MoveDown();

            if (CheckValidPosition())
            {
                game.UpdateGrid(this);
            }
            else
            {
                Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение вниз для объекта " + this.name + " недопустимо! "
                    + "Производится обратное движение вверх и остановка фигуры");

                MoveUp();
                EndMoveTetromino();
            }

            timeLastFall = Time.time;
        }

        int MaxStepLimiter = 20;

        if ((Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.Space)) 
            || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space)))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на мгновенное движение влево для объекта " + this.name);

            int i = 0;
            do
            {             
                MoveLeft();
                if (CheckValidPosition())
                {
                    game.UpdateGrid(this);
                }
                else
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение влево для объекта " + this.name + " недопустимо! "
                        + "Производится обратное движение вправо");

                    MoveRight();
                    break;
                }
                i++;
            } while (i <= MaxStepLimiter);
        }

        if ((Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.Space))
            || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space)))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на мгновенное движение вправо для объекта " + this.name);

            int i = 0;
            do
            {
                MoveRight();
                if (CheckValidPosition())
                {
                    game.UpdateGrid(this);
                }
                else
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение вправо для объекта " + this.name + " недопустимо! "
                        + "Производится обратное движение влево");

                    MoveLeft();
                    break;
                }
            } while (i <= MaxStepLimiter);
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.Space))
            || (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.Space)))
        {
            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Принят сигнал на мгновенное движение вниз для объекта " + this.name);

            int i = 0;
            do
            {
                MoveDown();
                if (CheckValidPosition())
                {
                    game.UpdateGrid(this);
                }
                else
                {
                    Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение вниз для объекта " + this.name + " недопустимо! "
                        + "Производится обратное движение вверх и остановка фигуры");

                    MoveUp();
                    EndMoveTetromino();
                    break;
                }
            } while (i <= MaxStepLimiter);
        }
    }

    public void MoveUp()
    {
        if (itStone != true)
        {
            Vector3 vec3 = transform.position;

            transform.position += new Vector3(0, stepSize, 0);

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение объекта " + this.name + " вверх: "
                + "Начальные координаты: " + vec3.ToString() + "; "
                + "Конечные коорлинаты: " + this.transform.position.ToString() + "; ");
        }
    }

    public void EndMoveTetromino()
    {
        game.DeleteRow();

        enabled = false;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Объект " + this.name + " закончил свое движение. "
            + "Конечные координаты: " + this.transform.position.ToString());

        if (!game.GetItRowDelete() && game.GetGameMode() == SingleGameHelper.GameModeEnum.HardCore)
        {
            game.SpawnStoneLine();
        }

        game.SetItRowDelete(false);

        game.SpawnNextTetromino();

        if (game.ItGameOver(this))
        {
            game.GameOver();
        }
    }

    public void UnRotate()
    {
        if (itStone != true)
        {
            Quaternion qua = transform.rotation;

            if (numberPossibleTurns == 2 && itTurn == true)
            {
                transform.Rotate(0, 0, 90);
                itTurn = false;
                return;
            }
            else if (numberPossibleTurns == 2 && itTurn == false)
            {
                transform.Rotate(0, 0, -90);
                itTurn = true;
                return;
            }

            if (numberPossibleTurns == 4)
            {
                transform.Rotate(0, 0, 90);
            }

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Оратный поворот объекта " + this.name + " : "
                + "Начальные угол: " + qua.ToString() + "; "
                + "Конечные угол: " + this.transform.rotation.ToString() + "; ");
        }
    }

    public void Rotate() //метод поворота объекта (фигуры)
    {
        if (itStone != true)
        {
            Quaternion qua = transform.rotation;

            if (numberPossibleTurns == 2 && itTurn == false)
            {
                transform.Rotate(0, 0, -90);
                itTurn = true;
                return;
            }
            else if (numberPossibleTurns == 2 && itTurn == true)
            {
                transform.Rotate(0, 0, 90);
                itTurn = false;
                return;
            }

            if (numberPossibleTurns == 4)
            {
                transform.Rotate(0, 0, -90);
            }

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Поворот объекта " + this.name + " : "
               + "Начальные угол: " + qua.ToString() + "; "
               + "Конечные угол: " + this.transform.rotation.ToString() + "; ");
        }
    }

    public void MoveRight()    //метод движения объекта (фигуры) вправо
    {
        if (itStone != true)
        {
            Vector3 vec3 = transform.position;

            transform.position += new Vector3(stepSize, 0, 0);

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение объекта " + this.name + " вправо: "
                + "Начальные координаты: " + vec3.ToString() + "; "
                + "Конечные коорлинаты: " + this.transform.position.ToString() + "; ");
        }
    }

    public void MoveLeft()     //метод движения объекта (фигуры) влево
    {
        if (itStone != true)
        {
            Vector3 vec3 = transform.position;
     
            transform.position += new Vector3(-stepSize, 0, 0);

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение объекта " + this.name + " влево: "
                + "Начальные координаты: " + vec3.ToString() + "; "
                + "Конечные коорлинаты: " + this.transform.position.ToString() + "; ");
        }
    }

    public void MoveDown()     //метод движения объекта (фигуры) вниз
    {
        if (itStone != true)
        {
            Vector3 vec3 = transform.position;

            transform.position += new Vector3(0, -stepSize, 0);

            Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Движение объекта " + this.name + " вниз: "
               + "Начальные координаты: " + vec3.ToString() + "; "
               + "Конечные коорлинаты: " + this.transform.position.ToString() + "; ");
        }        
    }

    private bool CheckValidPosition()   //проверка на разрешенную позицию
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Началоп проверки на валидность позиции для объекта " + this.name);

        foreach (Transform tetramino in transform)
        {
            Vector2 pos = game.RoundVec2(tetramino.position);

            if (game.OnTheFrame(pos) == false)
            {
                return false;
            }

            if (game.GetTransformGridPosition(pos) != null
            && game.GetTransformGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Проверка на валидность позиции для объекта " + this.name + " пройдена успешно");

        return true;
    }

    public Vector2 RoundVec2(Vector2 pos)   //округление вектора
    {
        Vector2 vec = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Округление координат для фигуры " + this.name + " : "
            + "До: " + pos.ToString() + "; "
            + "После: " + vec.ToString() + "; ");

        return vec;
    }
   
}
