using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

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
    /// Switches camera focus to playmode = on vehicle, by creating target group from all blocks
    /// </summary>
    /// <param name="vehicleParent">parent object of vehicle</param>
    public void SwitchCameraToPlayMode(GameObject vehicleParent)
    {
        editorPositionBuffer = cinemachineVirtualCamera.transform.position;

        targetGroup = cinemachineVirtualCamera.gameObject.AddComponent<CinemachineTargetGroup>();

        foreach (Transform child in vehicleParent.transform)
        {
            targetGroup.AddMember(child, 1, 1);
            targetObjects.Add(child);
        }

        cinemachineVirtualCamera.m_Lens.OrthographicSize = 9;
    }
}
