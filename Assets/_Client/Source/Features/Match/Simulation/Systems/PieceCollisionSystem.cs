using Nanory.Lex;
using UnityEngine;
using Unity.Mathematics.FixedPoint;

namespace Client.Match
{
    [Battle]
    [UpdateBefore(typeof(UpdateCellLinkSystem))]
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
                    var hasPieceArived = fpmath.lengthsq(cellGravityDirection) >=
                        fpmath.lengthsq(cellGravityDirection + cellPosition - piecePosition);

                    if (hasPieceArived)
                    {
                        piecePosition = cellPosition;
                        Get<Velocity>(pieceEntity).Value = fp2.zero;
                        Del<FallingTag>(pieceEntity);
                    }
                }
            }
        }
    }
}
