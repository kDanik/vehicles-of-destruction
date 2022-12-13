using UnityEngine;

/// <summary>
/// Manages writes and reads for level completion data (Stored in PlayerPrefs)
/// </summary>
public class LevelCompletionApi
{
    private static readonly string LEVEL_PREFIX = "Level - ";

    /// <summary>
    /// Marks level as completed by adding it to PlayerPrefs
    /// </summary>
    /// <param name="levelId">id of Level that should be marked as completed</param>
    public void MarkLevelAsCompleted(int levelId)
    {
        PlayerPrefs.SetInt(AssembleLevelPrefsKey(levelId), 1); // value is not important, only key

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Checks if level is completed (in PlayerPrefs)
    /// </summary>
    /// <param name="levelId">id of Level that should be checked</param>
    /// <returns>true if there is entry in PlayerPrefs for given level, otherwise false</returns>
    public bool IsLevelCompleted(int levelId)
    {
        return PlayerPrefs.HasKey(AssembleLevelPrefsKey(levelId));
    }

    /// <summary>
    /// Resets all saved preferences for levels from levelsData to default (not completed) value.
    /// This probably only should be used for development 
    /// </summary>
    /// <param name="levelsData"></param>
    public void ResetLevelPrefs(LevelsData levelsData)
    {
        Debug.LogWarning("ResetLevelPrefs used to reset completed level in playerpref. This should only be used during development, and not in actual build!");

        foreach (Level level in levelsData.GetAllLevels())
        {
            PlayerPrefs.DeleteKey(AssembleLevelPrefsKey(level.LevelId));
        }

        PlayerPrefs.Save();
    }

    private string AssembleLevelPrefsKey(int levelId)
    {
        // when changing generation method of preference key, old keys (old versions) should be also updated
        return LEVEL_PREFIX + levelId;
    }
}
