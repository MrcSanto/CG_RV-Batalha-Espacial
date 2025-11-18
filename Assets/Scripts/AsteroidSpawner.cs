using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject AsteroidGO;

    public float maxSpawnRateInSeconds = 5f;

    // aumenta o tamanho do asteroide randomicamente
    public float maxScaleMultiplier = 12f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // função para spawnar um asteroide
    void SpawnAsteroid()
    {
        // canto inferior esquerdo da tela
        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

        // canto superior direito da tela
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

        // instanciando um asteroide
        GameObject anAsteroid = (GameObject)Instantiate (AsteroidGO);
        anAsteroid.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        // aplicando a escala aleatoria
        float randomScale = Random.Range(1f, maxScaleMultiplier);
        anAsteroid.transform.localScale *= randomScale;

        ScheduleNextAsteroidSpawn ();
    }

    void ScheduleNextAsteroidSpawn()
    {
        float spawnInNSeconds;

        if (maxSpawnRateInSeconds > 1f)
        {
            // escolhendo um n° entre max e maxSpawnRateInSeconds
            spawnInNSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        } 
        else
            spawnInNSeconds = 1f;

        Invoke("SpawnAsteroid", spawnInNSeconds);
    }

    void IncreaseSpawnRate()
    {
        if (maxSpawnRateInSeconds > 1f)
            maxSpawnRateInSeconds--;
        
        if (maxSpawnRateInSeconds == 1f)
            CancelInvoke("IncreaseSpawnRate");
    }

    public void UnscheduleAsteroidSpawner()
    {
        CancelInvoke ("SpawnAsteroid");
        CancelInvoke ("IncreaseSpawnRate");
    }

    public void ScheduleAsteroidSpawner()
    {
        if (DifficultyManager.CurrentDifficulty == DifficultyManager.Difficulty.Normal)
        {
            maxSpawnRateInSeconds = 4f;
        }
        else
        {
            maxSpawnRateInSeconds = 1.5f;
        }

        Invoke("SpawnAsteroid", maxSpawnRateInSeconds);
        InvokeRepeating("IncreaseSpawnRate", 0f, 30f);
    }

}
