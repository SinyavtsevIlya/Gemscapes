using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [M3]
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

                if (IsColliding(cellPosition, cellGravityDirection, grid, pieceEntity))
                {
                    var hasPieceArived = cellGravityDirection.sqrMagnitude >=
                                        (cellGravityDirection + cellPosition - piecePosition.ToVector2Int()).sqrMagnitude;

                    if (hasPieceArived)
                    {
                        Debug.Log($"Stop {pieceEntity} ");
                        piecePosition.SetFromVector2(cellPosition);
                        Get<Velocity>(pieceEntity).Value.Value = Vector2Int.zero;
                        Del<FallingTag>(pieceEntity);
                        Add<StoppedEvent>(pieceEntity);
                    }
                }
            }
        }

        private bool IsColliding(Vector2Int cellPosition, Vector2Int cellGravityDirection, Grid grid, int pieceEntity)
        {
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
                                return true;
                            }
                            else
                            {

                                ref var pieceVelocity = ref Get<Velocity>(pieceEntity).Value;
                                ref var blockingPieceVolocity = ref Get<Velocity>(blockingPieceEntity).Value;
                                if (pieceVelocity.Value.sqrMagnitude > blockingPieceVolocity.Value.sqrMagnitude)
                                {
                                    if (Get<GravityDirection>(nextCellEntity).Value == Get<GravityDirection>(grid.Value[cellPosition.x, cellPosition.y]).Value)
                                    {
                                        pieceVelocity.Value = blockingPieceVolocity.Value;
                                    }
                                    else
                                    {
                                        pieceVelocity.Value = Vector2Int.zero;
                                    }
                                    
                                }
      
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}
