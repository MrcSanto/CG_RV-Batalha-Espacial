using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public GameObject timerUI;
    private GameObject gameManagerGO;

    private float elapsedTime;
    private bool isRunning = false;

    private const float WARNING_TIME = 120f;
    private const float CRITICAL_TIME = 150f;
    private const float MAX_TIME = 180f;

    private Color normalColor = Color.white;
    private Color warningColor = Color.red;

    private float blinkTimer = 0f;
    private float blinkInterval = 0.5f;
    private bool isBlinking = false;

    void Start()
    {
        timerUI.SetActive(false);
        gameManagerGO = GameObject.FindGameObjectWithTag("GameManagerTag");
    }

    void Update()
    {
        if (!isRunning)
            return;

        elapsedTime += Time.deltaTime;

        UpdateTimerDisplay();
        CheckTimeWarnings();

        if (elapsedTime >= MAX_TIME)
        {
            TimeOut();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

        timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

    void CheckTimeWarnings()
    {
        if (elapsedTime >= CRITICAL_TIME)
        {
            if (!isBlinking)
            {
                isBlinking = true;
                blinkTimer = 0f;
            }

            BlinkRed();
        }
        else if (elapsedTime >= WARNING_TIME)
        {
            timerText.color = warningColor;
            isBlinking = false;
        }
        else
        {
            timerText.color = normalColor;
            isBlinking = false;
        }
    }

    void BlinkRed()
    {
        blinkTimer += Time.deltaTime;

        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;

            timerText.color = 
                timerText.color == warningColor ? 
                normalColor : 
                warningColor;
        }
    }

    void TimeOut()
    {
        isRunning = false;

        if (gameManagerGO != null)
        {
            GameManager gm = gameManagerGO.GetComponent<GameManager>();
            gm?.SetGameManagerState(GameManager.GameManagerState.GameOver);
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        timerUI.SetActive(true);

        isBlinking = false;
        blinkTimer = 0f;

        timerText.color = normalColor;
    }

    public void StopTimer()
    {
        isRunning = false;
        timerUI.SetActive(false);
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = false;

        isBlinking = false;
        blinkTimer = 0f;

        if (timerText != null)
            timerText.color = normalColor;

        timerText.text = "00:00.000";
        timerUI.SetActive(false);
    }

    public float GetTime() => elapsedTime;
}
