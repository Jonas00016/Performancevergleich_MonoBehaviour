using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnSystem : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int axisLength = 100;
    [SerializeField] float spacingAmount = 1.5f;
    [SerializeField] float time = 0f;
    [SerializeField] float breakUntill = 5f;

    private int spawnedCubes = 0;
    public int _spawnAmount;

    public bool selfIncrement = true;

    void Start()
    {
        _spawnAmount = axisLength * axisLength;
    }

    void Update()
    {
        if (!selfIncrement) return;

        time += Time.deltaTime;
        if (time < breakUntill) return;
        time = 0f;

        SpawnNextWave();
    }

    public void SpawnNextWave()
    {
        for (int x = 0; x < axisLength; x++)
        {
            for (int z = 0; z < axisLength; z++)
            {
                Vector3 position = new Vector3(x, spawnedCubes / _spawnAmount, z) * spacingAmount;
                Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                Instantiate(prefab, position, rotation);

                spawnedCubes++;
            }

        }
    }

    public int spawnAmount
    {
        get => axisLength * axisLength;
    }
}
