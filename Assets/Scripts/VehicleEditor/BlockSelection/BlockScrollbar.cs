using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockScrollbar : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GraphicRaycaster m_Raycaster;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform canvasRect;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Set the Pointer Event Position to that of the mouse position
        eventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(eventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            BlockScrollbarElement blockScrollbarElement = result.gameObject.transform.parent.GetComponent<BlockScrollbarElement>();

            if (blockScrollbarElement != null) {
                blockScrollbarElement.OnClick();
            }
        }
    }
}
