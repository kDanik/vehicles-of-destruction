using UnityEngine;

public class LevelCompletion : MonoBehaviour
{
    private SceneStateManager sceneStateManager;

    private void Awake()
    {
        sceneStateManager = GetComponent<SceneStateManager>();
    }

    public void CompleteCurrentLevel()
    {
        LevelCompletionApi.MarkLevelAsCompleted(SceneLoader.GetSelectedLevelId());

        sceneStateManager.SwitchToLevelCompletedMode();
    }
}
