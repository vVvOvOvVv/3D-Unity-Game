using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemySpawn // to control the enemy spawn rates
{
    [SerializeField] public string name; // primarily for debugging
    [SerializeField] public GameObject prefab;
    [Range(0f, 100f)] public float chance = 100f;

    [HideInInspector] public float weight;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawn[] enemies; // sniper spawns are guaranteed, and should not be included here
                                                   // ensure order is grunt > brute
    [SerializeField] private GameObject spawnPlane; // to get spawn boundaries
    [SerializeField]
    private bool inUse, // to allow toggling of spawn locations
        inRoom1, afterGate, inRoom2, inFenceRoom, inKegRoom, // to separate spawn planes by rooms
        inBalconyRoom, inFenceRoom2, inRoom3;
    public bool allEnemiesDefeated, // flag to determine if all enemies have been defeated
        finalWaveSpawned; // flag to determine if final wave has spawned
    private SniperSpawn sniperSpawnScript;


    private float accumulatedWeight;
    private System.Random rand = new System.Random();
    private float xUp, y, xDown, zUp, zDown; // spawn boundaries

    private void Awake()
    {
        CalculateSpawnWeights();
        GameObject sniperSpawner = GameObject.Find("Sniper Spawner");
        sniperSpawnScript = sniperSpawner.GetComponent<SniperSpawn>();
    }

    private void Start()
    {
        // get spawn boundaries
        Vector3 spawnPlaneCenter = spawnPlane.transform.position;
        float xLengthFromCenter = spawnPlane.transform.localScale.x / 2,
            zLengthFromCenter = spawnPlane.transform.localScale.z / 2;
        xUp = spawnPlaneCenter.x + xLengthFromCenter;
        xDown = spawnPlaneCenter.x - xLengthFromCenter;
        y = spawnPlaneCenter.y;
        zUp = spawnPlaneCenter.z + zLengthFromCenter;
        zDown = spawnPlaneCenter.z - zLengthFromCenter;

        allEnemiesDefeated = false;

        StartCoroutine(GameTimer());
    }

    private void Update()
    {
        if (GameObject.FindWithTag("Enemy") == null)
            allEnemiesDefeated = true;
    }

    private void SpawnRandomEnemy(Vector3 pos)
    {
        EnemySpawn randomEnemy = enemies[GetRandomEnemyIndex()];
        Instantiate(randomEnemy.prefab, pos, Quaternion.identity, transform);
    }

    private void CalculateSpawnWeights()
    {
        accumulatedWeight = 0f;
        foreach (EnemySpawn enemy in enemies)
        {
            accumulatedWeight += enemy.chance;
            enemy.weight = accumulatedWeight;
        }
    }

    private int GetRandomEnemyIndex()
    {
        double randDouble = rand.NextDouble() * accumulatedWeight;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].weight >= randDouble)
                return i;
        }

        return 0;
    }

    private IEnumerator GameTimer() // timer used to decide when to spawn enemies
    {
        // first wave - give player 2 seconds to ease into the game
        yield return new WaitForSeconds(2);

        /* for (int i = 0; i < 20; i++)         -- to test the spawn system
            SpawnRandomEnemy(new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3))); */

        // initial wave - 6 grunts spread over 2 spawn areas to ease the player into the game
        SpawnEnemies(3, inRoom1);

        enemies[0].chance = 80f; // decrease spawn rate of grunt
        enemies[1].chance = 40f; // increase spawn rate of brutes
        CalculateSpawnWeights(); // recalculate weights

        // after 20 seconds, introduce brutes over 2 spawn areas
        yield return new WaitForSeconds(18); 

        SpawnEnemies(3, inRoom1); 

        enemies[0].chance = 70f; // decrease spawn rate of grunt
        enemies[1].chance = 60f; // increase spawn rate of brutes
        CalculateSpawnWeights(); // recalculate weights

        // spawn after the gate to guide the player - 1 spawn area
        yield return new WaitForSeconds(20); 

        SpawnEnemies(5, afterGate); 

        // room 2 - 1 spawn area, 10 enemies
        yield return new WaitForSeconds(30); 

        SpawnEnemies(10, inRoom2, inUse);

        // room 2 - 2 spawn areas
        yield return new WaitForSeconds(30); 

        SpawnEnemies(6, inRoom2); 

        enemies[0].chance = 100f; // spawn ratio now 50/50
        enemies[1].chance = 100f;  
        CalculateSpawnWeights(); // recalculate weights

        // fence room - 1 spawn area
        yield return new WaitForSeconds(90); 

        SpawnEnemies(5, inFenceRoom); 

        // keg room - 1 spawn area + sniper
        yield return new WaitForSeconds(90);  

        SpawnEnemies(5, inKegRoom);
        sniperSpawnScript.Spawn(1);

        // balcony room - 2 snipers, 1 spawn area
        yield return new WaitForSeconds(60);  

        SpawnEnemies(8, inBalconyRoom);
        sniperSpawnScript.Spawn(2);

        // fence room 2 electric boogaloo - 1 sniper
        yield return new WaitForSeconds(90);  

        SpawnEnemies(5, inFenceRoom2);
        sniperSpawnScript.Spawn(1);

        // room 3, pt 1, bridge area
        yield return new WaitForSeconds(60);  

        SpawnEnemies(5, inRoom3, inUse);
        sniperSpawnScript.Spawn(2);

        // room 3, pt 2, larger area
        yield return new WaitForSeconds(90);  

        SpawnEnemies(5, inFenceRoom2);
        sniperSpawnScript.Spawn(2);
        finalWaveSpawned = true;
    }

    private void SpawnEnemies(int numOfEnemies, bool inRoom, bool use = true)
    {
        if (inRoom && use) 
        {
            for (int i = 0; i < numOfEnemies; i++)
                SpawnRandomEnemy(new Vector3(Random.Range(xDown, xUp), y, Random.Range(zDown, zUp)));
        }
    }
}