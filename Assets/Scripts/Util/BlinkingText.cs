using System.Collections;
using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    [SerializeField]
    private float delayBetweenBlinks = 0.5f ;

    [SerializeField]
    private TMP_Text text;

    private void Awake()
    {
        StartCoroutine(GameOverBlink());
    }

    private IEnumerator GameOverBlink()
    {
        while(true)
        {
            text.enabled = !text.enabled;

            yield return new WaitForSeconds(delayBetweenBlinks);
        }
    }
}
