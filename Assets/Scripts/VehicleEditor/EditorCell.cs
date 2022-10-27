using UnityEngine;

public class EditorCell : MonoBehaviour
{
    public Vector2Int positionInEditorGrid;
    private VehicleEditor vehicleEditor;    

    public void Initialize(Vector2Int positionInEditorGrid, VehicleEditor vehicleEditor)
    {
        this.positionInEditorGrid = positionInEditorGrid;
        this.vehicleEditor = vehicleEditor;
    }

    private void OnMouseEnter()
    {
  //      editorGrid.EditorCellMouseEnter(positionInEditorGrid);
    }

    private void OnMouseDown()
    {
        vehicleEditor.EditorCellMouseDown(positionInEditorGrid);
    }

    public void OnBlockDraggedAction(GameObject block, Vector2Int positionInGridBeforeDrag)
    {
        vehicleEditor.OnBlockDraggedAction(block, positionInEditorGrid, positionInGridBeforeDrag);
    }

    public void OnDragOutsideOfGrid()
    {
        vehicleEditor.OnDragOutsideOfGrid(positionInEditorGrid);
    }
}
