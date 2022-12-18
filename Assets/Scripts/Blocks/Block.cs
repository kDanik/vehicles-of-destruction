using UnityEngine;

/// <summary>  
/// Default script that manages configuration of each vehicle block.
/// </summary>
public class Block : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Can block be attached to the top of this block")]
    private bool allowTopJoint;
    [SerializeField]
    [Tooltip("Can block be attached to the left side of this block")]
    private bool allowLeftJoint;

    [SerializeField]
    [Tooltip("Can block be attached to the right side of this block")]
    private bool allowRightJoint;

    [SerializeField]
    [Tooltip("Can block be attached to the bottom of this block")]
    private bool allowBottomJoint;

    [Tooltip("Maximum force that can be applied to this blocks joints. Check EditorProcessor.cs and Joints documentation")]
    public int JointBreakForce = 600;
    [Tooltip("Maximum torque that can be applied to this blocks joints. Check EditorProcessor.cs and Joints documentation")]
    public int JointBreakTorque = 600;

    [Tooltip("Block type name used by block selection to manage block count, should ALWAYS be set and be unique.")]
    public string BlockTypeName;

    [Tooltip("Sprite that is used to draw UI components related to this block. For multiple sprite blocks use one sprite that combines all sprites")]
    public Sprite BlockUISprite;

    [SerializeField, HideInInspector]
    private int rotation = 0;

    /// <summary>
    /// Rotates block by 90 degrees to the left direction and updates its allowed joints configuration
    /// </summary>
    public void Rotate()
    {
        transform.Rotate(0, 0, 90);

        // rotation variable is used to handle allowed joint configuration
        if (rotation + 90 == 360)
        {
            rotation = 0;
        }
        else
        {
            rotation += 90;
        }
    }


    /// <summary>
    /// Checks if this block allows joint from the left side (counting current rotation)
    /// </summary>
    public bool IsLeftJointAllowed()
    {
        if (rotation == 90)
        {
            return allowTopJoint;
        }
        else if (rotation == 180)
        {
            return allowRightJoint;
        }
        else if (rotation == 270)
        {
            return allowBottomJoint;
        }

        return allowLeftJoint;
    }

    /// <summary>
    /// Checks if this block allows joint from the right side (counting current rotation)
    /// </summary>
    public bool IsRightJointAllowed()
    {
        if (rotation == 90)
        {
            return allowBottomJoint;
        }
        else if (rotation == 180)
        {
            return allowLeftJoint;
        }
        else if (rotation == 270)
        {
            return allowTopJoint;
        }

        return allowRightJoint;
    }

    /// <summary>
    /// Checks if this block allows joint from the top side (counting current rotation)
    /// </summary>
    public bool IsTopJointAllowed()
    {
        if (rotation == 90)
        {
            return allowRightJoint;
        }
        else if (rotation == 180)
        {
            return allowBottomJoint;
        }
        else if (rotation == 270)
        {
            return allowLeftJoint;
        }

        return allowTopJoint;
    }

    /// <summary>
    /// Checks if this block allows joint from the bottom side (counting current rotation)
    /// </summary>
    public bool IsBottomJointAllowed()
    {
        if (rotation == 90)
        {
            return allowLeftJoint;
        }
        else if (rotation == 180)
        {
            return allowTopJoint;
        }
        else if (rotation == 270)
        {
            return allowRightJoint;
        }

        return allowBottomJoint;
    }

}
