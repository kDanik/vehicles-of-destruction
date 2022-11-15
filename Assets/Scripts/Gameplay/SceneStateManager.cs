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

    private GameObject dynamicObjectsBuffer;

    private VehicleEditor editor;

    public void Start()
    {
        editor = gameObject.GetComponent<VehicleEditor>();
        dynamicObjectsBuffer = Instantiate(dynamicObjects);
        dynamicObjectsBuffer.SetActive(false);

        sceneState = SceneState.EDITOR;
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
    }

    /// <summary>
    /// Switches game state to playmode by deactivating editor and generating vehicle
    /// </summary>
    public void SwitchToPlayMode()
    {
        sceneState = SceneState.PLAYMODE;

        editor.ProcessEditorGrid();
        editor.DeactivateEditor();
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

    public static bool IsEditorMode()
    {
        return sceneState == SceneState.EDITOR;
    }

    public static bool IsPlaymode()
    {
        return sceneState == SceneState.PLAYMODE;
    }
}
