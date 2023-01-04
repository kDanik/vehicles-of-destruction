using UnityEngine;

public class SpringBlockConfiguration : BlockConfiguration
{
    [SerializeField]
    private GameObject leftJointGameObject;

    [SerializeField]
    private GameObject rightJointGameObject;

    protected override GameObject GetLeftJointGameobject()
    {
        return leftJointGameObject;
    }
    protected override GameObject GetRightJointGameobject()
    {
        return rightJointGameObject;
    }
}
