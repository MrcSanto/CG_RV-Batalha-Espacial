using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    [Header("Ranking UI")]
    public Text textRank1;
    public Text textRank2;
    public Text textRank3;
    public Text textRank4;

    private const string RANKING_KEY = "GameRanking";
    private List<int> topScores = new List<int>();

    void Start()
    {
        LoadRanking();
        UpdateRankingUI();
    }

    // Chame este método quando o jogo terminar para adicionar o score
    public void AddScore(int newScore)
    {
        topScores.Add(newScore);
        topScores = topScores.OrderByDescending(s => s).Take(4).ToList();
        SaveRanking();
        UpdateRankingUI();
    }

    void UpdateRankingUI()
    {
        // Atualiza os textos do ranking
        textRank1.text = topScores.Count > 0 ? FormatScore(topScores[0]) : "No Data";
        textRank2.text = topScores.Count > 1 ? FormatScore(topScores[1]) : "No Data";
        textRank3.text = topScores.Count > 2 ? FormatScore(topScores[2]) : "No Data";
        textRank4.text = topScores.Count > 3 ? FormatScore(topScores[3]) : "No Data";
    }

    string FormatScore(int score)
    {
        return string.Format("{0:000000}", score);
    }

    void SaveRanking()
    {
        // Salva os scores como string separada por vírgulas
        string scoresStr = string.Join(",", topScores);
        PlayerPrefs.SetString(RANKING_KEY, scoresStr);
        PlayerPrefs.Save();
    }

    void LoadRanking()
    {
        topScores.Clear();
        
        if (PlayerPrefs.HasKey(RANKING_KEY))
        {
            string scoresStr = PlayerPrefs.GetString(RANKING_KEY);
            string[] scoreArray = scoresStr.Split(',');
            
            foreach (string s in scoreArray)
            {
                if (int.TryParse(s, out int score))
                {
                    topScores.Add(score);
                }
            }
        }
    }

    // Método opcional para resetar o ranking
    public void ResetRanking()
    {
        topScores.Clear();
        PlayerPrefs.DeleteKey(RANKING_KEY);
        UpdateRankingUI();
    }
}