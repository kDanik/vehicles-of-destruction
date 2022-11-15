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
                if (placedBlocksGrid[x, y] == null) continue;

                GameObject block = placedBlocksGrid[x, y];
                block.transform.parent = vehicleParent.transform;

                SetupRigidbody(block);

                if (x != editorGrid.GetGridWidth() - 1 && IsJointInRightDirectionAllowed(block, placedBlocksGrid[x + 1, y]))
                {
                    CreateJoint(block, placedBlocksGrid[x + 1, y]);
                }
                if (y != editorGrid.GetGridHeight() - 1 && IsJointInTopDirectionAllowed(block, placedBlocksGrid[x, y + 1]))
                {
                    CreateJoint(block, placedBlocksGrid[x, y + 1]);
                }
            }
        }
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

    private void SetupRigidbody(GameObject block)
    {
        block.GetComponent<Rigidbody2D>().simulated = true;

        // activate also for child objects for more complex blocks
        if (block.transform.childCount != 0)
        {

            foreach (Transform child in block.transform)
            {
                Rigidbody2D childRigidbody2D = child.gameObject.GetComponent<Rigidbody2D>();

                if (childRigidbody2D != null) childRigidbody2D.simulated = true;
            }
        }

    }

    /// <summary>
    /// Creates and configures joint between block1 and block2
    /// </summary>
    /// <param name="block1"></param>
    /// <param name="block2"></param>
    private void CreateJoint(GameObject block1, GameObject block2)
    {
        FixedJoint2D joint = block1.AddComponent<FixedJoint2D>();

        joint.connectedBody = block2.GetComponent<Rigidbody2D>();
        joint.enableCollision = true;

        SetJointStrength(joint, block1, block2);
    }

    /// <summary>
    /// Calculates and set joint torque and force break strength.
    /// The strength for joint will be taken from the block with lowest strength of two.
    /// </summary>
    private void SetJointStrength(Joint2D joint, GameObject block1, GameObject block2)
    {
        Block blockScript1 = block1.GetComponent<Block>();
        Block blockScript2 = block2.GetComponent<Block>();

        // use minimum joint break force / torque from 2 blocks. (TODO Maybe average strength would be more logical in future)
        joint.breakForce = blockScript1.JointBreakForce > blockScript2.JointBreakForce ? blockScript2.JointBreakForce : blockScript1.JointBreakForce;
        joint.breakTorque = blockScript1.JointBreakTorque > blockScript2.JointBreakTorque ? blockScript2.JointBreakTorque : blockScript1.JointBreakTorque;
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

        return block1.GetComponent<Block>().AllowTopJoint && block2.GetComponent<Block>().AllowBottomJoint;
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

        return block1.GetComponent<Block>().AllowRightJoint && block2.GetComponent<Block>().AllowLeftJoint;
    }
}