using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    public Text scoreUI;
    public Text gameoverScoreUI;
    public Text moneyUI;

    public Text lifeUI;

    public GameObject optionMenuUI;

    public GameObject gameOverUI;

    public Slider[] volumeSliders;

    public static GameUI instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnGameOverStatic += GameOver;
    }

    private void OnDisable()
    {
        GameManager.OnGameOverStatic -= GameOver;
    }

    void Start()
    {
        AudioSetting audioSetting = GameDataManager.instance.gameData.optionData.audioSetting;
        volumeSliders[0].value = audioSetting.masterVolumePercent;
        volumeSliders[1].value = audioSetting.musicVolumePercent;
        volumeSliders[2].value = audioSetting.sfxVolumePercent;
    }
    
    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Option()
    {
        AudioManager.instance.PlaySound2D("MouseClick");
        optionMenuUI.SetActive(!optionMenuUI.activeSelf);
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        gameoverScoreUI.text = scoreUI.text;
    }

    public void UpdateScore()
    {
        scoreUI.text = "Score : " + GameManager.instance.score;
    }

    public void UpdateLife()
    {
        lifeUI.text = "Life : " + GameManager.instance.life;
    }

    public void UpdateMoney()
    {
        moneyUI.text = "Money : " + GameManager.instance.money;
    }

    public void Restart()
    {
        AudioManager.instance.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
