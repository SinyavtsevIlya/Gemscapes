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
                ref var gravityDirection = ref Get<GravityDirection>(gravityCellEntity).Value;
                velocity.Value += gravityDirection * GravityAmount;
                
                if (velocity.IsGreaterThanDivisor())
                {
                    velocity.SetFromVector2Int(gravityDirection);
                }

                var cellPosition = new Vector2IntScaled(Get<CellPosition>(cellEntity).Value, velocity.Divisor);

                if ((position.Value + velocity.Value).sqrMagnitude < cellPosition.Value.sqrMagnitude)
                {
                    position.Value += velocity.Value;
                }
                else
                {
                    gravityCellEntity = cellEntity;

                    var upcomingGravityDirection = Get<GravityDirection>(cellEntity).Value;

                    var remainder = (position.Value + velocity.Value) - cellPosition.Value;
                    // NOTE: just take the longest side to respect deterministic approach. 
                    var remainderMagnitude = remainder.x > remainder.y ? remainder.x : remainder.y;
                    var orientedRemainder = new Vector2IntScaled() 
                    {
                        Value = upcomingGravityDirection * remainderMagnitude / 4,
                        Divisor = velocity.Divisor
                    };

                    position.Value = cellPosition.Value + orientedRemainder.Value;
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
