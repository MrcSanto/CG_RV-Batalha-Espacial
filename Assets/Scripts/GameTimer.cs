using UnityEngine;
using TMPro;   // IMPORTANTE

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public GameObject timerUI;

    private float elapsedTime;
    private bool isRunning = false;

    void Start()
    {
        // Começa invisível
        timerUI.SetActive(false);
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

            timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        timerUI.SetActive(true);   // aparece
    }

    public void StopTimer()
    {
        isRunning = false;
        timerUI.SetActive(false);  // desaparece
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerText.text = "00:00.000";
        timerUI.SetActive(false);  // também esconde
    }

    public float GetTime()
    {
        return elapsedTime;
    }
}
