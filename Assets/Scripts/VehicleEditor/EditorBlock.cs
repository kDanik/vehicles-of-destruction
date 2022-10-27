using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorBlock : MonoBehaviour
{
    private Vector2 positionDragBuffer;
    private EditorCell cellBeforeDrag;

    private bool isDragged = false;


    void OnMouseDown()
    {
        // buffer initial position for cases when element drag is unsuccessful
        positionDragBuffer = transform.position;

        EditorCell editorCell = FindEditorCell();
        if (editorCell != null)
        {
            cellBeforeDrag = FindEditorCell();
        }
    }

    void OnMouseDrag()
    {
        // TODO maybe add delay to drag?

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;

        isDragged = true;
    }

    void OnMouseUp()
    {
        if (isDragged) OnDragStop();

        isDragged = false;
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
            FindEditorCell().OnBlockDraggedAction(gameObject, cellBeforeDrag.positionInEditorGrid);
        }
    }

    public void DragFail()
    {
        transform.position = positionDragBuffer;
    }

    public EditorCell FindEditorCell()
    {
        int layerMask = LayerMask.GetMask("Editor Cell");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10, layerMask);

        if (hit.collider != null)
        {
            EditorCell editorCell = hit.collider.gameObject.GetComponent<EditorCell>();
            Debug.Log(hit.collider.gameObject.name);
            return editorCell;
        }

        return null;
    }
}
