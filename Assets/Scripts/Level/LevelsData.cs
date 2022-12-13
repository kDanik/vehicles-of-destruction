using UnityEngine;

/// <summary>
/// Scriptable object that stores information about existing levels (see Level struct) and helper methods for them
/// </summary>
[CreateAssetMenu(fileName = "LevelsData", menuName = "ScriptableObjects/LevelsData", order = 1)]
public class LevelsData : ScriptableObject
{
    [SerializeField] private Level[] levels;


    /// <summary>
    /// Finds and returns Level by provided level id. If none found return empty Level struct
    /// </summary>
    public Level FindLevelByLevelId(int levelId)
    {
        foreach (Level level in levels) {
            if (level.LevelId == levelId) return level;
        }
    
        return Level.GetEmptyLevelStruct();
    }

    /// <summary>
    /// Returns array containing all registered levels
    /// </summary>
    public Level[] GetAllLevels()
    {
        return levels;
    }
}
