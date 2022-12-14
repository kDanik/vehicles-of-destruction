using UnityEngine;

/// <summary>
/// Abstract class for any level completion trigger.
/// Exact way OnLevelCompletion() will be triggered should be specified in child class.
/// </summary>
public abstract class AbstractLevelCompletionTrigger : MonoBehaviour
{
    [Tooltip("Reference to script that handle level completion logic")]
    [SerializeField] private LevelCompletion levelCompletion;

    /// <summary>
    /// Action that should be triggered on level completion
    /// </summary>
    public void OnLevelCompletion()
    {
        levelCompletion.CompleteCurrentLevel();

        Destroy(gameObject);
    }
}
