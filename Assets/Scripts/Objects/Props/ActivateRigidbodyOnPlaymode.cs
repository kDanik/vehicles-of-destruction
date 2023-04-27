using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRigidbodyOnPlaymode : MonoBehaviour, IPlaymodeStartListener
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.simulated = false;
    }

    public void OnPlaymodeStart()
    {
        rb.simulated = true;
    }
}
