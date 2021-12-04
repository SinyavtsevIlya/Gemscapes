using UnityEngine;
using Nanory.Lex;
using Unity.Mathematics.FixedPoint;

namespace Client.Match
{
    public struct Grid
    {
        public int[,] Value;

        public Grid(int[,] value)
        {
            Value = value;
        }
    }

    public static class GridExtensions
    {
        public static bool TryGetCell(this Grid grid, fp2 position, out int cellEntity)
        {
            cellEntity = -1;
            if (grid.IsInsideBounds((int)position.x, (int)position.y))
            {
                cellEntity = grid.Value[(int)position.x, (int)position.y];
                return true;
            }
            return false;
        }

        public static bool IsBlocking(this Grid grid, EcsWorld world, fp2 position, fp2 direction)
        {
            if (grid.TryGetCell(position + direction, out var cellEntity))
            {
                if (world.TryGet<PieceLink>(cellEntity, out var pieceLink))
                {
                    return pieceLink.Value.Unpack(world, out var pieceEntity);
                }
                return false;
            }
            return true;
        }

        public static bool IsInsideBounds(this Grid grid, int x, int y)
        {
            return x >= 0 &&
                   y >= 0 && 
                   x < grid.Value.GetLength(0) &&
                   y < grid.Value.GetLength(1);
        }
    }
}