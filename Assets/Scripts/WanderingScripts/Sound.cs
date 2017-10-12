using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

    PlayerPreferencesHelper PPrefs = new PlayerPreferencesHelper();

    public AudioSource Audio_BackGroundMusic;
    public bool GetMute_BackGroundMusic()
    {
        return Audio_BackGroundMusic.mute;
    }
    public float GetVolume_BackGroundMusic()
    {
        return Audio_BackGroundMusic.volume;
    }

    public AudioSource Audio_ButtonClickSound;
    public bool GetMute_ButtonClickSound()
    {
        return Audio_ButtonClickSound.mute;
    }
    public float GetVolume_ButtonClickSound()
    {
        return Audio_ButtonClickSound.volume;
    }

    private bool Primary;

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

        if (FindObjectsOfType<Sound>().Length == 1)
        {
            Primary = true;
        }

        if (Primary == false)
        {
            Destroy(this.gameObject);
        }

        InitializedSound();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void InitializedSound()
    {
        Audio_BackGroundMusic.volume = (PlayerPrefs.GetFloat(PlayerPreferencesHelper.VolumePrefEnum.MusicVolumePercentage.ToString(), 100) / 100);
        Audio_ButtonClickSound.volume = (PlayerPrefs.GetFloat(PlayerPreferencesHelper.VolumePrefEnum.SoundVolumePercentage.ToString(), 100) / 100);

        bool muteMusic = false;

        if (PlayerPrefs.GetInt(PlayerPreferencesHelper.VolumePrefEnum.MusicMuteBool.ToString(), 0) == 1)
        {
            muteMusic = true;
        }
        Audio_BackGroundMusic.mute = muteMusic;

        bool muteSound = false;

        if (PlayerPrefs.GetInt(PlayerPreferencesHelper.VolumePrefEnum.SoundMuteBool.ToString(), 0) == 1)
        {
            muteSound = true;
        }
        Audio_ButtonClickSound.mute = muteSound;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Инициализация параметров звука: "
           + "Активность музыки: " + Audio_BackGroundMusic.mute + "; "
           + "Громкость музыки: " + Audio_BackGroundMusic.volume + "%; "
           + "Активность звука: " + Audio_ButtonClickSound.mute + "; "
           + "Громкость звука: " + Audio_ButtonClickSound.volume + "%; ");
    }

    public void PlaySoundButtonClick()
    {
        Audio_ButtonClickSound.Play();
    }

    public void SetVolume_BackGroundMusic(float volume) // 0-100%
    {      
        Audio_BackGroundMusic.volume = (volume / 100);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Изменено значение громкости музыки: " + Audio_BackGroundMusic.volume * 100 + " %");
    }

    public void SetVolume_ButtonClickSound(float volume) // 0-100%
    {
        Audio_ButtonClickSound.volume = (volume / 100);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Изменено значение громкости звука: " + Audio_ButtonClickSound.volume * 100 + " %");
    }

    public void SetMute_BackGroundMusic(bool mute)
    {
        Audio_BackGroundMusic.mute = mute;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Измененно значение активности музыки: " + GetMute_BackGroundMusic());
    }

    public void SetMute_ButtonClickSound(bool mute)
    { 
        Audio_ButtonClickSound.mute = mute;

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Измененно значение активности звука: " + GetMute_ButtonClickSound());
    }

    public void SaveChange()
    {
        int muteClick = 0;

        if (Audio_ButtonClickSound.mute)
        {
            muteClick = 1;
        }
        PlayerPrefs.SetInt(PlayerPreferencesHelper.VolumePrefEnum.SoundMuteBool.ToString(), muteClick);

        int muteMusic = 0;
        if (Audio_BackGroundMusic.mute)
        {
            muteMusic = 1;
        }
        PlayerPrefs.SetInt(PlayerPreferencesHelper.VolumePrefEnum.MusicMuteBool.ToString(), muteMusic);

        PlayerPrefs.SetFloat(PlayerPreferencesHelper.VolumePrefEnum.SoundVolumePercentage.ToString()
            , Audio_ButtonClickSound.volume * 100);
        PlayerPrefs.SetFloat(PlayerPreferencesHelper.VolumePrefEnum.MusicVolumePercentage.ToString()
            , Audio_BackGroundMusic.volume * 100);

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сохранены изменения звука: " 
            + "Активность музыки: " + Audio_BackGroundMusic.mute + "; "
            + "Громкость музыки: " + Audio_BackGroundMusic.volume * 100 + "%; "
            + "Активность звука: " + Audio_ButtonClickSound.mute + "; "
            + "Громкость звука: " + Audio_ButtonClickSound.volume * 100 + "%; ");

    }
}
