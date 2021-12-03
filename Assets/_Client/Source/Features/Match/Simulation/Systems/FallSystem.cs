using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class FallSystem : EcsSystemBase
    {
        private const float G = .5f;

        protected override void OnUpdate()
        {
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
