using UnityEngine;

/// <summary>
/// Scriptable object that stores information about existing levels (see Level struct) and helper methods for them
/// </summary>
[CreateAssetMenu(fileName = "LevelsData", menuName = "ScriptableObjects/LevelsData", order = 1)]
public class LevelsData : ScriptableObject
{
    [SerializeField] private Level[] levels;


    /// <summary>
    /// Finds and returns Level by provided level index. If none found return empty Level struct
    /// </summary>
    public Level FindLevelByLevelIndex(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length) return Level.GetEmptyLevelStruct();

        return levels[levelIndex];
    }

    /// <summary>
    /// Returns array containing all registered levels
    /// </summary>
    public Level[] GetAllLevels()
    {
        return levels;
    }
}
