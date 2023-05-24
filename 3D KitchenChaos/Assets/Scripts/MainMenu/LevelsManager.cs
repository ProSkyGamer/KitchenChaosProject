using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelsManager
{
    private const string LEVELS_UNCLOCKED_PLAYER_PREFS = "CurrentUnlockedLevel";
    private const string CURRENT_LEVEL_PLAYER_PREFS = "CurrentLevel";
    private static LevelsSO currentLevelSO;


    public static bool IsLevelUnlocked(int levelNumber)
    {
        return PlayerPrefs.GetInt(LEVELS_UNCLOCKED_PLAYER_PREFS, 0) >= levelNumber;
    }

    public static void UnlockLevel(int toUnlock)
    {
        if (PlayerPrefs.GetInt(LEVELS_UNCLOCKED_PLAYER_PREFS, 0) < toUnlock)
        {
            PlayerPrefs.SetInt(LEVELS_UNCLOCKED_PLAYER_PREFS, toUnlock);
        }
    }

    public static void ResetReachedLevels()
    {
        PlayerPrefs.SetInt(LEVELS_UNCLOCKED_PLAYER_PREFS, 0);
    }

    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(CURRENT_LEVEL_PLAYER_PREFS, 0);
    }

    public static void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(CURRENT_LEVEL_PLAYER_PREFS, level);
    }

    public static int GetReachedLevel()
    {
        return PlayerPrefs.GetInt(LEVELS_UNCLOCKED_PLAYER_PREFS, 0);
    }

    public static LevelsSO GetCurrentLevelSO()
    {
        return currentLevelSO;
    }

    public static void SetLevelSO(LevelsSO levelSO)
    {
        currentLevelSO = levelSO;
    }
}
