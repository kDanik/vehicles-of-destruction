using UnityEngine;

public class Block : MonoBehaviour
{
    public bool allowTopJoint;
    public bool allowLeftJoint;
    public bool allowRightJoint;
    public bool allowBottomJoint;

    public int jointBreakForce;
    public int jointBreakTorque;

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
        bool newAllowTopJoint = allowRightJoint;
        bool newAllowRightJoint = allowBottomJoint;
        bool newAllowBottomJoint = allowLeftJoint;
        bool newAllowLeftJoint = allowTopJoint;

        allowTopJoint = newAllowTopJoint;
        allowBottomJoint = newAllowBottomJoint;
        allowLeftJoint = newAllowLeftJoint;
        allowRightJoint = newAllowRightJoint;
    }
}
