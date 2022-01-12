using UnityEngine;

namespace Client.Match
{
    public struct CellPosition
    {
        public Vector2Int Value;
    }
}

public struct Vector2IntScaled
{
    public Vector2Int Value;
    public int Divisor;

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

    public bool IsGreaterThanDivisor()
    {
        return Value.sqrMagnitude >= Divisor * Divisor;
    }
}

public static class VectorScaledExtensions
{
    public static Vector2Int ToVector2Int(this Vector2IntScaled vector2IntScaled)
    {
        //return new Vector2Int(vector2IntScaled.Value.x / vector2IntScaled.Divisor, vector2IntScaled.Value.y / vector2IntScaled.Value.y);
        return Vector2Int.RoundToInt(vector2IntScaled.ToVector2());
    }

    public static Vector2 ToVector2(this Vector2IntScaled vector2IntScaled)
    {
        return (Vector2)vector2IntScaled.Value / vector2IntScaled.Divisor;
    }

    public static int GetLongestSide(this Vector2IntScaled vector2IntScaled)
    {
        var v = vector2IntScaled.Value;
        return v.x > v.y ? v.x : v.y;
    }
}