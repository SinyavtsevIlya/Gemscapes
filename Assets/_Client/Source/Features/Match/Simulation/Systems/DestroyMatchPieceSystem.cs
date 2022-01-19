using Nanory.Lex;

namespace Client.Match
{
    [M3]
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
