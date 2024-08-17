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
        inRoom1, afterGate, inRoom2, pillarRoom, // to separate spawn planes by rooms
        finalWaveSpawned;

    private float accumulatedWeight;
    private System.Random rand = new System.Random();
    private float xUp, y, xDown, zUp, zDown; // spawn boundaries

    private void Awake()
    {  
        CalculateSpawnWeights();
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

        finalWaveSpawned = false;

        StartCoroutine(GameTimer());
    }

    private void Update()
    {
        if (finalWaveSpawned && GameObject.FindWithTag("Enemy") == null)
        {
            // boss time!
        }
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
        if (inRoom1)
        {
            for (int i = 0; i < 3; i++)
                SpawnRandomEnemy(new Vector3(Random.Range(xDown, xUp), y, Random.Range(zDown, zUp))); 
        }

        enemies[0].chance = 80f; // decrease spawn rate of grunt
        enemies[1].chance = 40f; // increase spawn rate of brutes
        CalculateSpawnWeights(); // recalculate weights

        // after 30 seconds, spawn 10 grunts, and introduce brutes over 2 spawn areas
        yield return new WaitForSeconds(28); // 28 seconds plus the initial 2 seconds

        if (inRoom1)
        {
            for (int i = 0; i < 5; i++)
                SpawnRandomEnemy(new Vector3(Random.Range(xDown, xUp), y, Random.Range(zDown, zUp)));
        }

        enemies[0].chance = 70f; // decrease spawn rate of grunt
        enemies[1].chance = 60f; // increase spawn rate of brutes
        CalculateSpawnWeights(); // recalculate weights

        // after 1 min, spawn after the gate to guide the player - 1 spawn area
        yield return new WaitForSeconds(30); // 1min elapsed

        if (afterGate)
        {
            for (int i = 0; i < 5; i++)
                SpawnRandomEnemy(new Vector3(Random.Range(xDown, xUp), y, Random.Range(zDown, zUp)));
        }

        // room 2 - 1 spawn area, 10 enemies
        yield return new WaitForSeconds(30); //1.5min elapsed

        if (inRoom2 && inUse)
        {
            for (int i = 0; i < 10; i++)
                SpawnRandomEnemy(new Vector3(Random.Range(xDown, xUp), y, Random.Range(zDown, zUp)));
        }

        // room 2 - 2 spawn areas, introduce a sniper
        yield return new WaitForSeconds(60); // 2.5min elapsed

        if (inRoom2)
        { 
            for (int i = 0; i < 6; i++)
                SpawnRandomEnemy(new Vector3(Random.Range(xDown, xUp), y, Random.Range(zDown, zUp))); 
            // sniper spawns handled by SniperSpawn.cs
        }

        // room 2, 2 snipers
        yield return new WaitForSeconds(90); // 4min elapsed

        if (inRoom2)
        {
            for (int i = 0; i < 8; i++)
                SpawnRandomEnemy(new Vector3(Random.Range(xDown, xUp), y, Random.Range(zDown, zUp)));
        }

        // pillar room - 1 spawn area, sniper on the pillar
        yield return new WaitForSeconds(90); // 5.5min

        if (pillarRoom)
        {
            for (int i = 0; i < 10; i++)
                SpawnRandomEnemy(new Vector3(Random.Range(xDown, xUp), y, Random.Range(zDown, zUp)));
            finalWaveSpawned = true;
        }
    } 
}
