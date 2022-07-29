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

    private string tempFPSCalculations = "";

    void Start()
    {
#if UNITY_EDITOR
        performanceReportPath = $"Assets/PerformanceReports/{fileName}.csv";
#else
        performanceReportPath = $"{Application.dataPath}/PerformanceReports/{fileName}.csv";
#endif

        if (cubeSpawnSystem.enabled)
        {
            cubeSpawnSystem.selfIncrement = false;
            spawnAmount = cubeSpawnSystem.spawnAmount;
        }

        if (physicsCubeSpawnSystem.enabled)
        {
            physicsCubeSpawnSystem.selfIncrement = false;
        }
    }

    void Update()
    {
        if (Time.time < 1f) return;

        if (cubeSpawnSystem.enabled)
        {
            CalculateFPS();

            if (Time.time < nextIncrementation) return;

            SaveCalculations();
            IncreaseCubes();

            return;
        }
        
        if (physicsCubeSpawnSystem.enabled)
        {
            CalculateFPSPhysics();
            SaveCalculationsPhysics();
        }
    }

    private void SaveCalculations()
    {
#if UNITY_EDITOR
        if (!File.Exists(performanceReportPath))
        {
            File.WriteAllText(performanceReportPath, "MIN_FPS, AVG_FPS, MAX_FPS, CUBES,");
        }
#else
        string directory = performanceReportPath.Substring(0, performanceReportPath.LastIndexOf("/"));
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
#endif

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
        if (avgFps >= 24) return;

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void CalculateFPSPhysics()
    {
        fps = (int)(1f / Time.deltaTime);

        fpsMeasured++;
        sumFps += fps;

        spawnAmount = physicsCubeSpawnSystem.SpawnPhysicsCube();
    }

    private void SaveCalculationsPhysics()
    {
        if (!File.Exists(performanceReportPath))
        {
            File.WriteAllText(performanceReportPath, "FPS, CUBES,");
        }

        tempFPSCalculations += $"\n{fps}, {spawnAmount}";

        if (spawnAmount % 100 != 0) return;

        File.AppendAllText(performanceReportPath, tempFPSCalculations);

        tempFPSCalculations = "";

        CheckForEnd(sumFps / fpsMeasured);

        sumFps = 0;
        fpsMeasured = 0;
    }
}
