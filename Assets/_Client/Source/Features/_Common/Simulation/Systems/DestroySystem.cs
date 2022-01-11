using Nanory.Lex;

namespace Client
{
    [AllWorld]
    [UpdateInGroup(typeof(OneFrameSystemGroup), OrderFirst = true)]
    public sealed class DestroySystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationDestructionECBSystem>();

            foreach (var destoyedEntity in Filter()
            .With<DestroyedEvent>()
            .End())
            {
                later.DelEntity(destoyedEntity);
            }
        }
    }
}
