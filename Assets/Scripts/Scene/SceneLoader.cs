using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private LevelsData levelsData;
    private LevelLoader levelLoader;

    private static int currentlySelectedLevelIndex = UNSELECTED_LEVEL_INDEX;
    private static readonly int UNSELECTED_LEVEL_INDEX = -1;


    private void Awake()
    {
        levelLoader = new LevelLoader(levelsData);
    }


    public void LoadLevelScene(int levelIndex)
    {
        currentlySelectedLevelIndex = levelIndex;
        levelLoader.LoadLevelByIndex(levelIndex);
    }

    public void LoadMainMenuScene()
    {
        currentlySelectedLevelIndex = UNSELECTED_LEVEL_INDEX;
        SceneManager.LoadSceneAsync("MainMenu");
    }


    public static int GetSelectedLevelId()
    {
        return currentlySelectedLevelIndex;
    }

    public static bool IsAnyLevelSelected()
    {
        return UNSELECTED_LEVEL_INDEX != currentlySelectedLevelIndex;
    }
}
