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
                view.SetMovableState(Has<FallingTag>(pieceEntity));
            }

            foreach (var pieceEntity in Filter()
            .With<CellLinkUpdatedEvent>()
            .End())
            {
                ref var cellLinkUpdatedEvent = ref Get<CellLinkUpdatedEvent>(pieceEntity);
                ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity).Value;
                ref var grid = ref Get<Grid>(pieceEntity);

                ref var cellPositionPrevious = ref Get<CellPosition>(cellLinkUpdatedEvent.PreviousCell).Value;
                ref var cellPositionCurrent = ref Get<CellPosition>(cellLinkUpdatedEvent.CurrentCell).Value;

                var one = new Vector3(cellPositionPrevious.x, cellPositionPrevious.y, 0f);
                var two = new Vector3(cellPositionCurrent.x, cellPositionCurrent.y, 0f);
                Debug.DrawLine(one, two, Color.green, .01f);
            }

            foreach (var pieceEntity in Filter()
            .With<Mono<MovablePieceView>>()
            .With<CreatedEvent>()
            .End())
            {
                ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity).Value;
                view.Clicked += () => { Add<FallingTag>(pieceEntity); };
            }
        }
    }
}
