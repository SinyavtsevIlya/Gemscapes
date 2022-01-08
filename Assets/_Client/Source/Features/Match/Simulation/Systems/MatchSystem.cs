using Nanory.Lex;
using Nanory.Collections;
#if DEBUG
using Nanory.Lex.UnityEditorIntegration;
#endif
namespace Client.Match
{
    [Battle]
    public sealed class MatchSystem : EcsSystemBase
    {
        private EcsFilter _fallingPieces;
        private ResizableArray<int> _matches;
        private int _lastMatchedTypeId;

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
                }
            }

            foreach (var boardEntity in Filter()
            .With<BoardTag>()
            .With<StoppedEvent>()
            .End())
            {
                later.Add<MatchRequest>(boardEntity);
            }

            foreach (var boardEntity in Filter()
            .With<MatchRequest>()
            .With<BoardTag>()
            .End())
            {
                ref var grid = ref Get<Grid>(boardEntity);

                _lastMatchedTypeId = -1;

                for (var iterationIdx = 0; iterationIdx < 2; iterationIdx++)
                {
                    var even = iterationIdx % 2;

                    var width = grid.Value.GetLength(iterationIdx % 2);
                    var height = grid.Value.GetLength((iterationIdx+1) % 2);

                    for (var row = 0; row < height; row++)
                    {
                        for (var column = 0; column < width; column++)
                        {
                            var x = even == 0 ? column : row;
                            var y = even == 0 ? row : column;
                            var cellEntity = grid.Value[x, y];

                            if (Has<GeneratorTag>(cellEntity))
                                continue;

                            if (this.TryGetPiece(cellEntity, out var pieceEntity))
                            {
                                var pieceTypeId = Get<PieceTypeId>(pieceEntity).Value;
                                if (pieceTypeId != _lastMatchedTypeId)
                                {
                                    Match();
                                    _lastMatchedTypeId = pieceTypeId;
                                }
                                _matches.Add(pieceEntity);
                            }
                            else
                            {
                                Match();
                            }
                        }
                        Match();
                    }
                }
            }

            void Match()
            {
                if (_matches.Count >= 3)
                {
                    for (int idx = 0; idx < _matches.Count; idx++)
                    {
                        var matchedEntity = _matches[idx];

                        if (!Has<MatchedTag>(matchedEntity))
                        {
                            Add<MatchedTag>(matchedEntity);
                            later.AddDelayed<MatchedEvent>(10 + idx * 4, matchedEntity);
                        }
                    }
                }
                _matches.Clear();
                _lastMatchedTypeId = -1;
            }

            foreach (var pieceEntity in Filter()
            .With<MatchedEvent>()
            .End())
            {
                later.AddDelayed<DestroyedEvent>(10, pieceEntity);
            }

            foreach (var pieceEntity in Filter()
            .With<DestroyedEvent>()
            .With<Position>()
            .End())
            {
                var grid = Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);
                Del<PieceLink>(cellEntity);
            }
        }
    }
    
    public class Matches
    {
        private readonly ResizableArray<int> _matches;
        private int _matchTypeId;

        public ResizableArray<int> GetMatches() => _matches;
        public int MatchTypeId => _matchTypeId;

        public void Add(int pieceEntity)
        {

        }
    }
}
