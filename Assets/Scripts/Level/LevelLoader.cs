using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages loading of Level Scenes (see LevelData ScriptableObject and Level object)
/// </summary>
public class LevelLoader
{
    private readonly LevelsData levelsData;

    public LevelLoader(LevelsData levelsData)
    {
        this.levelsData = levelsData;
    }

    /// <summary>
    /// Loads scene for level with given index in levelsData. If index is invalid does nothing.
    /// </summary>
    public void LoadLevelByIndex(int levelIndex)
    {
        Level level = levelsData.FindLevelByLevelIndex(levelIndex);

        if (!level.isEmpty && Application.CanStreamedLevelBeLoaded(level.SceneName))
        {
            SceneManager.LoadScene(level.SceneName);
        }
        else
        {
            Debug.LogError("Level with given index can't be loaded! " + levelIndex);
        }
    }
}
