using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour {

    public Text TxtVolumeMusicValue;
    public Slider SlidVolumeMusic;
    public Text TxtVolumeSoundValue;
    public Slider SlidVolumeSound;
    private Sound sound;
    // Use this for initialization
    void Start () {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Сцена: " + SceneManager.GetActiveScene().name + " загружена.");

        sound = FindObjectOfType<Sound>();
        SlidVolumeMusic.value = PlayerPrefs.GetFloat(PlayerPreferencesHelper.VolumePrefEnum.MusicVolumePercentage.ToString(), 100);
        SlidVolumeSound.value = PlayerPrefs.GetFloat(PlayerPreferencesHelper.VolumePrefEnum.SoundVolumePercentage.ToString(), 100);

        if(PlayerPrefs.GetInt(PlayerPreferencesHelper.VolumePrefEnum.MusicMuteBool.ToString(), 0) == 1)
        {
            TxtVolumeMusicValue.text = "Muted";
        }

        if (PlayerPrefs.GetInt(PlayerPreferencesHelper.VolumePrefEnum.SoundMuteBool.ToString(), 0) == 1)
        {
            TxtVolumeSoundValue.text = "Muted";
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ButtonClick_MuteMusic()
    {
        sound.SetMute_BackGroundMusic(!sound.GetMute_BackGroundMusic());

        if (sound.GetMute_BackGroundMusic())
        {
            TxtVolumeMusicValue.text = "Muted";
        }
        else
        {
            ChageSliderVolumeMusic();
        }
    }

    public void ChageSliderVolumeMusic()
    {
        sound.SetMute_BackGroundMusic(false);
        TxtVolumeMusicValue.text = SlidVolumeMusic.value + "%";
        sound.SetVolume_BackGroundMusic(SlidVolumeMusic.value);
    }

    public void ButtonClick_MuteSound()
    {       
        sound.SetMute_ButtonClickSound(!sound.GetMute_ButtonClickSound());
        if (sound.GetMute_ButtonClickSound())
        {
            TxtVolumeSoundValue.text = "Muted";
        }
        else
        {
            ChageSliderVolumeSound();
        }
    }

    public void ChageSliderVolumeSound()
    {      
        sound.SetMute_ButtonClickSound(false);
        TxtVolumeSoundValue.text = SlidVolumeSound.value + "%";
        sound.SetVolume_ButtonClickSound(SlidVolumeSound.value);
        sound.PlaySoundButtonClick();
    }

    public void ButtonClick_BackToMainMenu()
    {
        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Изменения НЕ сохраненны");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: "
           + SceneManagerHelper.SceneManagerEnum.MainMenu.ToString() + " .");

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());
    }

    public void ButtonClick_Save()
    {
        sound.SaveChange();

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Изменения сохраненны");

        Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Начало загрузки сцены: "
           + SceneManagerHelper.SceneManagerEnum.MainMenu.ToString() + " .");

        SceneManager.LoadScene(SceneManagerHelper.SceneManagerEnum.MainMenu.ToString());
    }
}
