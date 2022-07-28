using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class PerformanceMeasurer : MonoBehaviour
{
    [SerializeField] private CubeSpawnSystem cubeSpawnSystem;
    [SerializeField] private PhysicsCubeSpawnSystem physicsCubeSpawnSystem;
    [SerializeField] private string fileName = "performanceReport";

    private string performanceReportPath;
    private float nextIncrementation = 6f;
    private float breakeTime = 5f;

    private int fps;
    private int sumFps = 0;
    private int fpsMeasured = 0;
    private int minFps = int.MaxValue;
    private int maxFps = int.MinValue;

    private int incrementations = 0;
    private int spawnAmount = -1;

    void Start()
    {
        performanceReportPath = $"Assets/PerformanceReports/{fileName}.csv";

        if (cubeSpawnSystem.enabled)
        {
            cubeSpawnSystem.selfIncrement = false;
            spawnAmount = cubeSpawnSystem.spawnAmount;
        }
    }

    void Update()
    {
        if (Time.time < 1f) return;

        CalculateFPS();

        if (Time.time < nextIncrementation) return;

        SaveCalculations();
        IncreaseCubes();
    }

    private void SaveCalculations()
    {
        if (!File.Exists(performanceReportPath))
        {
            File.WriteAllText(performanceReportPath, "MIN_FPS, AVG_FPS, MAX_FPS, CUBES,");
        }

        int avgFps = (fpsMeasured == 0 ? 0 : sumFps / fpsMeasured);

        File.AppendAllText(performanceReportPath, $"\n{minFps}, {avgFps}, {maxFps}, {spawnAmount * incrementations},");

        CheckForEnd(avgFps);

        sumFps = 0;
        fpsMeasured = 0;
        minFps = int.MaxValue;
        maxFps = int.MinValue;
    }

    private void IncreaseCubes()
    {
        nextIncrementation = Time.time + breakeTime;

        incrementations++;

        if (cubeSpawnSystem.enabled) cubeSpawnSystem.SpawnNextWave();

        if (physicsCubeSpawnSystem.enabled) physicsCubeSpawnSystem.SpawnNextWave();
    }

    private void CalculateFPS()
    {
        fps = (int)(1f / Time.deltaTime);
        fpsMeasured++;
        sumFps += fps;

        if (fps > maxFps)
        {
            maxFps = fps;
            return;
        }
        if (fps < minFps) minFps = fps;
    }

    private void CheckForEnd(int avgFps)
    {
        if (avgFps >= 30) return;

        EditorApplication.ExitPlaymode();
    }
}
