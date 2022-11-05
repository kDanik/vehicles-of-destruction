using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is attached to each scrollbar element in block scrollbar and controls selection of block
/// </summary>
public class BlockScrollbarElement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("References image component of current scrollbar element. Don't change it in prefab!")]
    private Image blockImage;

    [SerializeField]
    [Tooltip("References text component of current scrollbar element. Don't change it in prefab!")]
    private TextMeshProUGUI blockText;

    // index of current scrollbar element block in EditorBlockSelection.editorBlocks 
    private int blockIndex;

    private EditorBlockSelection editorBlockSelection;

    public void Initialize(int blockIndex, Sprite blockUISprite, int blockCount, EditorBlockSelection editorBlockSelection)
    {
        this.blockIndex = blockIndex;
        this.editorBlockSelection = editorBlockSelection;

        SetBlockSprite(blockUISprite);
        UpdateBlockCount(blockCount);
    }

    /// <summary>
    /// On click action of BlockScrollbarElement.
    /// Updates selected block in EditorBlockSelection with this elements blockIndex.
    /// </summary>
    public void OnClick()
    {
        editorBlockSelection.ChangeSelectedBlockIndex(blockIndex);
    }

    /// <summary>
    /// Updates block count text (and visual effect of sprite) of this BlockScrollbarElement
    /// </summary>
    /// <param name="newBlockCount"></param>
    public void UpdateBlockCount(int newBlockCount)
    {
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

    private void SetBlockSprite(Sprite blockUISprite)
    {
        blockImage.sprite = blockUISprite;
        blockImage.preserveAspect = true;
    }
}
