using Nanory.Lex;
using UnityEngine;

namespace Client.Match3
{
    public sealed class PieceCollisionSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<Position>()
            .With<MovableTag>()
            .With<FallingTag>()
            .End())
            {
                ref var piecePosition = ref Get<Position>(pieceEntity).Value;
                var grid = Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);
                ref var cellPosition = ref Get<CellPosition>(cellEntity).Value;
                ref var cellGravityDirection = ref Get<GravityDirection>(cellEntity).Value;
                ref var pieceGravityDirection = ref Get<GravityDirection>(Get<GravityCellLink>(pieceEntity).Value).Value;

                var isBlockedByUpcomingPiece =
                    TryGetBlockingPiece(cellPosition, cellGravityDirection, grid, out var blockingPieceEntity);

                if (isBlockedByUpcomingPiece)
                {
                    if (Has<FallingTag>(blockingPieceEntity))
                    {
                        PreventOvertaking(pieceEntity, grid, cellEntity, blockingPieceEntity);
                    }
                }

                var isBlockedByLevelBounds = !grid.IsInsideBounds(cellPosition + cellGravityDirection);

                var isBlockedByStoppedPiece = isBlockedByUpcomingPiece && !Has<FallingTag>(blockingPieceEntity);

                if (isBlockedByStoppedPiece || isBlockedByLevelBounds)
                {
                    var hasPieceArrived = cellGravityDirection.sqrMagnitude >=
                                        (cellGravityDirection + cellPosition - piecePosition.ToVector2Int()).sqrMagnitude;

                    if (hasPieceArrived)
                    {
                        StopPieceEntity(pieceEntity, cellPosition);
                    }
                }

                if (grid.TryGetCell(cellPosition + pieceGravityDirection, out var intendingCellEntity) &&
                    TryGet<IntendingPieceLink>(intendingCellEntity, out var intendingPieceLink) &&
                    intendingPieceLink.Value.Unpack(World, out int intendingPieceEntity) &&
                    intendingPieceEntity != pieceEntity &&
                    pieceGravityDirection != Get<GravityDirection>(Get<GravityCellLink>(intendingPieceEntity).Value).Value &&
                    !Has<StoppedEvent>(pieceEntity))
                {
                    StopPieceEntity(pieceEntity, cellPosition);
                }
            }
        }

        private void StopPieceEntity(int pieceEntity, Vector2Int cellPosition)
        {
            ref var piecePosition = ref Get<Position>(pieceEntity).Value;
            piecePosition = new Vector2IntFixed(cellPosition, piecePosition.Divisor);
            Get<Velocity>(pieceEntity).Value.RawValue = Vector2Int.zero;
            Del<FallingTag>(pieceEntity);
            Add<StoppedEvent>(pieceEntity);
        }

        private void PreventOvertaking(int pieceEntity, Grid grid, int cellEntity, int blockingPieceEntity)
        {
            ref var pieceVelocity = ref Get<Velocity>(pieceEntity).Value;
            ref var blockingPieceVolocity = ref Get<Velocity>(blockingPieceEntity).Value;

            var hasPieceOvertook =
                pieceVelocity.RawValue.sqrMagnitude > blockingPieceVolocity.RawValue.sqrMagnitude;

            if (hasPieceOvertook)
            {
                var blockingPieceCell = grid.GetCellByPiece(World, blockingPieceEntity);

                var isDirectionSame = Get<GravityDirection>(blockingPieceCell).Value ==
                    Get<GravityDirection>(cellEntity).Value;

                pieceVelocity.RawValue = isDirectionSame ?
                    blockingPieceVolocity.RawValue : pieceVelocity.RawValue = Vector2Int.zero;
            }
        }

        private bool TryGetBlockingPiece(Vector2Int cellPosition, Vector2Int cellGravityDirection, Grid grid, out int blockingPieceEntity)
        {
            blockingPieceEntity = -1;

            return grid.TryGetCell(cellPosition + cellGravityDirection, out var nextCellEntity)
                && TryGet<PieceLink>(nextCellEntity, out var blockingPieceLink)
                && blockingPieceLink.Value.Unpack(World, out blockingPieceEntity);
        }
    }
}
