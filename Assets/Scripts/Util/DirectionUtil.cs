using UnityEngine;

/// <summary>
/// Utility class to work with Direction class (mostly used for vehicle editor)
/// </summary>
public class DirectionUtil
{
    /// <summary>
    /// Calculates direction from initialDirection by applying provided rotation(to the right)
    /// </summary>
    /// <param name="rotationInDegrees">See block implementation. Only accepts rotation with value 0, 90 , 180 and 270</param>
    public static Direction CalculateDirectionAfterRotation(Direction initialDirection, int rotationInDegrees)
    {
        while (rotationInDegrees != 0)
        {
            initialDirection = CalculateDirectionAfterOneRotation(initialDirection);
            rotationInDegrees -= 90;
        }

        return initialDirection;
    }

    /// <summary>
    /// Returns direction oposite from provided one (left --> right, top --> bottom, ...)
    /// </summary>
    public static Direction GetOpositeDirection(Direction direction)
    {
        return CalculateDirectionAfterOneRotation(CalculateDirectionAfterOneRotation(direction));
    }

    /// <summary>
    /// Return direction calculated from initial direction by rotating it once (to the right, 90 degrees).
    /// </summary>
    private static Direction CalculateDirectionAfterOneRotation(Direction initialDirection)
    {
        switch (initialDirection)
        {
            case Direction.LEFT:
                return Direction.TOP;
            case Direction.RIGHT:
                return Direction.BOTTOM;
            case Direction.BOTTOM:
                return Direction.LEFT;
            case Direction.TOP:
                return Direction.RIGHT;
            default:
                Debug.LogError("unknown direction enum provided for DirectionUtil");
                return Direction.UNDEFINED;
        }
    }
}
