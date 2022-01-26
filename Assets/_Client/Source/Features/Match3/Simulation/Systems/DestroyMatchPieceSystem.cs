using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public sealed class DestroyMatchPieceSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<MatchedEvent>()
            .End())
            {
                later.AddDelayed<DestroyedEvent>(10, pieceEntity);
            }
        }
    }
}
