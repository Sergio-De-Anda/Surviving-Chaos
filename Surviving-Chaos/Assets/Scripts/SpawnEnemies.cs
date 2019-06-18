using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;
    public GameObject ammoPrefab;
    public GameObject healthPrefab;
    public float spawnTime;
    public Transform[] spawnPoints;
    public Transform[] ammoSpawnPoints;
    public Transform[] healthSpawnPoints;
    public int enemiesCounter;

    void Start()
    {
        enemiesCounter = 0;
        Spawn();
    }

    void Update()
    {
        
        // spawn if zero enemies 
        if (enemiesCounter == 0)
        {
            Invoke("Spawn", 15);
            Debug.Log("Enemy Respawn Starting in 15 seconds");
        }
        
    }

    void Spawn()
    {
        if (enemiesCounter > 0)
            return;
        for (int i = 0; i < 10; i++)
            spawnEnemies();
        //}
        spawnAmmo();
        spawnHealth();
    }

    void spawnEnemies()
    {

        ////create 10 enemies
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        if (enemiesCounter < 7)
            Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        else
            Instantiate(enemyPrefab2, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        enemiesCounter++;       
    }

    void spawnAmmo()
    {
        int ammoPointIndex = Random.Range(0, ammoSpawnPoints.Length);
        Instantiate(ammoPrefab, spawnPoints[ammoPointIndex].position, spawnPoints[ammoPointIndex].rotation);
    }

    void spawnHealth()
    {
        int healthPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(healthPrefab, spawnPoints[healthPointIndex].position, spawnPoints[healthPointIndex].rotation);
    }

    public void decreaseEnemies()
    {
        enemiesCounter--;
    }
}
