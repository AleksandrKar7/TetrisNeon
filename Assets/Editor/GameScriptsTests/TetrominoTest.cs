using System;
using UnityEngine;
using NUnit.Framework;

public class TetrominoTest : MonoBehaviour{

    ////////////////////////////    Методы движения    ////////////////////////////

    [Test]
    public void MoveRight_TetrominoLPositionX5Plus1_X6Returned()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * tet.stepSize, 20.0f * tet.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        NowTetromino.GetComponent<Tetromino>().MoveRight();

        // assert
        Assert.AreEqual(6, NowTetromino.transform.position.x / NowTetromino.GetComponent<Tetromino>().stepSize);
    }

    [Test]
    public void MoveRight_MoveStontLine_NoMove()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 StoneLineSpawnPosition = new Vector2(0f * tet.stepSize, 0f * tet.stepSize);   //Место спавна первой каменной линии (HardCore Mode)
        GameObject NowStoneTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), StoneLineSpawnPosition, Quaternion.identity);

        // act
        NowStoneTetromino.GetComponent<Tetromino>().MoveRight();

        // assert
        Assert.AreEqual(StoneLineSpawnPosition.x, NowStoneTetromino.transform.position.x);
    }

    [Test]
    public void MoveLeft_TetrominoLPositionX5Minus1_X4Returned()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * tet.stepSize, 20.0f * tet.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        NowTetromino.GetComponent<Tetromino>().MoveLeft();

        // assert
        Assert.AreEqual(4, NowTetromino.transform.position.x / NowTetromino.GetComponent<Tetromino>().stepSize);
    }

    [Test]
    public void MoveLeft_MoveStontLine_NoMove()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 StoneLineSpawnPosition = new Vector2(0f * tet.stepSize, 0f * tet.stepSize);   //Место спавна первой каменной линии (HardCore Mode)
        GameObject NowStoneTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), StoneLineSpawnPosition, Quaternion.identity);

        // act
        NowStoneTetromino.GetComponent<Tetromino>().MoveLeft();

        // assert
        Assert.AreEqual(StoneLineSpawnPosition.x, NowStoneTetromino.transform.position.x);
    }

    [Test]
    public void MoveDown_TetrominoLPositionY20Minus1_Y19Returned()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * tet.stepSize, 20.0f * tet.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        NowTetromino.GetComponent<Tetromino>().MoveDown();

        // assert
        Assert.AreEqual(19, NowTetromino.transform.position.y / NowTetromino.GetComponent<Tetromino>().stepSize);
    }

    [Test]
    public void MoveDown_MoveStontLine_NoMove()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 StoneLineSpawnPosition = new Vector2(0f * tet.stepSize, 0f * tet.stepSize);   //Место спавна первой каменной линии (HardCore Mode)
        GameObject NowStoneTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), StoneLineSpawnPosition, Quaternion.identity);

        // act
        NowStoneTetromino.GetComponent<Tetromino>().MoveDown();

        // assert
        Assert.AreEqual(StoneLineSpawnPosition.x, NowStoneTetromino.transform.position.x);
    }

    [Test]
    public void MoveUp_TetrominoLPositionY20Plus1_Y21Returned()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * tet.stepSize, 20.0f * tet.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        NowTetromino.GetComponent<Tetromino>().MoveUp();

        // assert
        Assert.AreEqual(21, NowTetromino.transform.position.y / NowTetromino.GetComponent<Tetromino>().stepSize);
    }

    [Test]
    public void MoveUp_MoveStontLine_NoMove()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 StoneLineSpawnPosition = new Vector2(0f * tet.stepSize, 0f * tet.stepSize);   //Место спавна первой каменной линии (HardCore Mode)
        GameObject NowStoneTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), StoneLineSpawnPosition, Quaternion.identity);

        // act
        NowStoneTetromino.GetComponent<Tetromino>().MoveUp();

        // assert
        Assert.AreEqual(StoneLineSpawnPosition.x, NowStoneTetromino.transform.position.x);
    }

    [Test]
    public void Rotate_RotateStoneLine_NoRotate()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 StoneLineSpawnPosition = new Vector2(0f * tet.stepSize, 0f * tet.stepSize);   //Место спавна первой каменной линии (HardCore Mode)
        GameObject NowStoneTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), StoneLineSpawnPosition, Quaternion.identity);

        // act
        NowStoneTetromino.GetComponent<Tetromino>().Rotate();       

        // assert
        Assert.AreEqual(Quaternion.identity, NowStoneTetromino.transform.rotation);
    }

    [Test]
    public void Rotate_RotateTetrominoLWith4PositionClockwise_270Degrees()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * tet.stepSize, 20.0f * tet.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        NowTetromino.GetComponent<Tetromino>().Rotate();

        // assert
        Assert.AreEqual(270, NowTetromino.transform.rotation.eulerAngles.z);
    }

    [Test]
    public void Rotate_RotateTetrominoDuckWith2PositionClockwise_270Degrees()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * tet.stepSize, 20.0f * tet.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_Duck", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        NowTetromino.GetComponent<Tetromino>().Rotate();

        // assert
        Assert.AreEqual(270, NowTetromino.transform.rotation.eulerAngles.z);
    }

    [Test]
    public void Rotate_RotateTetrominoDuckWith2PositionAnticlockwise_90Degrees()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f, 20.0f);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_Duck", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);
        NowTetromino.GetComponent<Tetromino>().itTurn = true;

        // act
        NowTetromino.GetComponent<Tetromino>().Rotate();

        // assert
        Assert.AreEqual(90, NowTetromino.transform.rotation.eulerAngles.z);
    }

    [Test]
    public void UnRotate_RotateStoneLine_NoRotate()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 StoneLineSpawnPosition = new Vector2(0f * tet.stepSize, 0f * tet.stepSize);   //Место спавна первой каменной линии (HardCore Mode)
        GameObject NowStoneTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), StoneLineSpawnPosition, Quaternion.identity);

        // act
        NowStoneTetromino.GetComponent<Tetromino>().UnRotate();

        // assert
        Assert.AreEqual(Quaternion.identity, NowStoneTetromino.transform.rotation);
    }

    [Test]
    public void UnRotate_RotateTetrominoLWith4PositionAnticlockwise_90Degrees()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * tet.stepSize, 20.0f * tet.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        NowTetromino.GetComponent<Tetromino>().UnRotate();

        // assert
        Assert.AreEqual(90, NowTetromino.transform.rotation.eulerAngles.z);
    }

    [Test]
    public void UnRotate_RotateTetrominoDuckWith2PositionAnticlockwise_270Degrees()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * tet.stepSize, 20.0f * tet.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_Duck", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        NowTetromino.GetComponent<Tetromino>().UnRotate();

        // assert
        Assert.AreEqual(270, NowTetromino.transform.rotation.eulerAngles.z);
    }

    [Test]
    public void UnRotate_RotateTetrominoDuckWith2PositionClockwise_90Degrees()
    {
        // arrange
        Tetromino tet = new Tetromino();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * tet.stepSize, 20.0f * tet.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_Duck", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);
        NowTetromino.GetComponent<Tetromino>().itTurn = true;

        // act
        NowTetromino.GetComponent<Tetromino>().UnRotate();

        // assert
        Assert.AreEqual(90, NowTetromino.transform.rotation.eulerAngles.z);
    }

    // arrange

    // act

    // assert
}
