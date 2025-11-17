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

                playButton.SetActive(true);
                exitButton.SetActive(true);

                break;
            
            case GameManagerState.Gameplay:
                playButton.SetActive(false);
                exitButton.SetActive(false);
                playerShip.GetComponent<PlayerControl>().Init();

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
}
