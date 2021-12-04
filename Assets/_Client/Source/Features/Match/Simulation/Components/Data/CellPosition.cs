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
    private readonly int _divisor;

    public Vector2IntScaled(Vector2Int value, int divisor)
    {
        Value = value * divisor;
        _divisor = divisor;
    }

    public Vector2IntScaled(int x, int y, int divisor)
    {
        Value = new Vector2Int(x, y) * divisor;
        _divisor = divisor;
    }

    public Vector2Int ToVector2Int()
    {
        return Value / _divisor;
    }

    public Vector2 ToVector2()
    {
        return (Vector2) Value / _divisor;
    }

    public void FromVector2(Vector2 value)
    {
        Value = Vector2Int.RoundToInt(value * _divisor);
    }
}

public static class VectorScaledExtensions
{
}