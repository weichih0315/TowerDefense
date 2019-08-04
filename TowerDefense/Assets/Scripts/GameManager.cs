using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int life = 10;
    public int money = 1000;
    public int score = 0;

    public static GameManager instance;

    public static event System.Action OnGameOverStatic; 

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        Enemy.OnDeathStatic += OnEnemyDeath;
        Enemy.OnEndStatic += OnEnemyEnd;
    }

    private void OnDisable()
    {
        Enemy.OnDeathStatic -= OnEnemyDeath;
        Enemy.OnEndStatic -= OnEnemyEnd;
    }

    public void Start()
    {
        GameUI.instance.UpdateScore();
        GameUI.instance.UpdateLife();
        GameUI.instance.UpdateMoney();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameUI.instance.GameOver();
        }
    }

    public void GameOver()
    {
        if (OnGameOverStatic != null)
            OnGameOverStatic();
    }

    private void OnEnemyDeath()
    {
        score += 100;
        money += 100;
        GameUI.instance.UpdateScore();
        GameUI.instance.UpdateMoney();
    }

    private void OnEnemyEnd()
    {
        life -= 1;
        GameUI.instance.UpdateLife();
        if (life < 1)
        {
            GameOver();
        }
    }
}