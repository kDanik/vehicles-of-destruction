using UnityEngine;

public class VehicleEditor : MonoBehaviour
{
    // this will be replaced with block select in future steps
    public GameObject selectedObject;
    [SerializeField]
    private GameObject blockWrapper;

    [SerializeField]
    private GameObject editorCell;
    [SerializeField]
    private int editorWidth;
    [SerializeField]
    private int editorHeight;



    private Vector2Int focusedCellPosition;
    private EditorProcessor editorProcessor;

    private EditorGrid editorGrid;


    void Start()
    {
        editorProcessor = gameObject.AddComponent<EditorProcessor>();
        editorGrid = gameObject.AddComponent<EditorGrid>();

        editorGrid.InitiliazeEditorGrid(new Vector2Int(editorWidth, editorHeight), editorCell, blockWrapper);
    }

    public void EditorCellMouseDown(Vector2Int positionInEditorGrid)
    {
        if (!editorGrid.IsBlockPresent(positionInEditorGrid.x, positionInEditorGrid.y)) {
            editorGrid.AddNewBlock(positionInEditorGrid.x, positionInEditorGrid.y, selectedObject);
        } else
        {
            // focus on block (rotate, delete and etc menu) -- ?
        }
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
        }
    }

    public void OnDragOutsideOfGrid(Vector2Int positionInEditorGrid)
    {
        editorGrid.DeleteBlock(positionInEditorGrid.x, positionInEditorGrid.y);
    }

    public void DeactivateEditor()
    {
        gameObject.SetActive(false);
    }

    public void ActivateEditor()
    {
        gameObject.SetActive(true);
    }

    public void ProcessEditorGrid()
    {
        editorProcessor.ProcessEditorGrid(editorGrid);
    }
}
