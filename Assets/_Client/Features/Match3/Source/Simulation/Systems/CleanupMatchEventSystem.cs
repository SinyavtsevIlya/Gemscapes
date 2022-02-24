using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    /// <summary>
    /// MatchEvent MatchedPieces should be cleared manually in order to be
    /// correctly passed throw the EntityCommandBuffer.
    /// </summary>
    [UpdateInGroup(typeof(OneFrameSystemGroup))]
    public sealed class CleanupMatchEventSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var matchEventEntity in Filter()
            .With<MatchEvent>()
            .With<DestroyedEvent>()
            .End())
            {
                ref var matchEvent = ref Get<MatchEvent>(matchEventEntity);
                matchEvent.MatchedPieces.Clear();
                Buffer<int>.Pool.Push(matchEvent.MatchedPieces);
            }        
        }
    }
}
