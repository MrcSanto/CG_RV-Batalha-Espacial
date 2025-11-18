using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyGO;

    public float maxSpawnRateInSeconds = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // função para spawnar uma nave inimiga
    void SpawnEnemy()
    {
        // canto inferior esquerdo da tela
        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

        // canto superior direito da tela
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

        // instanciando um inimigo
        GameObject anEnemy = (GameObject)Instantiate (EnemyGO);
        anEnemy.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        ScheduleNextEnemySpawn ();
    }

    void ScheduleNextEnemySpawn()
    {
        float spawnInNSeconds;

        if (maxSpawnRateInSeconds > 1f)
        {
            // escolhendo um n° entre max e maxSpawnRateInSeconds
            spawnInNSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        } 
        else
            spawnInNSeconds = 1f;

        Invoke("SpawnEnemy", spawnInNSeconds);
    }

    void IncreaseSpawnRate()
    {
        if (maxSpawnRateInSeconds > 1f)
            maxSpawnRateInSeconds--;
        
        if (maxSpawnRateInSeconds == 1f)
            CancelInvoke("IncreaseSpawnRate");
    }

    public void UnscheduleEnemySpawner()
    {
        CancelInvoke ("SpawnEnemy");
        CancelInvoke ("IncreaseSpawnRate");
    }

    public void ScheduleEnemySpawner()
    {
        // Ajusta o spawn rate conforme a dificuldade
        if (DifficultyManager.CurrentDifficulty == DifficultyManager.Difficulty.Normal)
        {
            maxSpawnRateInSeconds = 5f;
        }
        else if (DifficultyManager.CurrentDifficulty == DifficultyManager.Difficulty.Hard)
        {
            maxSpawnRateInSeconds = 2f;
        }

        Invoke("SpawnEnemy", maxSpawnRateInSeconds);
        InvokeRepeating("IncreaseSpawnRate", 0f, 30f);
    }

}
