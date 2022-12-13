using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private LevelsData levelsData;
    private LevelLoader levelLoader;

    private static int currentlySelectedLevelId = UNSELECTED_LEVEL_ID;
    private static readonly int UNSELECTED_LEVEL_ID = -1;


    private void Awake()
    {
        levelLoader = new LevelLoader(levelsData);   
    }


    public void LoadLevelScene(int levelId)
    {
        currentlySelectedLevelId = levelId;
        levelLoader.LoadLevelById(levelId);
    }

    public void LoadMainMenuScene()
    {
        currentlySelectedLevelId = UNSELECTED_LEVEL_ID;
        SceneManager.LoadSceneAsync("MainMenu");
    }


    public static int GetSelectedLevelId()
    {
        return currentlySelectedLevelId;
    }

    public static bool IsAnyLevelSelected()
    {
        return UNSELECTED_LEVEL_ID != currentlySelectedLevelId;
    }
}
