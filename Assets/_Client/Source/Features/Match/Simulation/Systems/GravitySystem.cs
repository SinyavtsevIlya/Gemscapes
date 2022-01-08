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
            .With<MovableTag>()
            .With<FallingTag>()
            .End())
            {
                ref var velocity = ref Get<Velocity>(pieceEntity).Value;
                ref var position = ref Get<Position>(pieceEntity).Value;

                var roundedPosition = position.ToVector2Int();

                var grid = Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);
                ref var gravityDirection = ref Get<GravityOutputDirection>(cellEntity).Value;
                velocity.Value += gravityDirection * GravityAmount;

                if (velocity.IsGreaterThanDivisor())
                {
                    velocity.SetFromVector2Int(gravityDirection);
                }

                position.Value += velocity.Value;

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
