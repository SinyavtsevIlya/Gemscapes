using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    [UpdateBefore(typeof(PieceCollisionSystem))]
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
                ref var cellEntity = ref Get<CellLink>(pieceEntity).Value;
                Get<Velocity>(pieceEntity).Value += (Vector2)Get<GravityDirection>(cellEntity).Value * G * Time.deltaTime;
                Get<Position>(pieceEntity).Value += Get<Velocity>(pieceEntity).Value;
            }
        }
    }
}
