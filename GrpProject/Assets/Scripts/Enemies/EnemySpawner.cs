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
    [SerializeField] EnemySpawn[] enemies; // sniper spawns are guaranteed, and should not be included here

    private float accumulatedWeight;
    private System.Random rand = new System.Random();

    private void Awake()
    {
        CalculateSpawnWeights();
    }

    private void Start()
    {
        StartCoroutine(GameTimer());
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
            accumulatedWeight += enemy.weight;
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

        // spawn 20 grunts twice over 2min - 4 groups of 5


        // spawn 30 grunts twice over 2 min


        // spawn 25 grunts, 1-2 brutes - 1 min


        // roughly 15 grunts, 3-5 brutes - 1min


        // roughly 10 grunts, 5 brutes, 1-2 snipers - 1 min


        // 12 grunts, 5 brutes, 3 snipers - once this is cleared, boss room opens
    }
}
