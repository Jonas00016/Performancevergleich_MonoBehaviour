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

    public bool increment { get; set; } = false;

    private int spawnedCubes = 0;
    private int spawnAmount;

    void Start()
    {
        spawnAmount = axisLength * axisLength;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time < breakUntill) return;
        time = 0f;

        for (int x = 0; x < axisLength; x++)
        {
            for (int z = 0; z < axisLength; z++)
            {
                Vector3 position = new Vector3(x, spawnedCubes / spawnAmount, z) * spacingAmount;
                Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                Instantiate(prefab, position, rotation);

                spawnedCubes++;
            }

        }
    }
}
