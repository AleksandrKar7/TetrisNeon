using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGameHelper : MonoBehaviour {   
    public enum GameModeEnum : int
    {
        Classic
        , HardCore
        , LargeNotCreated  //Не реализован
    }

    private SingleGameHelper.GameModeEnum GameMode = 0;
    public SingleGameHelper.GameModeEnum GetGameMode()
    {
        return GameMode;
    }
    public void SetGameMode(SingleGameHelper.GameModeEnum GM)
    {
        GameMode = GM;
    }

    private int GameSpeed = 0;
    public int GetGameSpeed()
    {
        return GameSpeed;
    }
    public void SetGameSpeed(int GS)
    {
        GameSpeed = GS;
    }

}
