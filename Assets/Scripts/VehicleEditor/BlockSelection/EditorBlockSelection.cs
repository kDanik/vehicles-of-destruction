using System;
using UnityEngine;


/// <summary>
/// This script manages block that can be used in VehicleEditor, their count and their selection.
/// It also generates UI for block selection. See BlockScrollbarElement and BlockScrollbar.
/// </summary>
public class EditorBlockSelection : MonoBehaviour
{
    [Serializable]
    private struct EditorBlock
    {
        public GameObject block;
        public int count;

        public EditorBlock(GameObject block, int count) : this()
        {
            this.block = block;
            this.count = count;
        }
    }

    [SerializeField]
    [Tooltip("Content gameobject of block scroll bar")]
    private GameObject scrollBarContent;

    [SerializeField]
    [Tooltip("Configuration of which and how many blocks can be used in current scene")]
    private EditorBlock[] editorBlocks;

    [SerializeField]
    [Tooltip("UI element of block in block scroll bar. See related prefab for more information")]
    private GameObject baseBlockUIPrefab;

    private BlockScrollbarElement[] editorBlockUIElements;

    private int selectedBlockIndex;


    private void Start()
    {
        editorBlockUIElements = new BlockScrollbarElement[editorBlocks.Length];
        GenerateBlockScrollBar();
    }

    /// <summary>
    /// Action that should be triggered on block being deleted from EditorGrid.
    /// Updates editorBlocks count for removed block.
    /// </summary>
    /// <param name="removedBlock">Block that was removed from editor grid (or its prefab)</param>
    public void OnRemoveBlockFromEditorGrid(GameObject removedBlock)
    {
        for (int i = 0; i < editorBlocks.Length; i++)
        {
            if (IsOfSamePrefabType(editorBlocks[i].block, removedBlock))
            {
                editorBlocks[i] = new EditorBlock(editorBlocks[i].block, editorBlocks[i].count + 1);
                editorBlockUIElements[i].UpdateBlockCount(editorBlocks[i].count);

                return;
            }
        }
    }

    /// <summary>
    /// Action that should be triggered on block being added to EditorGrid.
    /// Updates editorBlocks count for new block.
    /// </summary>
    /// <param name="removedBlock">Block that was added to editor grid (or its prefab)</param>
    public void OnAddBlockToEditorGrid(GameObject addedBlock)
    {
        for (int i = 0; i < editorBlocks.Length; i++)
        {
            if (IsOfSamePrefabType(editorBlocks[i].block, addedBlock))
            {
                editorBlocks[i] = new EditorBlock(editorBlocks[i].block, editorBlocks[i].count - 1);
                editorBlockUIElements[i].UpdateBlockCount(editorBlocks[i].count);

                return;
            }
        }
    }

    /// <summary>
    /// Checks if selected block can be spawned = has block count more than 0
    /// </summary>
    public bool CanSelectedBlockBeSpawned()
    {
        return editorBlocks[selectedBlockIndex].count > 0;
    }

    /// <summary>
    /// Get prefab of currently selected block
    /// </summary>
    public GameObject GetSelectedBlock()
    {
        return editorBlocks[selectedBlockIndex].block;
    }

    /// <summary>
    /// Updates index of currently selected block
    /// </summary>
    public void ChangeSelectedBlockIndex(int newSelectedBlockIndex)
    {
        selectedBlockIndex = newSelectedBlockIndex;
    }

    private void GenerateBlockScrollBar()
    {
        for (int i = 0; i < editorBlocks.Length; i++)
        {
            EditorBlock editorBlock = editorBlocks[i];

            AddBlockToScrollBar(i, editorBlock);
        }
    }

    private void AddBlockToScrollBar(int index, EditorBlock editorBlock)
    {
        Sprite blockUISprite = editorBlock.block.GetComponent<Block>().BlockUISprite;

        GameObject newScrollbarBlock = Instantiate(baseBlockUIPrefab, scrollBarContent.transform, false);

        BlockScrollbarElement blockScrollbarElement = newScrollbarBlock.GetComponent<BlockScrollbarElement>();
        blockScrollbarElement.Initialize(index, blockUISprite, editorBlock.count, GetComponent<EditorBlockSelection>());

        editorBlockUIElements[index] = blockScrollbarElement;
    }

    private bool IsOfSamePrefabType(GameObject block1, GameObject block2)
    {
        return block1.GetComponent<Block>().BlockTypeName.Equals(block2.GetComponent<Block>().BlockTypeName);
    }
}
