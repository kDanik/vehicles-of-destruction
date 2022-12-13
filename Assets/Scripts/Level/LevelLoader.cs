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
    /// Loads scene for level with given id. If id is invalid does nothing.
    /// </summary>
    public void LoadLevelById(int levelId)
    {
        Level level = levelsData.FindLevelByLevelId(levelId);

        if (!level.isEmpty && Application.CanStreamedLevelBeLoaded(level.SceneName))
        {
            SceneManager.LoadScene(level.SceneName);
        }
        else
        {
            Debug.LogError("Level with given id can't be loaded! " + levelId);
        }
    }
}
