using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine;
using Nanory.Lex.Lifecycle;
#if DEBUG
using Nanory.Lex.UnityEditorIntegration;
#endif

namespace Client.Match3
{
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
            //.With<FallingTag>() ? 
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
                var grid = Get<Grid>(pieceEntity);
                var piecePrefabEntity = Get<PieceTypeId>(pieceEntity).Value;
                var prefabView = Get<GameObjectReference>(piecePrefabEntity).Value.GetComponent<MovablePieceView>();
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);
                var boardEntity = Get<BoardLink>(cellEntity).Value;
                var boardGameobject = Get<GameObjectReference>(boardEntity).Value;
                var piecesRoot = boardGameobject.transform.GetChild(1);
                var view = Object.Instantiate(prefabView, piecesRoot, true);

#if DEBUG
                EcsSystems.LinkDebugGameobject(World, pieceEntity, view.gameObject);
#endif

                Add<Mono<MovablePieceView>>(pieceEntity).Value = view;

                view.transform.position = Get<Position>(pieceEntity).Value.ToVector2();
                view.SetPosition(Get<Position>(pieceEntity).Value.ToVector2());
                view.SetPosition(Get<Position>(pieceEntity).Value.ToVector2());

                view.Clicked += () => 
                {
                    return;

                    var grid = Get<Grid>(pieceEntity);

                    var piecePosition = Get<CellPosition>(grid.GetCellByPiece(World, pieceEntity)).Value;

                    for (int i = piecePosition.y - 1; i < piecePosition.y + 1; i++)
                    {
                        for (int j = piecePosition.x - 4; j < piecePosition.x + 4; j++)
                        {
                            var pos = new Vector2Int(j, i);

                            if (grid.TryGetCell(pos, out var cellEntity)
                                && TryGet(cellEntity, out PieceLink pieceLink)
                                && pieceLink.Value.Unpack(World, out int currentPieceEntity))
                            {
                                later.AddOrSet<DestroyedEvent>(currentPieceEntity);
                                Del<PieceLink>(grid.GetCellByPiece(World, currentPieceEntity));
                            }
                        }
                    }
                };

                view.Draged += (dragDirection) => 
                {
                    // Avoid diagonal drags
                    bool isVerticalOrHorizontal = dragDirection.sqrMagnitude == 1;
                    var destinationPosition = Get<Position>(pieceEntity).Value.ToVector2Int() + dragDirection;
                    var originPosition = Get<Position>(pieceEntity).Value.ToVector2Int();

                    if (isVerticalOrHorizontal 
                        && grid.TryGetCell(destinationPosition, out var cellEntity)
                        && TryGet<PieceLink>(cellEntity, out var pieceLink)
                        && pieceLink.Value.Unpack(World, out var destinationPieceEntity))
                    {
                        if (IsValidMove(grid, pieceEntity, destinationPosition))
                        {
                            later.Add<SwapPieceRequest>(World.NewEntity()) = new SwapPieceRequest()
                            {
                                PieceA = pieceEntity,
                                PieceB = destinationPieceEntity
                            };

                            view.Swap(destinationPosition);
                            Get<Mono<MovablePieceView>>(destinationPieceEntity).Value
                            .Swap(originPosition);
                        }
                        else
                        {
                            view.FailSwap(destinationPosition);
                            Get<Mono<MovablePieceView>>(destinationPieceEntity).Value.FailSwap(originPosition);
                        }
                    }
                };

                view.enabled = true;
            }
        }

        private bool IsValidMove(Grid grid, int pieceEntity, Vector2Int move)
        {
            var pieceType = Get<PieceTypeId>(pieceEntity).Value;
            var originPosition = Get<CellPosition>(grid.GetCellByPiece(World, pieceEntity)).Value;

            var matches = 0;
            var hasMatched = false;

            for (int dimensionIdx = 0; dimensionIdx < 2; dimensionIdx++)
            {
                for (var distanceIdx = -SimConstants.LineMatchLength; distanceIdx < SimConstants.LineMatchLength; distanceIdx++)
                {
                    var currentPosition = move;
                    currentPosition[dimensionIdx] += distanceIdx;

                    if (currentPosition == originPosition)
                        continue;

                    if (currentPosition == move) 
                    {
                        hasMatched = true;
                    }
                    else
                    {
                        if (grid.TryGetCell(currentPosition, out var cellEntity))
                        {
                            if (TryGet<PieceLink>(cellEntity, out var pieceLink) &&
                                pieceLink.Value.Unpack(World, out var currentPieceEntity))
                            {
                                if (Get<PieceTypeId>(currentPieceEntity).Value == pieceType)
                                {
                                    hasMatched = true;
                                }
                            }
                        }
                    }

                    // Process matching
                    if (hasMatched)
                    {
                        matches++;
                        hasMatched = false;
                        if (matches == SimConstants.LineMatchLength)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }
                }
            }

            return false;
        }
    }
}
