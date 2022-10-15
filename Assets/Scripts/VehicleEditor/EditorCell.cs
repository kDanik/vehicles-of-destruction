using UnityEngine;

public class EditorCell : MonoBehaviour
{
    private Vector2Int positionInEditorGrid;
    private VehicleEditor vehicleEditor;    

    public void Initialize(Vector2Int positionInEditorGrid, VehicleEditor vehicleEditor)
    {
        this.positionInEditorGrid = positionInEditorGrid;
        this.vehicleEditor = vehicleEditor;
    }

    private void OnMouseEnter()
    {
        vehicleEditor.EditorCellMouseEnter(positionInEditorGrid);
    }

    private void OnMouseDown()
    {
        vehicleEditor.EditorCellMouseDown();
    }
}
