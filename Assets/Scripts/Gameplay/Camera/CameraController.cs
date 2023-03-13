using System.Collections;
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
    private CinemachineVirtualCamera cinemachineVirtualLevelOverviewCamera;

    private CinemachineTargetGroup targetGroup;

    private List<Transform> targetObjects;
    private VehicleEditor vehicleEditor;

    public void Initialize(CinemachineVirtualCamera cinemachineVirtualCamera, CinemachineVirtualCamera cinemachineVirtualLevelOverviewCamera, VehicleEditor vehicleEditor)
    {
        this.cinemachineVirtualCamera = cinemachineVirtualCamera;
        this.cinemachineVirtualLevelOverviewCamera = cinemachineVirtualLevelOverviewCamera;

        this.vehicleEditor = vehicleEditor;
        targetObjects = new List<Transform>();
    }


    public IEnumerator PlayLevelOverview()
    {
        cinemachineVirtualCamera.enabled = false;
        cinemachineVirtualLevelOverviewCamera.enabled = true;

        Animator animator = cinemachineVirtualLevelOverviewCamera.GetComponent<Animator>();

        animator.Play("LevelOverview");

        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    public void OnLevelOverviewEnd()
    {
        cinemachineVirtualCamera.enabled = true;
        cinemachineVirtualLevelOverviewCamera.enabled = false;
    }

    /// <summary>
    /// Switches camera to editor mode = removes focus from vehicle and focuses on vehicle editor
    /// </summary>
    public void SwitchCameraToEditorMode()
    {
        DestroyImmediate(targetGroup);
        targetObjects.Clear();

        Vector3 centerOFEditor = vehicleEditor.GetCenterOfEditorGrid();
        centerOFEditor.z = -10;

        cinemachineVirtualCamera.transform.position = centerOFEditor;

        cinemachineVirtualCamera.m_Lens.OrthographicSize = 5;
    }

    /// <summary>
    /// Switches camera focus to playmode = on vehicle control block
    /// </summary>
    public void SwitchCameraToPlayMode(GameObject vehicleParent)
    {
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
