using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyGO;

    public float maxSpawnRateInSeconds = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke ("SpawnEnemy", maxSpawnRateInSeconds);

        // vamos aumentar o spawn rate a cada 30 segundos
        InvokeRepeating ("IncreaseSpawnRate", 0f, 30f);
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
}
