using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public bool devMode;                                //測試用  直接過關

    public GameObject spawnPoint;                       //出生點
    public Enemy enemy;                                 //敵人
    public Wave[] waves;                                //關卡

    Wave currentWave;                                   //目前關卡
    int currentWaveNumber;                              //關卡編號

    int enemiesRemainingToSpawn;                        //敵人 產生剩餘數量
    int enemiesRemainingAlive;                          //敵人剩餘數量
    float nextSpawnTime;                                //下次產生時間

    public event System.Action<int> OnNewWave;          //新關卡觸發

    private void Start()
    {
        NextWave();
    }
    private void OnEnable()
    {
        Enemy.OnDeathStatic += OnEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnDeathStatic -= OnEnemyDeath;
    }

    void Update ()
	{

        if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)         //判斷產生敵人
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            StartCoroutine("SpawnEnemy");
        }
        
        //直接過關  測試用
        if (devMode)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StopCoroutine("SpawnEnemy");
                foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    GameObject.Destroy(enemy.gameObject);
                }
                NextWave();
            }
        }
    }

    //產生敵人動畫
    IEnumerator SpawnEnemy()
    {
        Enemy spawnedEnemy = Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity) as Enemy;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.enemyHealth, currentWave.skinColour);

        yield return null;
    }

    //敵人死亡
    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;    //敵人剩餘數量

        if (enemiesRemainingAlive == 0)
        {
            NextWave();             //下一關
        }
    }
   
    //換下一關
    void NextWave()
    {
        if (currentWaveNumber > 0)
        {
           // AudioManager.instance.PlaySound2D("Level Complete");            //播放過關音效
        }
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)                           //讀取關卡怪物數量等等
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);                               //載入關卡
            }
        }
    }

    //關卡類別  方便設定
    [System.Serializable]
    public class Wave
    {
        public bool infinite;                       //無限
        public int enemyCount;                      //敵人數量
        public float timeBetweenSpawns;             //產生間隔時間

        public float moveSpeed;                     //移動速度
        public float enemyHealth;                   //敵人生命值
        public Color skinColour;                    //敵人顏色
    }
}