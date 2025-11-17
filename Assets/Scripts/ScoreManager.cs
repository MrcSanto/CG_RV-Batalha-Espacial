using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    Text scoreTextUI;
    int score;
    public int Score
    {
        get
        {
            return this.score;
        }
        set
        {
            this.score = value;
            UpdateScoreTextUI();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreTextUI = GetComponent<Text> ();
    }

    void UpdateScoreTextUI()
    {
        string scoreStr = string.Format("{0:000000}", score);
        scoreTextUI.text = scoreStr;
    }
}

