using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ControlBlock : MonoBehaviour
{

    [SerializeField]
    private GameObject lightsContainer;

    [SerializeField]
    private GameObject screen;

    [SerializeField]
    private TMP_Text screenText;

    private Rigidbody2D rigidbodyBuffer;

    private void Awake()
    {
        rigidbodyBuffer = GetComponent<Rigidbody2D>();

        StartCoroutine(UpdateScreenText());
        StartCoroutine(UpdateLights());
    }

    private IEnumerator UpdateScreenText()
    {
        int velocityUnitsPerMin = (int)Math.Round(rigidbodyBuffer.velocity.magnitude);
        if (velocityUnitsPerMin > 99) velocityUnitsPerMin = 99;

        screenText.text = velocityUnitsPerMin + "";

        yield return new WaitForSeconds(0.2f);
        StartCoroutine(UpdateScreenText());
    }

    private IEnumerator UpdateLights()
    {
        int childIndex = UnityEngine.Random.Range(0, lightsContainer.transform.childCount);

        Light2D light = lightsContainer.transform.GetChild(childIndex).GetComponent<Light2D>();

        if (light.intensity == 0f)
        {
            StartCoroutine(LightsUtil.FadeLightOn(light, 1f, 0.05f));
        }
        else
        {
            StartCoroutine(LightsUtil.FadeLightOff(light, 0.2f));
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdateLights());
    }
}
