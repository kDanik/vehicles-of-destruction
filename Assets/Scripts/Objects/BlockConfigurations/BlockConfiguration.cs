using UnityEngine;

/// <summary>  
/// Default script that manages configuration of each vehicle block.
/// </summary>
public class BlockConfiguration : MonoBehaviour
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
    /// Checks if joint is allowed (from configuration of block) for provided direction.
    /// The rotation of this block will be considered.
    /// </summary>
    public bool IsJointAllowed(Direction direction)
    {

        Direction realDirection = DirectionUtil.CalculateDirectionAfterRotation(direction, rotation);

        switch (realDirection)
        {
            case Direction.LEFT:
                return allowLeftJoint;
            case Direction.RIGHT:
                return allowRightJoint;
            case Direction.BOTTOM:
                return allowBottomJoint;
            case Direction.TOP:
                return allowTopJoint;
            default:
                Debug.LogError("Invalid direction enum! " + realDirection);

                break;
        }

        return false;
    }

    /// <summary>
    /// Returns gameobject with which (or from which) joint can be made for provided direction.
    /// The rotation of this block will be considered.
    /// </summary>
    public GameObject GetGameobjectForJointCreation(Direction direction)
    {

        Direction realDirection = DirectionUtil.CalculateDirectionAfterRotation(direction, rotation);

        switch (realDirection)
        {
            case Direction.LEFT:
                return GetLeftJointGameobject();
            case Direction.RIGHT:
                return GetRightJointGameobject();
            case Direction.BOTTOM:
                return GetBottomJointGameobject();
            case Direction.TOP:
                return GetTopJointGameobject();
            default:
                Debug.LogError("Invalid direction enum! " + realDirection);

                break;
        }

        return null;
    }

    protected virtual GameObject GetLeftJointGameobject()
    {
        return gameObject;
    }
    protected virtual GameObject GetRightJointGameobject()
    {
        return gameObject;
    }
    protected virtual GameObject GetBottomJointGameobject()
    {
        return gameObject;
    }
    protected virtual GameObject GetTopJointGameobject()
    {
        return gameObject;
    }
}
