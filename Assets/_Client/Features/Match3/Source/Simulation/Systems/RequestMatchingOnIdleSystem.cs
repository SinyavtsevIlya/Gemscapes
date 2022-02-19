using Nanory.Lex;
#if DEBUG
using Nanory.Lex.UnityEditorIntegration;
#endif

namespace Client.Match3
{
    public sealed class RequestMatchingOnIdleSystem : EcsSystemBase
    {
        private EcsFilter _fallingPieces;

        protected override void OnCreate()
        {
            _fallingPieces = World.Filter<Position>().With<FallingTag>().End();
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
        }
    }
}
