using UnityEngine;

/// <summary>
/// Handles level completion logic (tracking of completed levels and scene state)
/// </summary>
public class LevelCompletion : MonoBehaviour
{
    private SceneStateManager sceneStateManager;

    private void Awake()
    {
        sceneStateManager = GetComponent<SceneStateManager>();
    }

    public void CompleteCurrentLevel()
    {
        LevelCompletionApi.MarkLevelAsCompleted(SceneLoader.GetSelectedLevelIndex());

        sceneStateManager.SwitchToLevelCompletedMode();
    }
}
