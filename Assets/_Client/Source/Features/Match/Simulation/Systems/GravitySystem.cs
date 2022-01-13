using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class GravitySystem : EcsSystemBase
    {
        private const int GravityAmount = 1;

        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<Position>()
            .With<GravityCellLink>()
            .With<FallingTag>()
            .End())
            {
                ref var velocity = ref Get<Velocity>(pieceEntity).Value;
                ref var position = ref Get<Position>(pieceEntity).Value;
                ref var gravityCellEntity = ref Get<GravityCellLink>(pieceEntity).Value;

                var roundedPosition = position.ToVector2Int();

                var grid = Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);
                ref var pieceGravityDirection = ref Get<GravityDirection>(gravityCellEntity).Value;
                ref var cellGravityDirectin = ref Get<GravityDirection>(cellEntity).Value;

                if (pieceGravityDirection == cellGravityDirectin)
                {
                    velocity.Value += pieceGravityDirection * GravityAmount;

                    if (velocity.IsGreaterThanDivisor())
                    {
                        velocity.SetFromVector2Int(pieceGravityDirection);
                    }
                }
                else
                {
                    velocity = new Vector2IntScaled()
                    {
                        Value = pieceGravityDirection * velocity.Value.GetDetermenistecMargnitude(),
                        Divisor = velocity.Divisor
                    };
                }

                var gravityCellPosition = new Vector2IntScaled(Get<CellPosition>(gravityCellEntity).Value, velocity.Divisor);
                var grivityDirectionScaled = new Vector2IntScaled(pieceGravityDirection, velocity.Divisor);
                var pieceDelta = position.Value + velocity.Value - gravityCellPosition.Value;

                if (pieceDelta.sqrMagnitude < grivityDirectionScaled.Value.sqrMagnitude)
                {
                    position.Value += velocity.Value;
                }
                else
                {
                    gravityCellEntity = cellEntity;

                    var upcomingGravityDirection = Get<GravityDirection>(cellEntity).Value;

                    var remainder = pieceDelta - grivityDirectionScaled.Value;
                    // NOTE: just take the longest side to respect deterministic approach. 
                    var remainderMagnitude = remainder.GetDetermenistecMargnitude();
                    var orientedRemainder = new Vector2IntScaled() 
                    {
                        Value = upcomingGravityDirection * remainderMagnitude,
                        Divisor = velocity.Divisor
                    };

                    position.Value = new Vector2IntScaled(Get<CellPosition>(cellEntity).Value, velocity.Divisor).Value + orientedRemainder.Value;
                }



                if (position.ToVector2Int() != roundedPosition)
                {
                    later.Add<CellPositionUpdatedEvent>(pieceEntity) = new CellPositionUpdatedEvent()
                    {
                        PreviousCell = cellEntity,
                        CurrentCell = grid.GetCellByPiece(World, pieceEntity)
                    };
                }
            }
        }
    }
}
