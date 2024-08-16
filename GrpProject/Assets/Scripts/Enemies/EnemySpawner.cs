using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    private float accumulatedWeight;
    private System.Random rand = new System.Random();

    private void Start()
    {
        StartCoroutine(GameTimer());
    }

    private void SpawnRandomEnemy(Vector3 pos)
    {
        GameObject prefab = enemies[Random.Range(0, enemies.Length)];

        Instantiate(prefab, pos, Quaternion.identity, transform);
    }

    private void CalculateSpawnWeights()
    {
        accumulatedWeight = 0f;
        foreach (GameObject enemy in enemies)
        {
            accumulatedWeight += enemy.GetComponent<Enemy>().spawnChance;
        }
    }

    private IEnumerator GameTimer() // timer used to decide when to spawn enemies
    {
        // first wave - give player 5 seconds to ease into the game
        yield return new WaitForSeconds(5);

        // spawn 10 grunts twice over 2min


        // spawn 15 grunts twice over 2 min


        // spawn 13-15 grunts, 1-2 brutes - 1 min


        // roughly 15 grunts, 3-5 brutes - 1min


        // roughly 10 grunts, 5 brutes, 1-2 snipers - 1 min


        // 12 grunts, 5 brutes, 3 snipers - once this is cleared, boss room opens
    }
}
