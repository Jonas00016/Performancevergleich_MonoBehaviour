using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    private const int MAX_ENEMIES = 10000;
    private const int SPAWN_AMOUNT = 100;
    private const float BREAK_TIME = 10f;

    private int enemyCount = 0;
    private float timer = 0f;
    private Transform playerTransform;

    [SerializeField] GameObject enemyPrefab;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (enemyCount >= MAX_ENEMIES || timer < BREAK_TIME) return;
        timer = 0f;

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < SPAWN_AMOUNT; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition(i);

            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            spawnedEnemy.transform.LookAt(playerTransform);
        }
    }

    private Vector3 GetSpawnPosition(int index)
    {
        if (index < SPAWN_AMOUNT / 4 * 1)
        {
            return new Vector3(
                    Random.Range(-50f, -10f),
                    0f,
                    Random.Range(-50f, 50f)
                );
        }

        if (index < SPAWN_AMOUNT / 4 * 2)
        {
            return new Vector3(
                    Random.Range(-50f, 50f),
                    0f,
                    Random.Range(-50f, -10f)
                );
        }

        if (index < SPAWN_AMOUNT / 4 * 3)
        {
            return new Vector3(
                    Random.Range(50f, 10f),
                    0f,
                    Random.Range(-50f, 50f)
                );
        }
       
        return new Vector3(
                Random.Range(-50f, 50f),
                0f,
                Random.Range(50f, 10f)
            );
    }
}
