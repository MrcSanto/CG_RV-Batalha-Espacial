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
    public GameObject menuButton;
    public GameObject settingsMenu;


    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver
    }

    GameManagerState GMState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GMState = GameManagerState.Opening;   
    }

    void UpdateGameManagerState()
    {
        switch(GMState)
        {
            case GameManagerState.Opening:
                GameOverGO.SetActive(false);
                menuButton.SetActive(false);

                playButton.SetActive(true);
                exitButton.SetActive(true);
                settingsButton.SetActive(true);

                break;
            
            case GameManagerState.Gameplay:
                scoreUITextGO.GetComponent<ScoreManager>().Score = 0;

                playButton.SetActive(false);
                exitButton.SetActive(false);
                settingsButton.SetActive(false);
                playerShip.GetComponent<PlayerControl>().Init();

                menuButton.SetActive(true);

                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();
                asteroidSpawner.GetComponent<AsteroidSpawner>().ScheduleAsteroidSpawner();

                break;

            case GameManagerState.GameOver:
                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                asteroidSpawner.GetComponent<AsteroidSpawner>().UnscheduleAsteroidSpawner();

                GameOverGO.SetActive(true);

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
}
