using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public GameObject PortalGO;
    private GameObject scoreUITextGO;
    
    private bool portalSpawned = false;
    private float gameStartTime;
    private float spawnCheckInterval = 5f; // Verifica a cada 5 segundos
    private float nextCheckTime;

    // Configurações de tempo
    public float MIN_SPAWN_TIME = 60f;  // 1 minuto
    public float MAX_SPAWN_TIME = 120f; // 2 minutos

    // Configurações de pontuação para chances de spawn
    private const int LOW_SCORE_THRESHOLD = 500;
    private const int HIGH_SCORE_THRESHOLD = 2000;

    void Start()
    {
        gameStartTime = Time.time;
        nextCheckTime = Time.time + spawnCheckInterval;

        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    void Update()
    {
        // Se já spawnou o portal, não faz mais nada
        if (portalSpawned) return;

        // Verifica se já passou o tempo mínimo
        float elapsedTime = Time.time - gameStartTime;
        if (elapsedTime < MIN_SPAWN_TIME) return;

        // Se passou do tempo máximo, spawna obrigatoriamente
        if (elapsedTime >= MAX_SPAWN_TIME)
        {
            SpawnPortal();
            return;
        }

        // Verifica periodicamente se deve spawnar baseado na pontuação
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + spawnCheckInterval;
            CheckSpawnChance();
        }
    }

    void CheckSpawnChance()
    {
        if (scoreUITextGO == null) return;

        int currentScore = scoreUITextGO.GetComponent<ScoreManager>().Score;
        
        // Calcula a chance de spawn baseada na pontuação
        float spawnChance = CalculateSpawnChance(currentScore);

        // Rola o dado
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= spawnChance)
        {
            SpawnPortal();
        }
    }

    float CalculateSpawnChance(int score)
    {
        // Score baixo (< 500): 5% de chance a cada verificação
        if (score < LOW_SCORE_THRESHOLD)
        {
            return 0.05f;
        }
        // Score médio (500-2000): 10-30% de chance (interpolação linear)
        else if (score < HIGH_SCORE_THRESHOLD)
        {
            float t = (float)(score - LOW_SCORE_THRESHOLD) / (HIGH_SCORE_THRESHOLD - LOW_SCORE_THRESHOLD);
            return Mathf.Lerp(0.10f, 0.30f, t);
        }
        // Score alto (>= 2000): 40% de chance a cada verificação
        else
        {
            return 0.40f;
        }
    }

    void SpawnPortal()
    {
        if (portalSpawned || PortalGO == null) return;

        Vector2 spawnPosition = GetRandomSpawnPosition();
        
        // Instancia o portal da mesma forma que o EnemySpawner
        GameObject aPortal = (GameObject)Instantiate(PortalGO);
        aPortal.transform.position = spawnPosition;
        
        portalSpawned = true;
        Debug.Log("Portal spawned na posição: " + spawnPosition);
    }

        Vector2 GetRandomSpawnPosition()
    {
        // Pega os limites da tela
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        float centerY = (min.y + max.y) / 2f;
        float verticalRange = (max.y - min.y) * 0.3f;
        
        // Escolhe esquerda (0) ou direita (1)
        int side = Random.Range(0, 2);
        Vector2 position = Vector2.zero;

        if (side == 0) // Lateral esquerda
        {
            position = new Vector2(
                Random.Range(min.x + 1f, min.x + 2.5f),
                Random.Range(centerY - verticalRange, centerY + verticalRange) 
            );
        }
        else // Lateral direita
        {
            position = new Vector2(
                Random.Range(max.x - 2.5f, max.x - 1f), 
                Random.Range(centerY - verticalRange, centerY + verticalRange) 
            );
        }

        return position;
    }

    // Método para resetar o spawner
    public void ResetSpawner()
    {
        portalSpawned = false;
        gameStartTime = Time.time;
        nextCheckTime = Time.time + spawnCheckInterval;
    }

    // Método para agendar o spawner
    public void SchedulePortalSpawner()
    {
        portalSpawned = false; // Garante que o portal pode spawnar novamente
        gameStartTime = Time.time;
        nextCheckTime = Time.time + spawnCheckInterval;
        enabled = true;
    }

    // Método para desagendar o spawner 
    public void UnschedulePortalSpawner()
    {
        enabled = false;
    }
}