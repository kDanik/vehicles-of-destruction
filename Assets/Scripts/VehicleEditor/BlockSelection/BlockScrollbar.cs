using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This script should be attached to block scroll bar and is used to detect click on scroll bar items (Blocks).
/// (Unity UI doesn't allow that without some tricks)
/// </summary>
public class BlockScrollbar : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    [Tooltip("use GraphicRaycaster of Canvas. Used for raycasting in UI elements")]
    private GraphicRaycaster m_Raycaster;
    [SerializeField]
    [Tooltip("use EventSystem of EventSystem GameObject. Used for raycasting in UI elements")]
    private EventSystem m_EventSystem;
    [SerializeField]
    [Tooltip("use RectTransform of Canvas. Used for raycasting in UI elements")]
    private RectTransform canvasRect;

    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> results = new();

        // Set the Pointer Event Position to that of the mouse position (also should work for finger touch input, but should be tested)
        eventData.position = Input.mousePosition;

        m_Raycaster.Raycast(eventData, results);

        // For every result returned, search for BlockScrollbarElement and trigger on click 
        foreach (RaycastResult result in results)
        {
            BlockScrollbarElement blockScrollbarElement = result.gameObject.transform.parent.GetComponent<BlockScrollbarElement>();

            if (blockScrollbarElement != null)
            {
                blockScrollbarElement.OnClick();
            }
        }
    }
}
