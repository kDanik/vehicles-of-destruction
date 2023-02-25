using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// When added to gameobject this script will periodicly activate and deactive (blinking effect) specified text component
/// </summary>
public class BlinkingText : MonoBehaviour
{
    [Tooltip("Delay between activated and deactivated states")]
    [SerializeField]
    private float delayBetweenBlinks = 0.5f;

    [Tooltip("Text component that should blink")]
    [SerializeField]
    private TMP_Text text;

    private void Awake()
    {
        StartCoroutine(TextBlink());
    }

    private IEnumerator TextBlink()
    {
        while (true)
        {
            text.enabled = !text.enabled;

            yield return new WaitForSeconds(delayBetweenBlinks);
        }
    }
}
