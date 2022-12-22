using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBlock : Block
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
