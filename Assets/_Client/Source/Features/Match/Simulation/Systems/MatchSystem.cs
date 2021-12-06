using Nanory.Lex;
using Nanory.Collections;

namespace Client.Match
{
    [Battle]
    public sealed class MatchSystem : EcsSystemBase
    {
        private EcsFilter _fallingPieces;
        private ResizableArray<int> _matches;

        protected override void OnCreate()
        {
            _fallingPieces = World.Filter<Position>().With<FallingTag>().End();
            _matches = new ResizableArray<int>(8);
        }
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            // TODO: implement SharedComponents 
            foreach (var boardEntity in Filter()
            .With<BoardTag>()
            .Without<IdleTag>()
            .End())
            {
                if (_fallingPieces.GetEntitiesCount() == 0)
                {
                    later.Add<IdleTag>(boardEntity);
                    later.Add<StoppedEvent>(boardEntity);
                }
            }

            foreach (var boardEntity in Filter()
            .With<BoardTag>()
            .With<IdleTag>()
            .End())
            {
                if (_fallingPieces.GetEntitiesCount() > 0)
                {
                    later.Del<IdleTag>(boardEntity);
                    //later.Add<>
                }
            }

            foreach (var boardEntity in Filter()
            .With<StoppedEvent>()
            .With<BoardTag>()
            .End())
            {
                ref var grid = ref Get<Grid>(boardEntity);

                var width = grid.Value.GetLength(0);
                var height = grid.Value.GetLength(1);


                var lastMatchedTypeId = -1;
                for (int row = 0; row < height; row++)
                {
                    for (int column = 0; column < width; column++)
                    {
                        var cellEntity = grid.Value[column, row];

                        if (TryGet<PieceLink>(cellEntity, out var pieceLink))
                        {
                            if (pieceLink.Value.Unpack(World, out var pieceEntity))
                            {
                                var pieceTypeId = Get<PieceTypeId>(pieceEntity).Value;
                                if (pieceTypeId == lastMatchedTypeId)
                                {
                                    _matches.Add(pieceEntity);
                                    if (_matches.Count == 3)
                                    {
                                        for (int idx = 0; idx < _matches.Count; idx++)
                                        {
                                            later.Add<MatchedEvent>(_matches[idx]);
                                        }
                                        _matches.Clear();
                                        lastMatchedTypeId = -1;
                                    }
                                }
                                else
                                {
                                    _matches.Clear();
                                    lastMatchedTypeId = pieceTypeId;
                                    _matches.Add(pieceEntity);
                                }
                            }
                        }
                        else
                        {
                            _matches.Clear();
                            lastMatchedTypeId = -1;
                        }

                    }
                    _matches.Clear();
                    lastMatchedTypeId = -1;
                }
            }

            foreach (var pieceEntity in Filter()
            .With<MatchedEvent>()
            .End())
            {
                later.Add<DestroyedEvent>(pieceEntity);
            }

            foreach (var pieceEntity in Filter()
            .With<DestroyedEvent>()
            .With<Position>()
            .End())
            {
                ref var cellEntity = ref Get<CellLink>(pieceEntity).Value;
                Del<PieceLink>(cellEntity);
            }

            
        }

    }
}
