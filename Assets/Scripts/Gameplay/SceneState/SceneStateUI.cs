using UnityEngine;

public class SceneStateUI
{
    private readonly GameObject[] editorStateUI;
    private readonly GameObject[] levelCompletedStateUI;
    private readonly GameObject[] playmodeStateUI;

    public SceneStateUI(GameObject[] editorStateUI, GameObject[] levelCompletedStateUI, GameObject[] playmodeStateUI)
    {
        this.editorStateUI = editorStateUI;
        this.levelCompletedStateUI = levelCompletedStateUI;
        this.playmodeStateUI = playmodeStateUI;
    }

    public void DeactivateEditorStateUI()
    {
        ChangeActiveState(editorStateUI, false);
    }

    public void DeactivateLevelCompletedStateUI()
    {
        ChangeActiveState(levelCompletedStateUI, false);
    }

    public void DeactivatePlaymodeStateUI()
    {
        ChangeActiveState(playmodeStateUI, false);
    }

    public void ActivateEditorStateUI()
    {
        ChangeActiveState(editorStateUI, true);
    }

    public void ActivateLevelCompletedStateUI()
    {
        ChangeActiveState(levelCompletedStateUI, true);
    }

    public void ActivatePlaymodeStateUI()
    {
        ChangeActiveState(playmodeStateUI, true);
    }

    private void ChangeActiveState(GameObject[] gameObjects, bool newActiveState)
    {
        foreach (GameObject uiElement in gameObjects)
        {
            uiElement.SetActive(newActiveState);
        }

    }
}
