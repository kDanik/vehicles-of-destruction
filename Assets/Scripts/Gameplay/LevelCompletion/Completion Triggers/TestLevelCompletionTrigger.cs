using UnityEngine;

public class TestLevelCompletionTrigger : AbstractLevelCompletionTrigger
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ControlBlock")) {
            OnLevelCompletion();
        }
    }
}
