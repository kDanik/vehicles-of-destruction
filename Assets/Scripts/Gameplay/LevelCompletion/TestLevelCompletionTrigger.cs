using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelCompletionTrigger : AbstractLevelCompletionTrigger
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnLevelCompletion();
    }
}
