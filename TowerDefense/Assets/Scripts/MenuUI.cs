using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {

    public GameObject mainMenuUI;
    public GameObject optionMenuUI;

    public Slider[] volumeSliders;

    void Start()
    {
        AudioSetting audioSetting = GameDataManager.instance.gameData.optionData.audioSetting;
        volumeSliders[0].value = audioSetting.masterVolumePercent;
        volumeSliders[1].value = audioSetting.musicVolumePercent;
        volumeSliders[2].value = audioSetting.sfxVolumePercent;
    }

    public void MainMenu()
    {
        AudioManager.instance.PlaySound2D("MouseClick");
        mainMenuUI.SetActive(true);
        optionMenuUI.SetActive(false);
    }

    public void Play()
    {
        AudioManager.instance.PlaySound2D("MouseClick");
        SceneManager.LoadScene("Game");
    }

    public void Option()
    {
        AudioManager.instance.PlaySound2D("MouseClick");
        mainMenuUI.SetActive(false);
        optionMenuUI.SetActive(true);
    }

    public void Quit()
    {
        AudioManager.instance.PlaySound2D("MouseClick");
        Application.Quit();
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetSfxVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }
}
