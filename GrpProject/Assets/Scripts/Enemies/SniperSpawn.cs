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
    } 

    public void Spawn(int numOfSpawns)
    {
        for (int i = spawnCounter; i < numOfSpawns + spawnCounter; i++)
        {
            snipers[i].SetActive(true);
        }

        spawnCounter += numOfSpawns;
    }
}
