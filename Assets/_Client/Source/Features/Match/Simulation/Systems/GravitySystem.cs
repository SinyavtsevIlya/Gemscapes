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
            .With<CellLink>()
            .With<FallingTag>()
            .End())
            {
                ref var velocity = ref Get<Velocity>(pieceEntity).Value;
                ref var cellEntity = ref Get<CellLink>(pieceEntity).Value;
                ref var gravityDirection = ref Get<GravityDirection>(cellEntity).Value;
                velocity.Value += gravityDirection * GravityAmount;

                if (velocity.IsGreaterThanDivisor())
                {
                    velocity.SetFromVector2Int(gravityDirection);
                }

                Get<Position>(pieceEntity).Value.Value += Get<Velocity>(pieceEntity).Value.Value;
            }
        }
    }
}
