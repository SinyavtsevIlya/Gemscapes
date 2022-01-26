using UnityEngine;

namespace Client.Match3
{
    public struct CellPosition
    {
        public Vector2Int Value;
    }
}

namespace Client
{
    public struct f_int64
    {

    }

    public struct Vector2IntFixed
    {
        public Vector2Int RawValue;
        public int Divisor;

        public Vector2IntFixed(Vector2Int value, int divisor)
        {
            RawValue = value * divisor;
            Divisor = divisor;
        }

        public Vector2IntFixed(int x, int y, int divisor)
        {
            RawValue = new Vector2Int(x, y) * divisor;
            Divisor = divisor;
        }

        public void SetFromVector2(Vector2 value)
        {
            RawValue = Vector2Int.RoundToInt(value * Divisor);
        }
    }

    public static class VectorScaledExtensions
    {
        public static int GetDetermenistecMargnitude(this Vector2Int vector2Int)
        {
            var lengthX = Mathf.Abs(vector2Int.x);
            var lengthY = Mathf.Abs(vector2Int.y);
            return lengthX > lengthY ? lengthX : lengthY;
        }

        public static Vector2Int ToVector2Int(this Vector2IntFixed vector2IntScaled)
        {
            var divisor = vector2IntScaled.Divisor;
            //return new Vector2Int(vector2IntScaled.RawValue.x / divisor, vector2IntScaled.RawValue.y / divisor);
            return Vector2Int.RoundToInt(vector2IntScaled.ToVector2());
        }

        public static Vector2 ToVector2(this Vector2IntFixed vector2IntScaled)
        {
            return (Vector2)vector2IntScaled.RawValue / vector2IntScaled.Divisor;
        }

        public static int GetLongestSide(this Vector2IntFixed vector2IntScaled)
        {
            var v = vector2IntScaled.RawValue;
            return v.x > v.y ? v.x : v.y;
        }
    }
}

