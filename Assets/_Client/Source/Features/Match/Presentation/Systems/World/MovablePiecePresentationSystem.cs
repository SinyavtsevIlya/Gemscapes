using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public sealed class MovablePiecePresentationSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var pieceEntity in Filter()
            .With<Mono<MovablePieceView>>()
            .With<Position>()
            //.With<FallingTag>()
            .End())
            {
                ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity).Value;
                view.SetPosition(Get<Position>(pieceEntity).Value);
            }

            foreach (var pieceEntity in Filter()
            .With<Mono<MovablePieceView>>()
            .With<Position>()
            .End())
            {
                ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity).Value;
                //view.SetMovableState(Has<FallingTag>(pieceEntity));
            }
        }
    }
}
