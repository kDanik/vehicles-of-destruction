using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


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
    private GameObject scrollBarContent;

    [SerializeField]
    private EditorBlock[] editorBlocks;

    private BlockScrollbarElement[] editorBlockUIElements;

    [SerializeField]
    private GameObject baseBlockUIPrefab;

    private int selectedBlockIndex;


    private void Start()
    {
        editorBlockUIElements = new BlockScrollbarElement[editorBlocks.Length];
        GenerateBlockScrollBar();
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
        Sprite blockUISprite = editorBlock.block.GetComponent<Block>().blockUISprite;

        GameObject newScrollbarBlock = Instantiate(baseBlockUIPrefab, scrollBarContent.transform, false);

        BlockScrollbarElement blockScrollbarElement = newScrollbarBlock.GetComponent<BlockScrollbarElement>();
        blockScrollbarElement.Initialize(index, blockUISprite, editorBlock.count, GetComponent<EditorBlockSelection>());

        editorBlockUIElements[index] = blockScrollbarElement;
    }

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


    public bool CanSelectedBlockBeSpawned()
    {
        return editorBlocks[selectedBlockIndex].count > 0;
    }

    public GameObject GetSelectedBlock()
    {
        return editorBlocks[selectedBlockIndex].block;
    }

    public void ChangeSelectedBlockIndex(int newSelectedBlockIndex)
    {
        selectedBlockIndex = newSelectedBlockIndex;
    }


    private bool IsOfSamePrefabType(GameObject block1, GameObject block2)
    {
        return block1.GetComponent<Block>().blockTypeName.Equals(block2.GetComponent<Block>().blockTypeName);
    }
}
