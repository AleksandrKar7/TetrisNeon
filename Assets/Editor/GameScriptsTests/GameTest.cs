using UnityEngine;
using NUnit.Framework;

public class GameTest : MonoBehaviour
{

    [Test]
    public void OnTheFrame_X100Y15_False()
    {
        // arrange
        Game game = new Game();
        Vector2 pos = new Vector2(100 * game.stepSize, 15 * game.stepSize);

        // act
        bool res = game.OnTheFrame(pos);

        // assert
        Assert.AreEqual(false, res);
    }

    [Test]
    public void OnTheFrame_X5YMinus10_False()
    {
        // arrange
        Game game = new Game();
        Vector2 pos = new Vector2(5 * game.stepSize, -10 * game.stepSize);

        // act
        bool res = game.OnTheFrame(pos);

        // assert
        Assert.AreEqual(false, res);
    }

    [Test]
    public void OnTheFrame_X100YMinus10_False()
    {
        // arrange
        Game game = new Game();
        Vector2 pos = new Vector2(100 * game.stepSize, -10 * game.stepSize);       

        // act
        bool res = game.OnTheFrame(pos);

        // assert
        Assert.AreEqual(false, res);
    }

    [Test]
    public void OnTheFrame_X5Y10_True()
    {
        // arrange
        Game game = new Game();
        Vector2 pos = new Vector2(5 * game.stepSize, 10 * game.stepSize);

        // act
        bool res = game.OnTheFrame(pos);

        // assert
        Assert.AreEqual(true, res);
    }

    [Test]
    public void RoundVec2_X5dot35Y10dot45_X5Y10()
    {
        // arrange
        Game game = new Game();
        Vector2 pos = new Vector2(5.35f * game.stepSize, 10.45f * game.stepSize);

        // act
        Vector2 res = game.RoundVec2(pos);

        // assert
        Assert.AreEqual(new Vector2(5 * game.stepSize, 10 * game.stepSize), res);
    }

    [Test]
    public void RoundVec2_X5dot85Y10dot70_X6Y11()
    {
        // arrange
        Game game = new Game();
        Vector2 pos = new Vector2(5.85f * game.stepSize, 10.70f * game.stepSize);

        // act
        Vector2 res = game.RoundVec2(pos);

        // assert
        Assert.AreEqual(new Vector2(6 * game.stepSize, 11 * game.stepSize), res);
    }

    [Test]
    public void ItGameOver_X5Y10_False()
    {
        // arrange
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * game.stepSize , 10.0f * game.stepSize );  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        bool res = game.ItGameOver(NowTetromino.GetComponent<Tetromino>());

