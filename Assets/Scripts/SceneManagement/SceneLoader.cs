using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main class for handling of switching, loading scenes such as menu, levels and etc.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Tooltip("Scriptable object that contains information about all levels")]
    [SerializeField] private LevelsData levelsData;
    private LevelLoader levelLoader;

    private static int currentlySelectedLevelIndex = UNSELECTED_LEVEL_INDEX;
    private static readonly int UNSELECTED_LEVEL_INDEX = -1;


    private void Awake()
    {
        levelLoader = new LevelLoader(levelsData);
    }


    /// <summary>
    /// Loads scene of Level with corresponding levelIndex (see LevelsData)
    /// </summary>
    public void LoadLevelScene(int levelIndex)
    {
        currentlySelectedLevelIndex = levelIndex;
        levelLoader.LoadLevelByIndex(levelIndex);
    }

    /// <summary>
    /// Loads main menu scene
    /// </summary>
    public void LoadMainMenuScene()
    {
        currentlySelectedLevelIndex = UNSELECTED_LEVEL_INDEX;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    /// <summary>
    /// Returns index of currently selected (loaded) level. If no level selected will return UNSELECTED_LEVEL_INDEX.
    /// </summary>
    public static int GetSelectedLevelIndex()
    {
        return currentlySelectedLevelIndex;
    }

    /// <summary>
    /// Checks if level scene is currently loaded (selected)
    /// </summary>
    /// <returns>true if level scene is loaded, in any other case false</returns>
    public static bool IsAnyLevelSelected()
    {
        return UNSELECTED_LEVEL_INDEX != currentlySelectedLevelIndex;
    }
}
