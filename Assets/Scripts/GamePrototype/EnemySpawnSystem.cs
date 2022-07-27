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

        for (int i = 0; i < SPAWN_AMOUNT; i++)
        {
            Vector3 spawnPosition;

            if (i < SPAWN_AMOUNT / 4 * 1)
            {
                spawnPosition = new Vector3(
                        Random.Range(-50f, -10f),
                        0f,
                        Random.Range(-50f, 50f)
                    );
            }
            else if (i < SPAWN_AMOUNT / 4 * 2)
            {
                spawnPosition = new Vector3(
                        Random.Range(-50f, 50f),
                        0f,
                        Random.Range(-50f, -10f)
                    );
            }
            else if (i < SPAWN_AMOUNT / 4 * 3)
            {
                spawnPosition = new Vector3(
                        Random.Range(50f, 10f),
                        0f,
                        Random.Range(-50f, 50f)
                    );
            }
            else
            {
                spawnPosition = new Vector3(
                        Random.Range(-50f, 50f),
                        0f,
                        Random.Range(50f, 10f)
                    );
            }

            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemy.transform.LookAt(playerTransform);
        }
    }
}
