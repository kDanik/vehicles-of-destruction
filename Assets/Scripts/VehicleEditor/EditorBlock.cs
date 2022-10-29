using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorBlock : MonoBehaviour
{
    private static Color focusedColor = new(0, 0, 0, 0.15f);
    private Vector2 positionDragBuffer;
    private EditorCell cellBeforeDrag;

    private bool isDragged = false;
    private float waitTimeBeforeDrag = 0.2f;
    private float dragTimer = 0;


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


    public void MakeFocused()
    {
        GetComponent<SpriteRenderer>().color = focusedColor;
    }

    public void MakeUnfocused()
    {
        GetComponent<SpriteRenderer>().color = Color.clear;
    }

    public void DragFail()
    {
        transform.position = positionDragBuffer;
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
