using UnityEngine;

/// <summary>
/// This class process editor grid and generates vehicle from it
/// </summary>
public class EditorProcessor : MonoBehaviour
{
    /// <summary>
    /// Create vehicle using given blueprint(placedBlocksGrid).
    /// Setups all joint, rigidbody and etc.
    /// </summary>
    /// <param name="placedBlocksGrid">VehicleEditor blueprint from which vehicle should be created</param>
    public void ProcessEditorGrid(EditorGrid editorGrid)
    {
        GameObject[,] placedBlocksGrid = CreateCopyOfEditorBlueprint(editorGrid);

        GameObject vehicleParent = new("VehicleParent");

        for (int y = 0; y < editorGrid.GetGridHeight(); y++)
        {
            for (int x = 0; x < editorGrid.GetGridWidth(); x++)
            {
                if (placedBlocksGrid[x, y] != null)
                {
                    SetupVehicleBlock(x, y, placedBlocksGrid, editorGrid, vehicleParent);
                }
            }
        }
    }

    private void SetupVehicleBlock(int x, int y, GameObject[,] placedBlocksGrid, EditorGrid editorGrid, GameObject vehicleParent)
    {
        GameObject block = placedBlocksGrid[x, y];
        block.transform.parent = vehicleParent.transform;

        SetupBlockRigidbodies(block);

        if (x != editorGrid.GetGridWidth() - 1 && IsJointInRightDirectionAllowed(block, placedBlocksGrid[x + 1, y]))
        {
            CreateJoint(block, placedBlocksGrid[x + 1, y], Direction.RIGHT);
        }
        if (y != editorGrid.GetGridHeight() - 1 && IsJointInTopDirectionAllowed(block, placedBlocksGrid[x, y + 1]))
        {
            CreateJoint(block, placedBlocksGrid[x, y + 1], Direction.TOP);
        }

    }

    private void SetupBlockRigidbodies(GameObject block)
    {
        SetupRigidbody(block);

        // activate also for child objects for more complex blocks

        foreach (Transform child in block.transform)
        {
            SetupRigidbody(child.gameObject);
        }

    }

    private void SetupRigidbody(GameObject block)
    {
        Rigidbody2D rigidbody2D = block.GetComponent<Rigidbody2D>();

        if (rigidbody2D != null) rigidbody2D.simulated = true;
    }

    /// <summary>
    /// Creates copy of placedBlocksGrid by instantiating gameobjects from it and populating same size array
    /// </summary>
    /// <param name="placedBlocksGrid">Editor "blueprint" of vehicle</param>
    /// <returns>copy of placedBlocksGrid</returns>
    private GameObject[,] CreateCopyOfEditorBlueprint(EditorGrid editorGrid)
    {
        GameObject[,] copy = new GameObject[editorGrid.GetGridWidth(), editorGrid.GetGridHeight()];

        for (int y = 0; y < editorGrid.GetGridHeight(); y++)
        {
            for (int x = 0; x < editorGrid.GetGridWidth(); x++)
            {
                if (editorGrid.IsCellEmpty(x, y)) continue;

                GameObject gameObjectToCopy = editorGrid.GetBlockWithoutWrapper(x, y);
                copy[x, y] = Instantiate(gameObjectToCopy, gameObjectToCopy.transform.position, gameObjectToCopy.transform.rotation);
            }
        }

        return copy;
    }

    /// <summary>
    /// Creates and configures joint between block1 and block2
    /// </summary>
    private void CreateJoint(GameObject block1, GameObject block2, Direction direction)
    {
        BlockConfiguration blockScript1 = block1.GetComponent<BlockConfiguration>();
        BlockConfiguration blockScript2 = block2.GetComponent<BlockConfiguration>();

        GameObject gameObjectContainingJoint = blockScript1.GetGameobjectForJointCreation(direction);
        GameObject gameObjectJointedTo = blockScript2.GetGameobjectForJointCreation(DirectionUtil.GetOpositeDirection(direction));

        FixedJoint2D joint = gameObjectContainingJoint.AddComponent<FixedJoint2D>();

        joint.connectedBody = gameObjectJointedTo.GetComponent<Rigidbody2D>();
        joint.enableCollision = true;

        SetJointStrength(joint, blockScript1, blockScript2);
    }

    /// <summary>
    /// Calculates and set joint torque and force break strength.
    /// The strength for joint will be average of joint strength of both blocks.
    /// </summary>
    private void SetJointStrength(Joint2D joint, BlockConfiguration blockScript1, BlockConfiguration blockScript2)
    {
        int minForce, maxForce, maxTorque, minTorque;
        if (blockScript1.JointBreakForce > blockScript2.JointBreakForce)
        {
            minForce = blockScript2.JointBreakForce;
            maxForce = blockScript1.JointBreakForce;
        }
        else {
            minForce = blockScript1.JointBreakForce;
            maxForce = blockScript2.JointBreakForce;
        }

        if (blockScript1.JointBreakTorque > blockScript2.JointBreakTorque)
        {
            minTorque = blockScript2.JointBreakTorque;
            maxTorque = blockScript1.JointBreakTorque;
        }
        else
        {
            minTorque = blockScript1.JointBreakTorque;
            maxTorque = blockScript2.JointBreakTorque;
        }

        joint.breakForce = maxForce - ((maxForce - minForce) / 2);


        joint.breakTorque = maxTorque - ((maxTorque - minTorque) / 2);
    }

    /// <summary>
    /// Joint between 2 blocks is allowed if both blocks allow joints for their respective directions.
    /// Here it means - block1 allows top joint and block2 allows bottom joint.
    /// </summary>
    /// <param name="block1">current block from which joint will be created</param>
    /// <param name="block2">block that is on top of current block, with thich joint will be created</param>
    /// <returns>true if joint can created, otherwise false</returns>
    private bool IsJointInTopDirectionAllowed(GameObject block1, GameObject block2)
    {
        if (block1 == null || block2 == null) return false;

        return block1.GetComponent<BlockConfiguration>().IsJointAllowed(Direction.TOP) && block2.GetComponent<BlockConfiguration>().IsJointAllowed(Direction.BOTTOM);
    }

    /// <summary>
    /// Joint between 2 blocks is allowed if both blocks allow joints for their respective directions.
    /// Here it means - block1 allows right joint and block2 allows left joint.
    /// </summary>
    /// <param name="block1">current block from which joint will be created</param>
    /// <param name="block2">block that is on right of current block, with thich joint will be created</param>
    /// <returns>true if joint can created, otherwise false</returns>
    private bool IsJointInRightDirectionAllowed(GameObject block1, GameObject block2)
    {
        if (block1 == null || block2 == null) return false;

        return block1.GetComponent<BlockConfiguration>().IsJointAllowed(Direction.RIGHT) && block2.GetComponent<BlockConfiguration>().IsJointAllowed(Direction.LEFT);
    }
}