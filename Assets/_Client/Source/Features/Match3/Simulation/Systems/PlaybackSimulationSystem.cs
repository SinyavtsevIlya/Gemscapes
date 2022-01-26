using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    [UpdateInGroup(typeof(RootSystemGroup), OrderLast = true)]
    public class PlaybackSimulationSystemGroup : EcsSystemGroup 
    {
        protected override void OnCreate(EcsSystems systems)
        {
            base.OnCreate(systems);
            IsEnabled = false;
        }
    }

    
    [UpdateInGroup(typeof(PlaybackSimulationSystemGroup))]
    public sealed class PlaybackSimulationSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var pieceEntity in Filter()
            .With<Mono<MovablePieceView>>()
            .With<DestroyedEvent>()
            .End())
            {
                ref var view = ref Get<Mono<MovablePieceView>>(pieceEntity);
                view.Value.gameObject.SetActive(false);
            }
        }
    }
}
