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

    private int selectedBlockIndex = -1;


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
        int index = FindIndexByPrefabType(removedBlock);

        editorBlocks[index] = new EditorBlock(editorBlocks[index].block, editorBlocks[index].count + 1);
        editorBlockUIElements[index].UpdateBlockCount(editorBlocks[index].count);
    }

    /// <summary>
    /// Action that should be triggered on block being added to EditorGrid.
    /// Updates editorBlocks count for new block.
    /// </summary>
    /// <param name="removedBlock">Block that was added to editor grid (or its prefab)</param>
    public void OnAddBlockToEditorGrid(GameObject addedBlock)
    {
        int index = FindIndexByPrefabType(addedBlock);

        editorBlocks[index] = new EditorBlock(editorBlocks[index].block, editorBlocks[index].count - 1);
        editorBlockUIElements[index].UpdateBlockCount(editorBlocks[index].count);
    }

    /// <summary>
    /// Checks if selected block can be spawned = has block count more than 0
    /// </summary>
    public bool CanSelectedBlockBeSpawned()
    {
        if (selectedBlockIndex < 0) return false;

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
    /// Updates currently selected block
    /// </summary>
    public void ChangeSelectedBlockIndex(int newSelectedBlockIndex)
    {
        if (selectedBlockIndex != -1)
        {
            editorBlockUIElements[selectedBlockIndex].DeactivateSelectedAppereance();
        }
        editorBlockUIElements[newSelectedBlockIndex].ActivateSelectedAppereance();

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
        Sprite blockUISprite = editorBlock.block.GetComponent<BlockConfiguration>().BlockUISprite;

        GameObject newScrollbarBlock = Instantiate(baseBlockUIPrefab, scrollBarContent.transform, false);

        BlockScrollbarElement blockScrollbarElement = newScrollbarBlock.GetComponent<BlockScrollbarElement>();
        blockScrollbarElement.Initialize(index, blockUISprite, editorBlock.count, GetComponent<EditorBlockSelection>());

        editorBlockUIElements[index] = blockScrollbarElement;
    }

    private bool IsOfSamePrefabType(GameObject block1, GameObject block2)
    {
        return block1.GetComponent<BlockConfiguration>().BlockTypeName.Equals(block2.GetComponent<BlockConfiguration>().BlockTypeName);
    }

    private int FindIndexByPrefabType(GameObject block)
    {
        for (int i = 0; i < editorBlocks.Length; i++)
        {
            if (IsOfSamePrefabType(editorBlocks[i].block, block))
            {
                return i;
            }
        }

        return -1;
    }
}
