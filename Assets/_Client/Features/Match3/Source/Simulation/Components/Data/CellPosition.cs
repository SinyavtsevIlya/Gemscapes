using UnityEngine;

namespace Client.Match3
{
    /// <summary>
    /// Discrete position of the entity in cell-coordinates.
    /// </summary>
    public struct CellPosition
    {
        public Vector2Int Value;
    }
}

namespace Client
{
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
    }

    public static class FixedMathExtensions
    {
        public static Vector2Int Normalize(this Vector2IntFixed vector2IntFixed)
        {
            var x = 1;
            if (vector2IntFixed.RawValue.x == 0) x = 0;
            if (vector2IntFixed.RawValue.x < 0) x = -1;
            var y = 1;
            if (vector2IntFixed.RawValue.y == 0) y = 0;
            if (vector2IntFixed.RawValue.y < 0) y = -1;

            return new Vector2Int(x, y);
        }
        public static int RoundToClosest(int input, int divisor)
        {
            var remainder = input % divisor;
            var lowerBound = input - remainder;
            var upperBound = lowerBound + divisor;
            return input - lowerBound < upperBound - input ? lowerBound : upperBound;
        }

        public static int GetDetermenistecMargnitude(this Vector2Int vector2Int)
        {
            var lengthX = Mathf.Abs(vector2Int.x);
            var lengthY = Mathf.Abs(vector2Int.y);
            return lengthX > lengthY ? lengthX : lengthY;
        }

        public static Vector2Int ToVector2Int(this Vector2IntFixed vector2IntScaled)
        {
            var divisor = vector2IntScaled.Divisor;
            return new Vector2Int(RoundToClosest(vector2IntScaled.RawValue.x, divisor) / divisor, RoundToClosest(vector2IntScaled.RawValue.y, divisor) / divisor);
        }

        public static Vector2 ToVector2(this Vector2IntFixed vector2IntScaled)
        {
            return (Vector2)vector2IntScaled.RawValue / vector2IntScaled.Divisor;
        }
    }
}

