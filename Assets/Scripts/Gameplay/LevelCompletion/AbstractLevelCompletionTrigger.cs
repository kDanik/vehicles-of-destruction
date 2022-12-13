using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLevelCompletionTrigger : MonoBehaviour
{
    [SerializeField] private LevelCompletion levelCompletion;

    public void OnLevelCompletion() {
        levelCompletion.CompleteCurrentLevel();

        Destroy(gameObject);
    }
}
