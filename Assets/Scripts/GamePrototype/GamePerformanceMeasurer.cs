using UnityEngine;
using UnityEditor;
using System.IO;

public class GamePerformanceMeasurer : MonoBehaviour
{
    private const int MAX_ENEMIES = 100000;
    private const float BREAK_TIME = 10f;

    [SerializeField] private EnemySpawnSystem enemySpawnSystem;
    [SerializeField] private string fileName = "gamePrototypeReport";

    private string performanceReportPath;

    private int fps;
    private int sumFps = 0;
    private int fpsMeasured = 0;
    private int minFps = int.MaxValue;
    private int maxFps = int.MinValue;

    private int enemyCount = 0;
    private float timer = 0f;
    private float timePassed = 0f;
    private int spawnedEnemies = 0;

    void Start()
    {
#if UNITY_EDITOR
        performanceReportPath = $"Assets/PerformanceReports/{fileName}.csv";
#else
        QualitySettings.vSyncCount = 0;
        performanceReportPath = $"{Application.dataPath}/PerformanceReports/{fileName}.csv";
#endif

        enemySpawnSystem.selfIncrement = false;
    }

    void Update()
    {
        if (Time.time < 1f) return;

        timePassed += Time.deltaTime;
        if (timePassed >= 60f) CheckForEnd(-1);

        CalculateFPS();

        timer += Time.deltaTime;

        if (enemyCount >= MAX_ENEMIES || timer < BREAK_TIME) return;
        
        SaveCalculations();
        IncreaseEnemies();
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

        File.AppendAllText(performanceReportPath, $"\n{minFps}, {avgFps}, {maxFps}, {spawnedEnemies},");

        CheckForEnd(avgFps);

        sumFps = 0;
        fpsMeasured = 0;
        minFps = int.MaxValue;
        maxFps = int.MinValue;
    }

    private void IncreaseEnemies()
    {
        timer = 0f;
        enemySpawnSystem.SpawnEnemies();
        spawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
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
}
