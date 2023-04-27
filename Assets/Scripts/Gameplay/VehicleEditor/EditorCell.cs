using UnityEngine;

/// <summary>
/// This script is attached to each cell of editorGrid and helps to control logic of EditorGrid.
/// It mostly helps vehicleEditor understand at which coordinates / position in grid some editing action occures.
/// 
/// It also serves as only reference to vehicleEditor from editor grid objects, because other editor grid helper object doesn't have access to it.
/// </summary>
public class EditorCell : MonoBehaviour
{
    /// <summary>
    /// Position in editor grid array. See EditorGrid.
    /// </summary>
    public Vector2Int PositionInEditorGrid;
    private VehicleEditor vehicleEditor;

    public void Initialize(Vector2Int positionInEditorGrid, VehicleEditor vehicleEditor)
    {
        PositionInEditorGrid = positionInEditorGrid;
        this.vehicleEditor = vehicleEditor;
    }

    private void OnMouseDown()
    {
        vehicleEditor.EditorCellMouseDown(PositionInEditorGrid);
    }

    public void OnBlockDraggedAction(GameObject block, Vector2Int positionInGridBeforeDrag)
    {
        vehicleEditor.OnBlockDraggedAction(block, PositionInEditorGrid, positionInGridBeforeDrag);
    }

    public void OnDragOutsideOfGrid()
    {
        vehicleEditor.OnDragOutsideOfGrid(PositionInEditorGrid);
    }

    public void FocusOnCell()
    {
        vehicleEditor.FocusOnCell(PositionInEditorGrid);
    }
}
