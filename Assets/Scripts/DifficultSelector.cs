using UnityEngine;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    public Button normalButton;
    public Button hardButton;

    public Color selectedColor = Color.yellow;
    public Color unselectedColor = Color.white;

    public enum Difficulty { Normal, Hard }
    public Difficulty currentDifficulty = Difficulty.Normal;

    void Start()
    {
        UpdateButtonColors();
    }

    public void SelectNormal()
    {
        currentDifficulty = Difficulty.Normal;
        UpdateButtonColors();
    }

    public void SelectHard()
    {
        currentDifficulty = Difficulty.Hard;
        UpdateButtonColors();
    }

    void UpdateButtonColors()
    {
        // cor do Normal
        normalButton.image.color =
            currentDifficulty == Difficulty.Normal ? selectedColor : unselectedColor;

        // cor do Hard
        hardButton.image.color =
            currentDifficulty == Difficulty.Hard ? selectedColor : unselectedColor;
    }
}
