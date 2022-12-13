using UnityEngine;

public class LevelCompletion : MonoBehaviour
{
    private SceneStateManager sceneStateManager;
    private LevelCompletionApi levelCompletionApi;

    private void Awake()
    {
        sceneStateManager = GetComponent<SceneStateManager>();
        levelCompletionApi = new LevelCompletionApi();
    }

    public void CompleteCurrentLevel()
    {
        levelCompletionApi.MarkLevelAsCompleted(SceneLoader.GetSelectedLevelId());

        sceneStateManager.SwitchToLevelCompletedMode();
    }
}
