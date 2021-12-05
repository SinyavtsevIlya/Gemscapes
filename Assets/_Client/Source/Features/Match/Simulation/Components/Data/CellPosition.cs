using UnityEngine;

namespace Client
{
    public struct CellPosition
    {
        public Vector2Int Value;
    }
}

public struct Vector2IntScaled
{
    public Vector2Int Value;
    public readonly int Divisor;

    public Vector2IntScaled(Vector2Int value, int divisor)
    {
        Value = value * divisor;
        Divisor = divisor;
    }

    public Vector2IntScaled(int x, int y, int divisor)
    {
        Value = new Vector2Int(x, y) * divisor;
        Divisor = divisor;
    }

    public void SetFromVector2(Vector2 value)
    {
        Value = Vector2Int.RoundToInt(value * Divisor);
    }

    public void SetFromVector2Int(Vector2Int value)
    {
        Value = value * Divisor;
    }
}

public static class VectorScaledExtensions
{
    public static Vector2Int ToVector2Int(this Vector2IntScaled vector2IntScaled)
    {
        return Vector2Int.RoundToInt(ToVector2(vector2IntScaled));
    }

    public static Vector2 ToVector2(this Vector2IntScaled vector2IntScaled)
    {
        return (Vector2)vector2IntScaled.Value / vector2IntScaled.Divisor;
    }
}