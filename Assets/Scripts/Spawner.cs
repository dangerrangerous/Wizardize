using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	// Problem: Enemy spawn isn't working properly. It could be the interaction with the Enemy Laugh coroutine...considering an object pool for enemies

    public Wave[] wave;
    public Enemy enemy;

	public bool developerMode;

    public Potion potion;

    LivingEntity playerEntity;
    Transform playerT;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;
    int potionsRemainingToSpawn;

    MapGenerator map;

    // AntiCamping - 
    // if player is camping... spawn the monsters possibly from behind camera position
    // player must move to fend off the encroaching darkness (tunnel vision effect?)
    float timeBetweenCampingChecks = 2;
    float campThresholdDistance = 1.5f;
    float nextCampCheckTime;
    Vector3 campPositionOld;
    bool isCamping;

    bool isDisabled;

    public event System.Action<int> OnNewWave;

    void Start()
    {
        playerEntity = FindObjectOfType<Player>();
       
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBetweenCampingChecks + Time.deltaTime;
        campPositionOld = playerT.position;
        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }

    void Update()
    {
        if (!isDisabled)
        {

            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerT.position;
            }

			if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
				Debug.Log("Enemies remaining to spawn: " + enemiesRemainingToSpawn);
            }
            if(potionsRemainingToSpawn > 0)
            {
                potionsRemainingToSpawn--;

                StartCoroutine(SpawnPotion());
                
            }
        }
		if (developerMode) 
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				StopCoroutine("SpawnEnemy");
				foreach (Enemy enemy in FindObjectsOfType<Enemy>()) 
				{
					GameObject.Destroy(gameObject);
				}
				NextWave();
			}
		}
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping)
        {
            spawnTile = map.GetTileFromPosition(playerT.position);
        }

        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        Color flashColor = Color.blue;
        float spawnTimer = 0;

		// replace this with enemy spawn effect
        while (spawnTimer < spawnDelay)
        {
            tileMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

		Enemy spawnedEnemy = Instantiate (enemy, spawnTile.position, Quaternion.identity) as Enemy;
		spawnedEnemy.OnDeath += OnEnemyDeath;

		Debug.Log ("enemy spawned at location : " + spawnedEnemy.transform.position);

		spawnedEnemy.SetCharacteristics (currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColor);
    }


    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    void ResetPlayerPosition()
    {

        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up;
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
		Debug.Log ("Enemy killed, enemies remaining: " + enemiesRemainingAlive);

        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }


    void NextWave()
    {
        currentWaveNumber++;

        print("Wave: " + currentWaveNumber);

        if (currentWaveNumber - 1 < wave.Length)
        {
            currentWave = wave[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }
            ResetPlayerPosition();

        }

    }

    // thinking about generic programming and how the spawn enemy functions could be used for potions...
    IEnumerator SpawnPotion ()
    {
        
        Transform spawnTile = map.GetRandomOpenTile();
        Potion spawnedPotion = Instantiate(potion, spawnTile.position + Vector3.up, Quaternion.identity) as Potion;
        yield return null;
        
    }

 
    [System.Serializable]
    public class Wave
    {
		public bool infinite;

        public int enemyCount;
        public float timeBetweenSpawns;
        public int potionCount;

		public float moveSpeed;
		public int hitsToKillPlayer;
		public float enemyHealth;
		public Color skinColor;

    }
}
