using UnityEngine;

/// <summary>  
/// Manages configuration of block type.
/// Should be attached to prefab of each block.
/// </summary>
public class Block : MonoBehaviour
{
    [Tooltip("Can block be attached to the top of this block")]
    public bool AllowTopJoint;
    [Tooltip("Can block be attached to the left side of this block")]
    public bool AllowLeftJoint;
    [Tooltip("Can block be attached to the right side of this block")]
    public bool AllowRightJoint;
    [Tooltip("Can block be attached to the bottom of this block")]
    public bool AllowBottomJoint;

    [Tooltip("Maximum force that connected to this block joints can have. Check EditorProcessor.cs and Joints documentation")]
    public int JointBreakForce = 600;
    [Tooltip("Maximum torque that connected to this block joints can have. Check EditorProcessor.cs and Joints documentation")]
    public int JointBreakTorque = 600;

    [Tooltip("Block type name used to detect which type of block prefab is block object, should ALWAYS be set and be unique.")]
    public string BlockTypeName;

    [Tooltip("Sprite that is used to draw UI components related to this block. For multiple sprite blocks use one sprite that combines all sprites")]
    public Sprite BlockUISprite;

    /// <summary>
    /// Rotates block by 90 degrees to the left direction and updates its allowed joints configuration
    /// </summary>
    public void Rotate()
    {
        transform.Rotate(0, 0, 90);

        RotateAllowedJoints();
    }

    /// <summary>
    /// Rotates allowed joints so their global direction stays the same.
    /// </summary>
    private void RotateAllowedJoints()
    {
        bool newAllowTopJoint = AllowRightJoint;
        bool newAllowRightJoint = AllowBottomJoint;
        bool newAllowBottomJoint = AllowLeftJoint;
        bool newAllowLeftJoint = AllowTopJoint;

        AllowTopJoint = newAllowTopJoint;
        AllowBottomJoint = newAllowBottomJoint;
        AllowLeftJoint = newAllowLeftJoint;
        AllowRightJoint = newAllowRightJoint;
    }
}
