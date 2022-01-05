using UnityEngine;
using Nanory.Lex;

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
        public static int GetCellByPiece(this Grid grid, EcsWorld world, int pieceEntity)
        {
            var roundedPiecePosition = world.Get<Position>(pieceEntity).Value.ToVector2Int();
            return grid.Value[roundedPiecePosition.x, roundedPiecePosition.y];
        }

        public static bool TryGetCell(this Grid grid, Vector2Int position, out int cellEntity)
        {
            cellEntity = -1;
            if (grid.IsInsideBounds(position))
            {
                cellEntity = grid.Value[position.x, position.y];

                if (cellEntity == -1)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        public static bool IsBlocking(this Grid grid, EcsWorld world, Vector2Int position, Vector2Int direction)
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

        public static bool IsInsideBounds(this Grid grid, Vector2Int position)
        {
            return position.x >= 0 &&
                   position.y >= 0 && 
                   position.x < grid.Value.GetLength(0) &&
                   position.y < grid.Value.GetLength(1);
        }
    }
}