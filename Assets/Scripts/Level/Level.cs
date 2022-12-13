/// <summary>
/// Struct that contains information about levels such as id and scene name
/// </summary>
[System.Serializable]
public struct Level
{
    public int LevelId;
    public string SceneName;
    public bool isEmpty;

    /// <summary>
    /// Creates and returns level sctruct that represents empty, not defined level (replacement for null for structs)
    /// </summary>
    public static Level GetEmptyLevelStruct()
    {
        Level level = new();
        level.isEmpty = true;

        return level;
    }
}
