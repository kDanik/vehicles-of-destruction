using UnityEngine;

/// <summary>
/// This script is attached to block wrapper, that helps to separate editor logic from type of blocks, their colliders and configuration.
/// Each block has same wrapper attached to it while in editor mode.
/// </summary>
public class EditorBlock : MonoBehaviour
{
    private Vector2 positionDragBuffer;
    private EditorCell cellBeforeDrag;

    private bool isDragged = false;
    private float waitTimeBeforeDrag = 0.2f;
    private float dragTimer = 0;

    private static Color colorWhenFocused = new(0, 0, 0, 0.15f);

    void OnMouseDown()
    {
        // buffer initial position for cases when element drag is unsuccessful
        positionDragBuffer = transform.position;

        EditorCell editorCell = FindEditorCell();
        if (editorCell != null)
        {
            cellBeforeDrag = FindEditorCell();
            editorCell.FocusOnCell();
        }
    }

    void OnMouseDrag()
    {
        if (!isDragged)
        {
            dragTimer += Time.deltaTime;

            if (dragTimer > waitTimeBeforeDrag) isDragged = true;
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos;
        }
    }

    void OnMouseUp()
    {
        if (isDragged) OnDragStop();

        isDragged = false;
        dragTimer = 0;
    }

    private void OnDragStop()
    {
        EditorCell editorCell = FindEditorCell();

        if (editorCell == null)
        {
            cellBeforeDrag.OnDragOutsideOfGrid();
        }
        else
        {
            FindEditorCell().OnBlockDraggedAction(gameObject, cellBeforeDrag.PositionInEditorGrid);
        }
    }
    /// <summary>
    /// Changes appearance of EditorBlock to focused state
    /// </summary>
    public void MakeFocused()
    {
        GetComponent<SpriteRenderer>().color = colorWhenFocused;
    }

    /// <summary>
    /// Changes appearance of EditorBlock to unfocused (default) state
    /// </summary>
    public void MakeUnfocused()
    {
        GetComponent<SpriteRenderer>().color = Color.clear;
    }

    public void DragFail()
    {
        transform.position = positionDragBuffer;
    }

    /// <summary>
    /// Find editor cell that mouse(or finger input) currently hovers over
    /// </summary>
    /// <returns>EditorCell of editor cell or null if none found</returns>
    private EditorCell FindEditorCell()
    {
        int layerMask = LayerMask.GetMask("Editor Cell");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, layerMask);

        if (hit.collider != null)
        {
            EditorCell editorCell = hit.collider.gameObject.GetComponent<EditorCell>();

            return editorCell;
        }

        return null;
    }
}
