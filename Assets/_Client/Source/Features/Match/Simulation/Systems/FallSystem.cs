using Nanory.Lex;
using UnityEngine;
using Unity.Mathematics.FixedPoint;

namespace Client.Match
{
    [Battle]
    [UpdateBefore(typeof(PieceCollisionSystem))]
    public sealed class FallSystem : EcsSystemBase
    {
        private const decimal G = 1.5m;

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
                Get<Velocity>(pieceEntity).Value += Get<GravityDirection>(cellEntity).Value * G * (fp)Time.fixedDeltaTime;
                Get<Position>(pieceEntity).Value += Get<Velocity>(pieceEntity).Value;
            }
        }
    }
}
