using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class FallSystem : EcsSystemBase
    {
        private const float G = .2f;

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
                ref var gravity = ref Get<GravityDirection>(cellEntity).Value;
                velocity.Value += gravity;

                if (velocity.ToVector2().magnitude >= 1)
                {
                    velocity.SetFromVector2Int(gravity * 3);
                }

                Get<Position>(pieceEntity).Value.Value += Get<Velocity>(pieceEntity).Value.Value;
            }
        }
    }
}
