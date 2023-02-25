using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

/// <summary>
/// This class is used to allow camera folowing of vehicle as target group of blocks.
/// As currently only one control block should be followed, this is absolute and can be changed.
/// </summary>
public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTargetGroup targetGroup;

    private List<Transform> targetObjects;
    private Vector3 editorPositionBuffer;

    public void Initialize(CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        this.cinemachineVirtualCamera = cinemachineVirtualCamera;
        editorPositionBuffer = transform.position;
        targetObjects = new List<Transform>();
    }

    /// <summary>
    /// Switches camera to editor mode = removes focus from vehicle and focuses on vehicle editor
    /// </summary>
    public void SwitchCameraToEditorMode()
    {
        DestroyImmediate(targetGroup);
        targetObjects.Clear();

        cinemachineVirtualCamera.transform.position = editorPositionBuffer;

        cinemachineVirtualCamera.m_Lens.OrthographicSize = 5;
    }

    /// <summary>
    /// Switches camera focus to playmode = on vehicle control block
    /// </summary>
    public void SwitchCameraToPlayMode(GameObject vehicleParent)
    {

        editorPositionBuffer = cinemachineVirtualCamera.transform.position;

        targetGroup = cinemachineVirtualCamera.gameObject.AddComponent<CinemachineTargetGroup>();

        foreach (Transform child in vehicleParent.transform)
        {
            if (child.CompareTag("ControlBlock"))
            {
                targetGroup.AddMember(child, 1, 1);
                targetObjects.Add(child);

                break;
            }
        }

        cinemachineVirtualCamera.m_Lens.OrthographicSize = 9;
    }
}
