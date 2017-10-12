using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerHelper : MonoBehaviour {

    //Хранилище всех имен и индексов активных сцен
    public enum SceneManagerEnum   
    {
        MainMenu 
        , ProfileSelect
        , ProfileEdit
        , VolumeSetting
        , SpeedRegulator
        , SingleGame
        , LANLobby
        , LANSearch
        , LANGame
    }
}
