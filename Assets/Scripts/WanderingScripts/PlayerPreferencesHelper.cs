using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferencesHelper : MonoBehaviour {

    enum PlayerPreferencesEnum
    {

    }

    public enum VolumePrefEnum
    {
        MusicVolumePercentage   //Ключ для получения целочисленного значения громкости музыки в процентах;    
        , MusicMuteBool  //Ключ для получения целочисленного значения об отключнении музыки (1-да, 0-нет);              
        , SoundVolumePercentage  //Ключ для получения целочисленного значения громкости звука в процентах;    
        , SoundMuteBool  //Ключ для получения целочисленного значения об отключнении звука (1-да, 0-нет);
    }

    public enum ProfilePrefEnum
    {
        CurrentProfile  //Ключ для получения целочисленного значения полседнего выбраного профиля (0, 1, 2);
        , ExistenceProfile  // Ключ для получения целочисленного значения об существовании определенного профиля (1-да, 0-нет) (используется вместе с префиксом профиля);
        , NickName  // Ключ для получения строки с именем профиля (используется вместе с префиксом профиля);
        , Avatar  // Ключ для получения строки с названием аватара (используется вместе с префиксом пути к папке);
        , RecordClassic  // Ключ хранящий информацию существует ли профиль (используется вместе с префиксом профиля);
        , RecordHardCore  // Ключ хранящий информацию существует ли профиль (используется вместе с префиксом профиля).
    }

    public static string GetPrefixProfileKey(int num)  //Генерирует и возвращает префикс профиля из его номера
    {
        string prefix = "Profile" + num + "_";

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Создан префикс ключа: " + prefix);

        return prefix;
    }

    public static string GetPrefixAvatarPath()  //Возвращает абсолютный путь к папке с аватарками
    {
        string prefixPath = Application.dataPath + @"/Resources/Profile/";

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Создан префикс абсолютный путь к папке с аватарками: " + prefixPath);

        return prefixPath;
    }

    //Генерирует ключ для получения личного рекорда, рекорда для определенного профиля, в определленном режиме, с определенной скоростью
    public static string GetPersonalBestRecordKey(int ProfileNum, SingleGameHelper.GameModeEnum GameMode, int GameSpeed) 
    {
        string Key = "";
        Key += GetPrefixProfileKey(ProfileNum);
        Key += "BestRecord";
        Key += GameMode.ToString();
        Key += GameSpeed;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Создан префикс ключ для получения личного рекорда: " + Key);

        return Key;  //Пример значения выглядит так (Profile0_BestRecordClassic9)
    }

    //Генерирует ключ для получения общего рекорда, рекорда в определленном режиме, с определенной скоростью
    public static string GetBestRecordKey(SingleGameHelper.GameModeEnum GameMode, int GameSpeed)
    {
        string Key = "";
        Key += "BestRecord";
        Key += GameMode.ToString();
        Key += GameSpeed;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Создан префикс ключ для получения общего рекорда: " + Key);

        return Key;  //Пример значения выглядит так (BestRecordClassic9)
    }
}
