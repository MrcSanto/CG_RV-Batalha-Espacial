using UnityEngine;

public class GameManager : MonoBehaviour
{   
    // referencia aos GOs
    public GameObject playButton;
    public GameObject exitButton;
    public GameObject playerShip;
    public GameObject enemySpawner;
    public GameObject asteroidSpawner;
    public GameObject GameOverGO;
    public GameObject scoreUITextGO;
    public GameObject settingsButton;
    public GameObject pauseButton;
    public GameObject settingsMenu;
    public GameObject pauseMenu;


    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver,
        Pause    
    }

    GameManagerState GMState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GMState = GameManagerState.Opening;   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Se estiver jogando -> Pausa
            if (GMState == GameManagerState.Gameplay)
            {
                PauseGame();
            }
            // Se estiver no pause -> Retoma
            else if (GMState == GameManagerState.Pause)
            {
                ResumeGame();
            }
        }
    }


    void UpdateGameManagerState()
    {
        switch(GMState)
        {
            case GameManagerState.Opening:
                GameOverGO.SetActive(false);
                pauseButton.SetActive(false);

                playButton.SetActive(true);
                exitButton.SetActive(true);
                settingsButton.SetActive(true);

                break;

            case GameManagerState.Pause:
                Time.timeScale = 0f;
                pauseButton.SetActive(false);
                pauseMenu.SetActive(true);

                break;
            
            case GameManagerState.Gameplay:
                scoreUITextGO.GetComponent<ScoreManager>().Score = 0;

                playButton.SetActive(false);
                exitButton.SetActive(false);
                settingsButton.SetActive(false);
                playerShip.GetComponent<PlayerControl>().Init();

                pauseButton.SetActive(true);

                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();
                asteroidSpawner.GetComponent<AsteroidSpawner>().ScheduleAsteroidSpawner();

                break;

            case GameManagerState.GameOver:
                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();

                GameOverGO.SetActive(true);
                pauseButton.SetActive(false);
                pauseMenu.SetActive(false);

                Invoke("ChangeToOpeningState", 8f);
                
                break;
        }
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState ();
    }

    public void StartGamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState ();
    }

    public void PauseGame()
    {
        GMState = GameManagerState.Pause;
        UpdateGameManagerState();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);

        GMState = GameManagerState.Gameplay;
    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;

        scoreUITextGO.GetComponent<ScoreManager>().Score = 0;

        playerShip.GetComponent<PlayerControl>().Init();
        playerShip.SetActive(true);

        DestroyAllEnemies();
        DestroyAllAsteroids();
        DestroyAllBullets();

        // Reinicia spawners
        enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
        asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();

        enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();
        asteroidSpawner.GetComponent<AsteroidSpawner>().ScheduleAsteroidSpawner();

        // Ícones da HUD
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        settingsButton.SetActive(false);
        playButton.SetActive(false);
        exitButton.SetActive(false);
        settingsMenu.SetActive(false);
        GameOverGO.SetActive(false);

        GMState = GameManagerState.Gameplay;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        DestroyAllEnemies();
        DestroyAllAsteroids();
        DestroyAllBullets();

        enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
        asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();

        playerShip.SetActive(false);

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        GameOverGO.SetActive(false);
        pauseButton.SetActive(false);

        // Volta para tela inicial
        GMState = GameManagerState.Opening;
        UpdateGameManagerState();
    }


    public void ChangeToOpeningState()
    {
        SetGameManagerState (GameManagerState.Opening);
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void SetNormalDifficulty()
    {
        DifficultyManager.CurrentDifficulty = DifficultyManager.Difficulty.Normal;
    }

    public void SetHardDifficulty()
    {
        DifficultyManager.CurrentDifficulty = DifficultyManager.Difficulty.Hard;
    }

    void DestroyAllEnemies()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("EnemyShipTag"))
            Destroy(enemy);
    }

    void DestroyAllAsteroids()
    {
        foreach (var asteroid in GameObject.FindGameObjectsWithTag("AsteroidEntityTag"))
            Destroy(asteroid);
    }

    void DestroyAllBullets()
    {
        foreach (var bullet in GameObject.FindGameObjectsWithTag("EnemyBulletTag"))
            Destroy(bullet);

        foreach (var bullet in GameObject.FindGameObjectsWithTag("PlayerBulletTag"))
            Destroy(bullet);
    }

}
