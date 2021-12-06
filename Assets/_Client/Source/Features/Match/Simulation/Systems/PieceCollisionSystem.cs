using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class PieceCollisionSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<Position>()
            .With<FallingTag>()
            .End())
            {
                ref var piecePosition = ref Get<Position>(pieceEntity).Value;
                ref var cellEntity = ref Get<CellLink>(pieceEntity).Value;
                ref var cellPosition = ref Get<CellPosition>(cellEntity).Value;
                ref var cellGravityDirection = ref Get<GravityDirection>(cellEntity).Value;
                ref var grid = ref Get<Grid>(pieceEntity);

                var isColliding = false;

                if (grid.IsBlocking(World, cellPosition, cellGravityDirection))
                {
                    if (grid.TryGetCell(cellPosition + cellGravityDirection, out var nextCellEntity))
                    {
                        if (TryGet<PieceLink>(nextCellEntity, out var blockingPieceLink))
                        {
                            if (blockingPieceLink.Value.Unpack(World, out var blockingPieceEntity))
                            {
                                if (!Has<FallingTag>(blockingPieceEntity))
                                {
                                    isColliding = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        isColliding = true;
                    }
                }

                if (isColliding)
                {
                    var hasPieceArived = cellGravityDirection.sqrMagnitude >=
                                        (cellGravityDirection + cellPosition - piecePosition.ToVector2Int()).sqrMagnitude;

                    if (hasPieceArived)
                    {
                        piecePosition.SetFromVector2(cellPosition);
                        Get<Velocity>(pieceEntity).Value.Value = Vector2Int.zero;
                        Del<FallingTag>(pieceEntity);
                        Add<StoppedEvent>(pieceEntity);
                    }
                }
            }
        }
    }
}
