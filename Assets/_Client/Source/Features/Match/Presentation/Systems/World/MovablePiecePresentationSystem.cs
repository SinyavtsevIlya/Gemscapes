using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine;
#if DEBUG
using Nanory.Lex.UnityEditorIntegration;
#endif

namespace Client.Match
{
    [M3]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public sealed class MovablePiecePresentationSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<Mono<MovablePieceView>>()
            .With<DestroyedEvent>()
            .End())
            {
                ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity).Value;
                view.Destroy(UnityIdents.Time.FixedDelta * 10);
            }

            foreach (var pieceEntity in Filter()
            .With<Mono<MovablePieceView>>()
            .With<Position>()
            //.With<FallingTag>()
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
                view.SetTo(view.transform.position);
                view.SetPosition(Get<Position>(pieceEntity).Value.ToVector2());
                view.SetAsStopped();
            }

            foreach (var pieceEntity in Filter()
            .With<MovableTag>() 
            .Without<Mono<MovablePieceView>>()
            .End())
            {
                Debug.Log("beep");
                var grid = Get<Grid>(pieceEntity);
                var piecePrefabEntity = Get<PieceTypeId>(pieceEntity).Value;
                var prefabView = Get<GameObjectReference>(piecePrefabEntity).Value.GetComponent<MovablePieceView>();
                var view = Object.Instantiate(prefabView);
#if DEBUG
                EcsSystems.LinkDebugGameobject(World, pieceEntity, view.gameObject);
#endif

                Add<Mono<MovablePieceView>>(pieceEntity).Value = view;

                view.transform.position = Get<Position>(pieceEntity).Value.ToVector2();
                view.SetPosition(Get<Position>(pieceEntity).Value.ToVector2());
                view.SetPosition(Get<Position>(pieceEntity).Value.ToVector2());

                view.Clicked += () => 
                {
                    var grid = Get<Grid>(pieceEntity);

                    var piecePosition = Get<CellPosition>(grid.GetCellByPiece(World, pieceEntity)).Value;

                    for (int i = piecePosition.y - 2; i < piecePosition.y + 2; i++)
                    {
                        for (int j = piecePosition.x - 2; j < piecePosition.x + 2; j++)
                        {
                            var pos = new Vector2Int(j, i);
                            if (grid.TryGetCell(pos, out var cellEntity))
                            {
                                if (TryGet(cellEntity, out PieceLink pieceLink))
                                {
                                    if (pieceLink.Value.Unpack(World, out int currentPieceEntity))
                                    {
                                        later.Add<DestroyedEvent>(currentPieceEntity);
                                        Del<PieceLink>(grid.GetCellByPiece(World, currentPieceEntity));
                                    }
                                }
                            }
                        }
                    }
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
