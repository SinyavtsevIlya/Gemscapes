using Nanory.Lex;
using UnityEngine;

namespace Client.Match3
{
    public sealed class GravitySystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<Position>()
            .With<MovableTag>()
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

                var magnitude = velocity.RawValue.GetDetermenistecMargnitude();
                magnitude = Mathf.Clamp(magnitude, 0, SimConstants.MaxVelocity);

                velocity = new Vector2IntFixed()
                {
                    RawValue = pieceGravityDirection * (magnitude + SimConstants.GravityAmount),
                    Divisor = velocity.Divisor
                };

                var gravityCellPosition = new Vector2IntFixed(Get<CellPosition>(gravityCellEntity).Value, velocity.Divisor);
                var grivityDirectionScaled = new Vector2IntFixed(pieceGravityDirection, velocity.Divisor);
                var pieceDelta = position.RawValue + velocity.RawValue - gravityCellPosition.RawValue;

                if (pieceDelta.sqrMagnitude < grivityDirectionScaled.RawValue.sqrMagnitude)
                {
                    position.RawValue += velocity.RawValue;
                }
                else
                {
                    gravityCellEntity = cellEntity;
                    if (grid.TryGetCell(Get<CellPosition>(gravityCellEntity).Value, out var upcomingGravityCell))
                    {
                        var upcomingGravityDirection = Get<GravityDirection>(upcomingGravityCell).Value;

                        var remainder = pieceDelta - grivityDirectionScaled.RawValue;

                        var remainderMagnitude = remainder.GetDetermenistecMargnitude();
                        var orientedRemainder = new Vector2IntFixed()
                        {
                            RawValue = upcomingGravityDirection * remainderMagnitude,
                            Divisor = velocity.Divisor
                        };

                        position.RawValue = gravityCellPosition.RawValue + grivityDirectionScaled.RawValue + orientedRemainder.RawValue;
                    }
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
