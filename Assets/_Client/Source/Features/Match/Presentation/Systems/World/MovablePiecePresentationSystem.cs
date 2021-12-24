using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine;
#if DEBUG
using Nanory.Lex.UnityEditorIntegration;
#endif

namespace Client.Match
{
    [Battle]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public sealed class MovablePiecePresentationSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<Mono<MovablePieceView>>()
            .With<MatchedEvent>()
            .End())
            {
                ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity).Value;
                view.Destroy(UnityIdents.Time.FixedDelta * 10);
            }

            foreach (var pieceEntity in Filter()
            .With<Mono<MovablePieceView>>()
            .With<Position>()
            .With<FallingTag>()
            .End())
            {
                ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity).Value;
                view.SetPosition(Get<Position>(pieceEntity).Value.ToVector2());
            }

            foreach (var pieceEntity in Filter()
            .With<Mono<MovablePieceView>>()
            .With<StoppedEvent>()
            .End())
            {
                ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity).Value;
                view.SetPosition(Get<Position>(pieceEntity).Value.ToVector2());
                view.SetAsStopped();
            }

            foreach (var pieceEntity in Filter()
            .With<CellLinkUpdatedEvent>()
            .End())
            {
                //ref var cellLinkUpdatedEvent = ref Get<CellLinkUpdatedEvent>(pieceEntity);
                ////ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity).Value;
                //ref var grid = ref Get<Grid>(pieceEntity);

                //ref var cellPositionPrevious = ref Get<CellPosition>(cellLinkUpdatedEvent.PreviousCell).Value;
                //ref var cellPositionCurrent = ref Get<CellPosition>(cellLinkUpdatedEvent.CurrentCell).Value;

                //var one = new Vector3(cellPositionPrevious.x, cellPositionPrevious.y, 0f);
                //var two = new Vector3(cellPositionCurrent.x, cellPositionCurrent.y, 0f);
                ////Debug.DrawLine(one, two, Color.green, .01f);
            }

            foreach (var pieceEntity in Filter()
            .With<MovableTag>()
            .With<CreatedEvent>()
            .End())
            {
                var grid = Get<Grid>(pieceEntity);
                var piecePrefabEntity = Get<PieceTypeId>(pieceEntity).Value;
                var prefabView = Get<GameObjectReference>(piecePrefabEntity).Value.GetComponent<MovablePieceView>();
                var view = Object.Instantiate(prefabView);

#if DEBUG
                EcsSystems.LinkDebugGameobject(World, pieceEntity, view.gameObject);
#endif

                Add<Mono<MovablePieceView>>(pieceEntity).Value = view;

                view.SetPosition(Get<Position>(pieceEntity).Value.ToVector2());

                view.Clicked += () => 
                {
                    //Add<FallingTag>(pieceEntity); 
                    //later.Add<DestroyedEvent>(pieceEntity);
                };

                view.Draged += (dragDirection) => 
                {
                    Debug.Log($"dragDirection: {dragDirection}");
                    if (grid.TryGetCell(Get<Position>(pieceEntity).Value.ToVector2Int() + dragDirection, out var cellEntity))
                    {
                        if (TryGet<PieceLink>(cellEntity, out var pieceLink))
                        {
                            if (pieceLink.Value.Unpack(World, out var targetPieceEntity))
                            {
                                Debug.Log("Add swap request");
                                later.Add<SwapPieceRequest>(World.NewEntity()) = new SwapPieceRequest()
                                {
                                    PieceA = pieceEntity,
                                    PieceB = targetPieceEntity
                                };

                                Get<Mono<MovablePieceView>>(pieceEntity).Value.SetPosition(Get<Position>(pieceEntity).Value.ToVector2Int() + dragDirection);
                                Get<Mono<MovablePieceView>>(targetPieceEntity).Value.SetPosition(Get<Position>(pieceEntity).Value.ToVector2Int());
                            }
                        }
                    }
                };
            }
        }
    }
}
