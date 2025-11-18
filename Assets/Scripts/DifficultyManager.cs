using UnityEngine;

public static class DifficultyManager
{
    public enum Difficulty
    {
        Normal,
        Hard
    }

    public static Difficulty CurrentDifficulty = Difficulty.Normal;
}
