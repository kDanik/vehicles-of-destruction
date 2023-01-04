using UnityEngine;

public class Wheel : MonoBehaviour, IPlaymodeStartListener
{
    [SerializeField]
    [Tooltip("Wheel object(round thingy) of wheel base")]
    private GameObject wheel;

    private void OnJointBreak2D(Joint2D joint)
    {
        if (joint is WheelJoint2D)
        {
            // when wheel joint breaks wheel should be removed from children of this wheel base
            wheel.transform.parent = transform.parent;
        }
    }

    void IPlaymodeStartListener.OnPlaymodeStart()
    {
        Debug.Log("hi");
    }
}
