using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public sealed class DestroyMatchPieceSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var pieceEntity in Filter()
            .With<MatchedPieceEvent>()
            .End())
            {
                Later.AddDelayed<DestroyedEvent>(10, pieceEntity);
            }
        }
    }
}
