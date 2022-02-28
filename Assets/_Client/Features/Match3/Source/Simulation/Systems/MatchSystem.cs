using Nanory.Lex;
using Nanory.Collections;
#if DEBUG
using Nanory.Lex.UnityEditorIntegration;
#endif
namespace Client.Match3
{
    public sealed class MatchSystem : EcsSystemBase
    {
        private ResizableArray<int> _matchedPieces = new ResizableArray<int>(8);
        private int _lastMatchedTypeId;

        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

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

                            if (!grid.TryGetCell(x, y, out var cellEntity))
                                continue;

                            //if (Has<GeneratorTag>(cellEntity))
                            //    continue;

                            if (this.TryGetPiece(cellEntity, out var pieceEntity))
                            {
                                var pieceTypeId = Get<PieceTypeId>(pieceEntity).Value;
                                if (pieceTypeId != _lastMatchedTypeId)
                                {
                                    Match();
                                    _lastMatchedTypeId = pieceTypeId;
                                }
                                _matchedPieces.Add(pieceEntity);
                            }
                            else
                            {
                                Match();
                            }
                        }
                        Match();
                    }
                }

                void Match()
                {
                    if (_matchedPieces.Count >= 3)
                    {
                        var matchEventEntity = NewEntity();
                        ref var matchEvent = ref later.Add<MatchEvent>(matchEventEntity);
                        ref var matchedPieces = ref matchEvent.MatchedPieces;
                        matchedPieces = Buffer<int>.Pool.Pop();

                        for (int idx = 0; idx < _matchedPieces.Count; idx++)
                        {
                            matchedPieces.Add(_matchedPieces[idx]);
                        }

                        later.Add<BoardLink>(matchEventEntity).Value = boardEntity;

                        for (int idx = 0; idx < _matchedPieces.Count; idx++)
                        {
                            var matchedPieceEntity = _matchedPieces[idx];

                            if (!Has<MatchedPieceTag>(matchedPieceEntity))
                            {
                                Add<MatchedPieceTag>(matchedPieceEntity);
                                later.AddDelayed<MatchedPieceEvent>(idx * 3, matchedPieceEntity);
                            }
                        }
                    }
                    _matchedPieces.Clear();
                    _lastMatchedTypeId = -1;
                }
            }
        }
    }
   
}
