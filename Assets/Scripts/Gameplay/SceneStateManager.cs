using Cinemachine;
using UnityEngine;

/// <summary>
/// Controls state of the scene(vehicle editor --- playtime).
/// Togles UI, resets dynamic objects, generates vehicle and etc.
/// </summary>
public class SceneStateManager : MonoBehaviour
{
    private static SceneState sceneState;

    [SerializeField]
    [Tooltip("All dynamic scene objects should be child objects of this dynamic object!")]
    private GameObject dynamicObjects;

    [SerializeField]
    [Tooltip("Cinemachine camera for this scene")]
    private CinemachineVirtualCamera virtualCamera;

    private GameObject dynamicObjectsBuffer;

    private VehicleEditor editor;

    private CameraController cameraController;

    public void Start()
    {
        sceneState = SceneState.EDITOR;

        editor = gameObject.GetComponent<VehicleEditor>();
        dynamicObjectsBuffer = Instantiate(dynamicObjects);
        dynamicObjectsBuffer.SetActive(false);

        cameraController = gameObject.AddComponent<CameraController>();
        cameraController.Initialize(virtualCamera);
    }

    /// <summary>
    /// Switches game state to editor by activating editor, destroying vehicle and reloading dynamic objects
    /// </summary>
    public void SwitchToEditorMode()
    {
        sceneState = SceneState.EDITOR;

        ReloadDynamicObjects();
        DestroyVehicle();
        editor.ActivateEditor();
        cameraController.SwitchCameraToEditorMode();
    }

    /// <summary>
    /// Switches game state to playmode by deactivating editor and generating vehicle
    /// </summary>
    public void SwitchToPlayMode()
    {
        sceneState = SceneState.PLAYMODE;

        editor.ProcessEditorGrid();
        editor.DeactivateEditor();
        cameraController.SwitchCameraToPlayMode(GameObject.Find("VehicleParent"));
    }

    /// <summary>
    /// Deletes current dynamicObjects GameObjects and replaces it with dynamicObjectsBuffer
    /// </summary>
    private void ReloadDynamicObjects()
    {
        dynamicObjectsBuffer.transform.parent = dynamicObjects.transform.parent;

        Destroy(dynamicObjects);

        dynamicObjects = dynamicObjectsBuffer;
        dynamicObjects.SetActive(true);

        dynamicObjectsBuffer = Instantiate(dynamicObjects);
        dynamicObjectsBuffer.SetActive(false);
    }

    /// <summary>
    /// Destroys vehicle by deleting parent object (VehicleParent)
    /// </summary>
    private void DestroyVehicle()
    {
        Destroy(GameObject.Find("VehicleParent"));
    }

    /// <summary>
    /// Check if current game/scene state is editor mode
    /// </summary>
    /// <returns>true if in editor mode, otherwise false</returns>
    public static bool IsEditorMode()
    {
        return sceneState == SceneState.EDITOR;
    }

    /// <summary>
    /// Check if current game/scene state is playmode mode
    /// </summary>
    /// <returns>true if in playmode mode, otherwise false</returns>
    public static bool IsPlaymode()
    {
        return sceneState == SceneState.PLAYMODE;
    }
}
