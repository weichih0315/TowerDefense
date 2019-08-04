using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public bool devMode;                                //���ե�  �����L��

    public GameObject spawnPoint;                       //�X���I
    public Enemy enemy;                                 //�ĤH
    public Wave[] waves;                                //���d

    Wave currentWave;                                   //�ثe���d
    int currentWaveNumber;                              //���d�s��

    int enemiesRemainingToSpawn;                        //�ĤH ���ͳѾl�ƶq
    int enemiesRemainingAlive;                          //�ĤH�Ѿl�ƶq
    float nextSpawnTime;                                //�U�����ͮɶ�

    public event System.Action<int> OnNewWave;          //�s���dĲ�o

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

        if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)         //�P�_���ͼĤH
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            StartCoroutine("SpawnEnemy");
        }
        
        //�����L��  ���ե�
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

    //���ͼĤH�ʵe
    IEnumerator SpawnEnemy()
    {
        Enemy spawnedEnemy = Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity) as Enemy;
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.enemyHealth, currentWave.skinColour);

        yield return null;
    }

    //�ĤH���`
    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;    //�ĤH�Ѿl�ƶq

        if (enemiesRemainingAlive == 0)
        {
            NextWave();             //�U�@��
        }
    }
   
    //���U�@��
    void NextWave()
    {
        if (currentWaveNumber > 0)
        {
           // AudioManager.instance.PlaySound2D("Level Complete");            //����L������
        }
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)                           //Ū�����d�Ǫ��ƶq����
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);                               //���J���d
            }
        }
    }

    //���d���O  ��K�]�w
    [System.Serializable]
    public class Wave
    {
        public bool infinite;                       //�L��
        public int enemyCount;                      //�ĤH�ƶq
        public float timeBetweenSpawns;             //���Ͷ��j�ɶ�

        public float moveSpeed;                     //���ʳt��
        public float enemyHealth;                   //�ĤH�ͩR��
        public Color skinColour;                    //�ĤH�C��
    }
}