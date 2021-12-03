using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class PieceCollisionSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
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

                if (grid.IsBlocking(World, cellPosition, cellGravityDirection))
                {
                    var hasPieceArived = cellGravityDirection.sqrMagnitude >=
                        (cellGravityDirection + cellPosition - piecePosition).sqrMagnitude;

                    if (hasPieceArived)
                    {
                        piecePosition = cellPosition;
                        Del<FallingTag>(pieceEntity);
                    }
                }
            }
        }
    }
}
