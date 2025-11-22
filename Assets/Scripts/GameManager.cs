using UnityEngine;

public class GameManager : MonoBehaviour
{   
    // referencia aos GOs
    public GameTimer gameTimer;
    public GameObject timerUI;
    public GameObject playButton;
    public GameObject exitButton;
    public GameObject playerShip;
    public GameObject enemySpawner;
    public GameObject asteroidSpawner;
    public GameObject GameOverGO;
    public GameObject YouWinGO;
    public GameObject scoreUITextGO;
    public GameObject settingsButton;
    public GameObject pauseButton;
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public GameObject rankingButton;
    public GameObject rankingMenu;
    public GameObject healthBar;
    public GameObject gameTitle;
    public GameObject enemyCountUI;

    [Header("Ranking")]
    public RankingManager rankingManager;

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver,
        Pause,
        Victory
    }

    GameManagerState GMState;

    void Start()
    {
        GMState = GameManagerState.Opening;   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GMState == GameManagerState.Gameplay)
            {
                PauseGame();
            }
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
                playerShip.SetActive(false);
                GameOverGO.SetActive(false);
                YouWinGO.SetActive(false);
                pauseButton.SetActive(false);
                healthBar.SetActive(false);
                enemyCountUI.SetActive(false);

                playButton.SetActive(true);
                exitButton.SetActive(true);
                settingsButton.SetActive(true);
                rankingButton.SetActive(true);
                gameTitle.SetActive(true);

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
                rankingButton.SetActive(false);
                gameTitle.SetActive(false);
                playerShip.GetComponent<PlayerControl>().Init();

                pauseButton.SetActive(true);
                healthBar.SetActive(true);
                enemyCountUI.SetActive(true);

                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();
                asteroidSpawner.GetComponent<AsteroidSpawner>().ScheduleAsteroidSpawner();
                gameTimer.StartTimer();

                break;

            case GameManagerState.GameOver:
                SaveCurrentScoreToRanking();
                EnemyControl.ResetEnemiesDestroyed(); 

                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();

                GameOverGO.SetActive(true);
                pauseButton.SetActive(false);
                playerShip.SetActive(false);
                pauseMenu.SetActive(false);
                enemyCountUI.SetActive(false);

                Invoke("ChangeToOpeningState", 4f);
                gameTimer.StopTimer();
                
                break;

            case GameManagerState.Victory:
                
                SaveCurrentScoreToRanking();
                EnemyControl.ResetEnemiesDestroyed(); 

                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();

                YouWinGO.SetActive(true);
                pauseButton.SetActive(false);
                playerShip.SetActive(false);
                pauseMenu.SetActive(false);
                enemyCountUI.SetActive(false);

                Invoke("ChangeToOpeningState", 6f);
                gameTimer.StopTimer();
                
                break;
        }
    }

    void SaveCurrentScoreToRanking()
    {
        if (rankingManager != null)
        {
            int currentScore = scoreUITextGO.GetComponent<ScoreManager>().Score;
            rankingManager.AddScore(currentScore);
        }
        else
        {
            Debug.LogWarning("RankingManager não está atribuído no GameManager!");
        }
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    public void StartGamePlay()
    {
        EnemyControl.ResetEnemiesDestroyed();

        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
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

        gameTimer.ResetTimer();
        gameTimer.StartTimer();

        scoreUITextGO.GetComponent<ScoreManager>().Score = 0;
        EnemyControl.ResetEnemiesDestroyed(); 

        playerShip.GetComponent<PlayerControl>().Init();
        playerShip.SetActive(true);

        DestroyAllEnemies();
        DestroyAllAsteroids();
        DestroyAllBullets();

        enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
        asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();

        enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();
        asteroidSpawner.GetComponent<AsteroidSpawner>().ScheduleAsteroidSpawner();

        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        settingsButton.SetActive(false);
        playButton.SetActive(false);
        exitButton.SetActive(false);
        settingsMenu.SetActive(false);
        GameOverGO.SetActive(false);
        YouWinGO.SetActive(false);

        GMState = GameManagerState.Gameplay;
    }

    public void ReturnToMainMenu()
    {
        gameTimer.ResetTimer();

        EnemyControl.ResetEnemiesDestroyed(); 

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
        YouWinGO.SetActive(false);
        pauseButton.SetActive(false);

        GMState = GameManagerState.Opening;
        UpdateGameManagerState();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Fechando o jogo...");
    }

    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void OpenRankingMenu()
    {
        rankingMenu.SetActive(true);
    }

    public void CloseRankingMenu()
    {
        rankingMenu.SetActive(false);
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