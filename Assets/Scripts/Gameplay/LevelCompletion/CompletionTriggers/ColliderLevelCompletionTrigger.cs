using UnityEngine;

public class ColliderLevelCompletionTrigger : AbstractLevelCompletionTrigger
{
    [Tooltip("Which tag should trigger this completion script. No tag means any object will trigger it.")]
    [SerializeField]
    private string triggerTag = ControlBlock.ControlBlockTag;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(triggerTag))
        {
            OnLevelCompletion();
        }
    }
}
