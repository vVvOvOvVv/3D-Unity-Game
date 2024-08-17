using System.Collections;
using UnityEngine;

public class SniperSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] snipers; // snipers separate as their spawns are guaranteed
    private int spawnCounter;
    private void Start()
    {
        // ensure all snipers are not active
        foreach (GameObject sniper in snipers) 
            sniper.SetActive(false);

        spawnCounter = 0;
        StartCoroutine(SpawnSnipers());
    }
    private IEnumerator SpawnSnipers()
    {
        yield return new WaitForSeconds(150); // 2.5 min
        Spawn(1);

        yield return new WaitForSeconds(90); // 4 min
        Spawn(2);

        yield return new WaitForSeconds(90); // 5.5 min
        Spawn(1);
    }

    private void Spawn(int numOfSpawns)
    {
        for (int i = spawnCounter; i < numOfSpawns + spawnCounter; i++) 
            snipers[i].SetActive(true);

        spawnCounter += numOfSpawns;
    }
}
