using UnityEngine;

public class EditorBlock
{
    public readonly GameObject gameObject;
    private Quaternion defaultRotation;
    private int rotation = 0;

    public EditorBlock(GameObject gameObject)
    {
        defaultRotation = gameObject.transform.rotation;
    }

    public void Rotate()
    {
        rotation += 90;

        if (rotation == 360) rotation = 0;

        gameObject.transform.rotation = defaultRotation * Quaternion.Euler(0, 0, rotation);
    }
}
