using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCubeSpawnSystem : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int cubesPerSecond = 100;
    [SerializeField] Vector3 spawnPosition = new Vector3(0f, 50f, 0f);
    [SerializeField] float upswing = 20f;

    private float nextTime = 0f;
    private float time = 0f;

    private int cubesSpawned = 0;

    void Update()
    {
        time += Time.deltaTime;

        if (time < nextTime) return;
        nextTime += 1f / cubesPerSecond;

        SpawnNextWave();
    }

    public void SpawnNextWave()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        GameObject spawnedCube = Instantiate(prefab, spawnPosition, rotation);

        spawnedCube.GetComponent<Rigidbody>().AddForce(Vector3.up * upswing, ForceMode.Impulse);
    }
}
