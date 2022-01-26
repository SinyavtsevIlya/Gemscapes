using Nanory.Lex;
using Nanory.Collections;
#if DEBUG
using Nanory.Lex.UnityEditorIntegration;
#endif
namespace Client.Match3
{
    public sealed class MatchSystem : EcsSystemBase
    {
        private ResizableArray<int> _matches = new ResizableArray<int>(8);
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
        }
    }
   
}
