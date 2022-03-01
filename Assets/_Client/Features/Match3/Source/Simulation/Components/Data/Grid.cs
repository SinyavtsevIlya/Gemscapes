using UnityEngine;
using Nanory.Lex;

namespace Client.Match3
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
        private const int InvalidEntityValue = -1;

        public static bool TryGetPiece(this EcsSystemBase ecsSystemBase, int cellEntity, out int pieceEntity) 
        {
            if (ecsSystemBase.World.TryGet<PieceLink>(cellEntity, out var pieceLink))
            {
                return pieceLink.Value.Unpack(ecsSystemBase.World, out pieceEntity);
            }
            pieceEntity = InvalidEntityValue;
            return false;
        }

        public static int GetCellByPiece(this Grid grid, EcsWorld world, int pieceEntity)
        {
            var roundedPiecePosition = world.Get<Position>(pieceEntity).Value.ToVector2Int();
#if DEBUG
            if (!grid.IsInsideBounds(roundedPiecePosition))
            {
                throw new System.IndexOutOfRangeException($"pieceEntity {pieceEntity} in {roundedPiecePosition} is outside of bounds: {grid.Value.GetLength(0)}, {grid.Value.GetLength(1)}");
            }

            if (grid.Value[roundedPiecePosition.x, roundedPiecePosition.y] == InvalidEntityValue)
            {
                throw new System.Exception($"pieceEntity {pieceEntity} has an invalid position: {roundedPiecePosition}. Raw position is {world.Get<Position>(pieceEntity).Value.RawValue}");
            }
#endif
            return grid.Value[roundedPiecePosition.x, roundedPiecePosition.y];
        }

        public static bool TryGetCell(this Grid grid, Vector2Int position, out int cellEntity)
        {
            return TryGetCell(grid, position.x, position.y, out cellEntity);
        }

        public static bool TryGetCell(this Grid grid, int x, int y, out int cellEntity)
        {
            cellEntity = InvalidEntityValue;
            if (grid.IsInsideBounds(x, y))
            {
                cellEntity = grid.Value[x, y];

                if (cellEntity == InvalidEntityValue)
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
            return IsInsideBounds(grid, position.x, position.y);
        }

        public static bool IsInsideBounds(this Grid grid, int x, int y)
        {
            return x >= 0 &&
                   y >= 0 &&
                   x < grid.Value.GetLength(0) &&
                   y < grid.Value.GetLength(1) &&
                   grid.Value[x,y] != InvalidEntityValue;
        }
    }
}