        // assert
        Assert.AreEqual(false, res);
    }

    [Test]
    public void ItGameOver_X5Y21_True()
    {
        // arrange
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(5.0f * game.stepSize , 21.0f * game.stepSize );  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        // act
        bool res = game.ItGameOver(NowTetromino.GetComponent<Tetromino>());

        // assert
        Assert.AreEqual(true, res);
    }

    [Test]
    public void SpawnStoneLine_SpawnStoneLine_StoneLineSpawned()
    {
        // arrange
        Game game = new Game();
        Vector2 StoneLineSpawnPosition = new Vector2(0f * game.stepSize , 0f * game.stepSize );   //Место спавна первой каменной линии (HardCore Mode)
        GameObject stoneLine = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), StoneLineSpawnPosition, Quaternion.identity);

        // act
        game.SpawnStoneLine();

        // assert
        Assert.AreEqual(stoneLine.name, game.stoneLine.name);
    }

    [Test]
    public void SpawnStoneLine_SpawnFirstStoneLine_StoneLinePositionX0Y0()
    {
        // arrange
        Game game = new Game();
        Vector3 ExpectedStoneLinePosition = new Vector3(0f * game.stepSize, 0f * game.stepSize, 0f * game.stepSize);   //Место спавна первой каменной линии (HardCore Mode)

        // act
        game.SpawnStoneLine();

        // assert
        Assert.AreEqual(ExpectedStoneLinePosition, game.stoneLine.transform.position);
    }

    [Test]
    public void UpdateGrid_WritingTetrominoLIntoToArray_X0Y0AndX1Y0AndX2Y0AndX2Y1IsNotNull()
    {
        // arrange 
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(1.0f * game.stepSize, 0.0f * game.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

        foreach (Transform mino in NowTetromino.transform)
        {
            //Vector2 vec = new Vector2(Mathf.Round(mino.transform.position.x), Mathf.Round(mino.transform.position.y));
            ExpectedGrid[(int)mino.position.x / game.stepSize, (int)mino.position.y / game.stepSize] = mino;
        }

        game.InitializedGrid();

        // act
        game.UpdateGrid(NowTetromino.GetComponent<Tetromino>());

        // assert    
        Assert.AreEqual(ExpectedGrid, game.GetGrid());
    }

    [Test]
    public void UpAllRows_AllY0IsFilledInArray_AllY1IsFilledAndAllY0IsNullInArray()
    {
        // arrange
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(0.0f * game.stepSize, 1.0f * game.stepSize);  //Место спавна фигуры
        GameObject stoneLine = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

        foreach (Transform mino in stoneLine.transform)
        {
            Vector2 vec = mino.position;
            ExpectedGrid[(int)vec.x / game.stepSize, (int)vec.y / game.stepSize] = mino;
        }

        game.InitializedGrid();
        game.SpawnStoneLine();
        game.UpdateGrid(game.stoneLine.GetComponent<Tetromino>());

        // act
        game.UpAllRows();

        // assert
        Assert.AreEqual(ExpectedGrid, game.GetGrid());
    }

    [Test]
    public void InitializedGrid_X0Y0AndX1Y0AndX2Y0AndX2Y1IsFilled_AllCellIsNullInArray()
    {
        // arrange
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(1.0f * game.stepSize, 0.0f * game.stepSize);  //Место спавна фигуры
        GameObject NowTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

        game.InitializedGrid();
        game.UpdateGrid(NowTetromino.GetComponent<Tetromino>());
        // act
        game.InitializedGrid();

        // assert    
        Assert.AreEqual(ExpectedGrid, game.GetGrid());
    }

    [Test]
    public void ItRowFull_AllY0IsFilledAndCheckedY0_True()
    {
        // arrange  
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(0.0f * game.stepSize, 0.0f * game.stepSize);  //Место спавна фигуры
        GameObject stoneLine = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        bool ExpectedBool = true;

        game.InitializedGrid();
        game.UpdateGrid(stoneLine.GetComponent<Tetromino>());

        // act
        bool res = game.ItRowFull(0);

        // assert
        Assert.AreEqual(ExpectedBool, res);
    }

    [Test]
    public void ItRowFull_Y0IsNotFilledAndCheckedY0_False()
    {
        // arrange  
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(1.0f * game.stepSize, 0.0f * game.stepSize);  //Место спавна фигуры
        GameObject tetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        bool ExpectedBool = false;

        game.InitializedGrid();
        game.UpdateGrid(tetromino.GetComponent<Tetromino>());

        // act
        bool res = game.ItRowFull(0);

        // assert
        Assert.AreEqual(ExpectedBool, res);
    }

    [Test]
    public void ItRowFull_AttemptToGetIndexOutExceptionY99_False()
    {
        // arrange 
        Game game = new Game();

        // act
        bool bol = game.ItRowFull(99);

        // assert
        Assert.AreEqual(bol, false);
    }

    [Test]
    public void ItRowFull_AttemptToGetIndexOutExceptionMinusY99_False()
    {
        // arrange 
        Game game = new Game();

        // act
        bool bol = game.ItRowFull(-99);

        // assert
        Assert.AreEqual(bol, false);
    }

    //[Test]
    //public void DeleteAllPointsInRow_AllY0AndY1IsFilledDeleteY0_AllY0IsNullAndY1IsFilled()
    //{
    //    // arrange 
    //    Game game = new Game();
    //    Vector2 TetrominoSpawnPosition = new Vector2(0.0f * game.StepSize, 1.0f * game.StepSize);  //Место спавна фигуры
    //    GameObject stoneLine = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

    //    game.InitializedGrid();

    //    int gridMaxHeight = 20;  //Высота игрового поля    
    //    int gridMaxWidth = 10;   //Ширина игрового поля   
    //    Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

    //    foreach (Transform mino in stoneLine.transform)
    //    {
    //        Vector2 vec = mino.position;
    //        ExpectedGrid[(int)vec.x, (int)vec.y] = mino;
    //    }

    //    game.UpdateGrid(stoneLine.GetComponent<Tetromino>());
    //    stoneLine.GetComponent<Tetromino>().transform.position = new Vector2(0.0f * game.StepSize, 0.0f * game.StepSize);
    //    game.UpdateGrid(stoneLine.GetComponent<Tetromino>());
    //    //game.SpawnStoneLine();
    //    //game.UpdateGrid(game.stoneLine.GetComponent<Tetromino>());
    //    //game.SpawnStoneLine();
    //    //game.UpdateGrid(game.stoneLine.GetComponent<Tetromino>());

    //    // act
    //    //game.DeleteAllPointsInRow(0);
        
    //    // assert
    //    Assert.AreEqual(ExpectedGrid, game.GetGrid());
    //}

    [Test]
    public void DeleteAllPointsInRow_AttemptToGetIndexOutExceptionY99_NoneException()
    {
        // arrange 
        Game game = new Game();

        game.InitializedGrid();

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

        // act
        game.DeleteAllPointsInRow(99);
        
        // assert
        Assert.AreEqual(ExpectedGrid, game.GetGrid());
    }

    [Test]
    public void DeleteAllPointsInRow_AttemptToGetIndexOutExceptionMinusY99_NoneException()
    {
        // arrange 
        Game game = new Game();

        game.InitializedGrid();

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

        // act
        game.DeleteAllPointsInRow(-99);

        // assert
        Assert.AreEqual(ExpectedGrid, game.GetGrid());
    }

    [Test]
    public void RowsDown_AllY1IsFilledRowsDownY1_AllY0IsFilledAndAllY1IsNull()
    {
        // arrange 
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(0.0f * game.stepSize, 0.0f * game.stepSize);  //Место спавна фигуры
        GameObject stoneLine = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        game.InitializedGrid();

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

        foreach (Transform mino in stoneLine.transform)
        {
            Vector2 vec = mino.position;
            ExpectedGrid[(int)vec.x / game.stepSize, (int)vec.y / game.stepSize] = mino;
        }


        game.SpawnStoneLine();
        game.UpdateGrid(game.stoneLine.GetComponent<Tetromino>());
        //stoneLine.GetComponent<Tetromino>().transform.position = new Vector2(0.0f * game.StepSize, 0.0f * game.StepSize);
        //game.UpdateGrid(stoneLine.GetComponent<Tetromino>());

        // act
        game.RowsDown(1);

        // assert
        Assert.AreEqual(ExpectedGrid, game.GetGrid());
    }

    [Test]
    public void RowsDown_AttemptToGetIndexOutExceptionAllY1IsFilledRowsDownY99_NoneException()
    {
        // arrange 
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(0.0f * game.stepSize, 0.0f * game.stepSize);  //Место спавна фигуры
        GameObject stoneLine = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        game.InitializedGrid();

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

        foreach (Transform mino in stoneLine.transform)
        {
            Vector2 vec = mino.position;
            ExpectedGrid[(int)vec.x / game.stepSize, (int)vec.y / game.stepSize] = mino;
        }


        game.SpawnStoneLine();
        game.UpdateGrid(game.stoneLine.GetComponent<Tetromino>());
        //stoneLine.GetComponent<Tetromino>().transform.position = new Vector2(0.0f * game.StepSize, 0.0f * game.StepSize);
        //game.UpdateGrid(stoneLine.GetComponent<Tetromino>());

        // act
        game.RowsDown(99);

        // assert
        Assert.AreEqual(ExpectedGrid, game.GetGrid());
    }

    [Test]
    public void RowsDown_AttemptToGetIndexOutExceptionAllY1IsFilledRowsDownMinusY99_NoneException()
    {
        // arrange 
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(0.0f * game.stepSize, 0.0f * game.stepSize);  //Место спавна фигуры
        GameObject stoneLine = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        game.InitializedGrid();

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

        foreach (Transform mino in stoneLine.transform)
        {
            Vector2 vec = mino.position;
            ExpectedGrid[(int)vec.x / game.stepSize, (int)vec.y / game.stepSize] = mino;
        }

        game.SpawnStoneLine();
        game.UpdateGrid(game.stoneLine.GetComponent<Tetromino>());

        // act
        game.RowsDown(-99);

        // assert
        Assert.AreEqual(ExpectedGrid, game.GetGrid());
    }

    [Test]
    public void AllRowsDown_AllY1AndY3IsFilled_AllY0AndY2IsFilledAndY1AndY2IsNull()
    {
        // arrange 
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(0.0f * game.stepSize, 0.0f * game.stepSize);  //Место спавна фигуры
        GameObject stoneLine = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        game.InitializedGrid();

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] ExpectedGrid = new Transform[gridMaxWidth, gridMaxHeight];

        foreach (Transform mino in stoneLine.transform)
        {
            Vector2 vec = mino.position;
            ExpectedGrid[(int)vec.x / game.stepSize, (int)vec.y / game.stepSize] = mino;
        }

        stoneLine.GetComponent<Tetromino>().transform.position = new Vector3(0.0f * game.stepSize, 2.0f * game.stepSize);
        foreach (Transform mino in stoneLine.transform)
        {
            Vector2 vec = mino.position;
            ExpectedGrid[(int)vec.x / game.stepSize, (int)vec.y / game.stepSize] = mino;
        }

        stoneLine.GetComponent<Tetromino>().transform.position = new Vector3(0.0f * game.stepSize, 1.0f * game.stepSize);
        game.UpdateGrid(stoneLine.GetComponent<Tetromino>());

        GameObject stoneLine2 = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/StoneLine", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);
        stoneLine2.GetComponent<Tetromino>().transform.position = new Vector3(0.0f * game.stepSize, 3.0f * game.stepSize);
        game.UpdateGrid(stoneLine2.GetComponent<Tetromino>());

        // act
        game.AllRowsDown(1);

        // assert
        Assert.AreEqual(ExpectedGrid, game.GetGrid());
    }

    [Test]
    public void GetTransformGridPosition_X0Y0AndX1Y0AndX2Y0AndX2Y1IsFilledAttemptToGetX1Y0_ReturnValueIsNull()
    {
        // arrange
        Game game = new Game();
        Vector2 TetrominoSpawnPosition = new Vector2(1.0f * game.stepSize , 0.0f * game.stepSize );  //Место спавна фигуры
        GameObject tetromino = (GameObject)Instantiate(Resources.Load("Prefabs/SingleGame/Tetromino_L", typeof(GameObject)), TetrominoSpawnPosition, Quaternion.identity);

        game.InitializedGrid();

        int gridMaxHeight = 20;  //Высота игрового поля    
        int gridMaxWidth = 10;   //Ширина игрового поля   
        Transform[,] Grid = new Transform[gridMaxWidth, gridMaxHeight];

        foreach (Transform mino in tetromino.transform)
        {
            Vector2 vec = mino.position;
            Grid[(int)vec.x / game.stepSize, (int)vec.y / game.stepSize] = mino;
        }

        game.UpdateGrid(tetromino.GetComponent<Tetromino>());

        Transform ExtentedTransform = Grid[1, 0];
        // act  

        Transform res = game.GetTransformGridPosition(new Vector2(1 , 0));

        // assert
        Assert.AreEqual(ExtentedTransform, res);
    }

    [Test]
    public void GetTransformGridPosition_AttemptToGetIndexOutExceptionX99Y99_NoneException()
    {
        // arrange
        Game game = new Game();

        // act  
        Transform res = game.GetTransformGridPosition(new Vector2(99 * game.stepSize, 99 * game.stepSize));

        // assert
        Assert.AreEqual(null, res);
    }

    [Test]
    public void GetTransformGridPosition_AttemptToGetIndexOutExceptionXMinus99YMinus99_NoneExceptionGetNull()
    {
        // arrange
        Game game = new Game();

        // act  
        Transform res = game.GetTransformGridPosition(new Vector2(-99 * game.stepSize, -99 * game.stepSize));

        // assert
        Assert.AreEqual(null, res);
    }

    [Test]
    public void GetRandomTetromino_GetRandomNameTetromino_NotNull()
    {
        // arrange
        Game game = new Game();
        
        // act
        string res = game.GetRandomTetromino();

        // assert
        Assert.AreNotEqual(res, null);
    }

    [Test]
    public void SpawnNextTetromino_SpawnedFirstNowAndPreviewTetromino_NowTetrominoIsNotNull()
    {
        // arrange
        Game game = new Game();

        // act  
        game.SpawnNextTetromino();

        // assert
        Assert.AreNotEqual(game.nowTetromino, null);
    }

    [Test]
    public void SpawnNextTetromino_SpawnedFirstNowAndPreviewTetromino_PreviewTetrominoIsNotNull()
    {
        // arrange
        Game game = new Game();

        // act  
        game.SpawnNextTetromino();

        // assert
        Assert.AreNotEqual(game.previewTetromino, null);
    }

    [Test]
    public void SpawnNextTetromino_SpawnedNewNowTetromino_PreviewTetrominoEqualOldNowTetromino()
    {
        // arrange
        Game game = new Game();

        // act  
        game.SpawnNextTetromino();
        GameObject ExtendetNowTetromino = game.nowTetromino;
        game.SpawnNextTetromino();

        // assert
        Assert.AreNotEqual(ExtendetNowTetromino, game.previewTetromino);
    }

    [Test]
    public void ToLowerCoordinates_X500Y125Step25_X20Y5()
    {
        // arrange
        Game game = new Game();
        Vector2 vec = new Vector2(500, 125);
        Vector2 expected = new Vector2(20.0f, 5.0f);
        game.stepSize = 25;

        // act  
        Vector2 res = game.ToLowerCoordinates(vec);

        // assert
        Assert.AreEqual(expected, res);
    }
    // arrange

    // act

    // assert
}
