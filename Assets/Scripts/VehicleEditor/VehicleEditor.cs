using UnityEngine;

public class VehicleEditor : MonoBehaviour
{
    [SerializeField]
    private GameObject blockWrapper;

    [SerializeField]
    private GameObject editorCell;
    [SerializeField]
    private int editorWidth;
    [SerializeField]
    private int editorHeight;

    private bool isFocused = false;
    private Vector2Int focusPositionInGrid;

    private EditorProcessor editorProcessor;

    private EditorGrid editorGrid;

    private EditorBlockSelection editorBlockSelection;


    void Start()
    {
        editorProcessor = gameObject.AddComponent<EditorProcessor>();
        editorGrid = gameObject.AddComponent<EditorGrid>();
        editorBlockSelection = GetComponent<EditorBlockSelection>();

        editorGrid.InitiliazeEditorGrid(new Vector2Int(editorWidth, editorHeight), editorCell, blockWrapper, editorBlockSelection);
    }

    public void EditorCellMouseDown(Vector2Int positionInEditorGrid)
    {
        if (!editorGrid.IsBlockPresent(positionInEditorGrid.x, positionInEditorGrid.y) && editorBlockSelection.CanSelectedBlockBeSpawned())
        {
            editorGrid.AddNewBlock(positionInEditorGrid.x, positionInEditorGrid.y, editorBlockSelection.GetSelectedBlock());
        }

        ChangeFocus(positionInEditorGrid);
    }

    public void OnBlockDraggedAction(GameObject block, Vector2Int newPositionInEditorGrid, Vector2Int positionInGridBeforeDrag)
    {
        // if new position is busy drag action is failed
        if (editorGrid.IsBlockPresent(newPositionInEditorGrid.x, newPositionInEditorGrid.y))
        {
            block.GetComponent<EditorBlock>().DragFail();
        }
        else
        {
            editorGrid.MoveBlock(positionInGridBeforeDrag.x, positionInGridBeforeDrag.y, newPositionInEditorGrid.x, newPositionInEditorGrid.y);
            ChangeFocus(newPositionInEditorGrid);
        }
    }

    public void OnDragOutsideOfGrid(Vector2Int positionInEditorGrid)
    {
        editorGrid.DeleteBlock(positionInEditorGrid.x, positionInEditorGrid.y);

        if (focusPositionInGrid.Equals(positionInEditorGrid)) ResetFocus();
    }

    public void FocusOnCell(Vector2Int positionInEditorGrid)
    {
        ChangeFocus(positionInEditorGrid);
    }

    public void RotateFocusedCell()
    {
        if (isFocused && editorGrid.IsBlockPresent(focusPositionInGrid.x, focusPositionInGrid.y))
        {
            editorGrid.RotateBlock(focusPositionInGrid.x, focusPositionInGrid.y);
        }
    }

    public void DeactivateEditor()
    {
        gameObject.SetActive(false);
    }

    public void ActivateEditor()
    {
        gameObject.SetActive(true);
        ResetFocus();
    }

    public void ProcessEditorGrid()
    {
        editorProcessor.ProcessEditorGrid(editorGrid);
    }

    private void ChangeFocus(Vector2Int newFocusPositionInGrid)
    {
        // if not focused than there is no point in changing focus state of object
        if (isFocused)
        {
            GameObject from = editorGrid.GetBlock(focusPositionInGrid.x, focusPositionInGrid.y);
            if (from != null) from.GetComponent<EditorBlock>().MakeUnfocused();
        }

        GameObject to = editorGrid.GetBlock(newFocusPositionInGrid.x, newFocusPositionInGrid.y);

        if (to != null) to.GetComponent<EditorBlock>().MakeFocused();

        isFocused = true;
        focusPositionInGrid = newFocusPositionInGrid;
    }

    private void ResetFocus()
    {
        GameObject currentFocus = editorGrid.GetBlock(focusPositionInGrid.x, focusPositionInGrid.y);

        if (currentFocus != null) currentFocus.GetComponent<EditorBlock>().MakeUnfocused();

        isFocused = false;
        focusPositionInGrid = new Vector2Int(-1, -1);
    }
}
