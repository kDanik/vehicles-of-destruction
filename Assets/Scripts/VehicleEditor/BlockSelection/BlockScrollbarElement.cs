using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockScrollbarElement : MonoBehaviour
{
    private int blockIndex;
    private EditorBlockSelection editorBlockSelection;

    [SerializeField]
    private Image blockImage;
    [SerializeField]
    private TextMeshProUGUI blockText;


    public void Initialize(int blockIndex, Sprite blockUISprite, int blockCount, EditorBlockSelection editorBlockSelection)
    {
        this.blockIndex = blockIndex;
        this.editorBlockSelection = editorBlockSelection;

        InitializeBlockUI(blockUISprite);
        UpdateBlockCount(blockCount);
    }


    public void OnClick()
    {
        editorBlockSelection.ChangeSelectedBlockIndex(blockIndex);
    }

    private void InitializeBlockUI(Sprite blockUISprite) {
        blockImage.sprite = blockUISprite;
    }

    public void UpdateBlockCount(int newBlockCount) {
        blockText.text = newBlockCount + "";

        if (newBlockCount == 0)
        {
            blockImage.color = Color.gray;
        }
        else
        {
            blockImage.color = Color.white;
        }
    }
}
