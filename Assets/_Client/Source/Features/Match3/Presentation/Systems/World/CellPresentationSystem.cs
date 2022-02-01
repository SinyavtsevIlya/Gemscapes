using Nanory.Lex;
using Nanory.Lex.Conversion;
using Nanory.Lex.Lifecycle;
using UnityEngine;
#if DEBUG
using Nanory.Lex.UnityEditorIntegration;
#endif

namespace Client.Match3
{
    
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public sealed class CellPresentationSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();


            foreach (var cellEntity in Filter()
                .With<CellPosition>()
                .With<Mono<CellView>>()
                .With<CreatedEvent>()
                .End())
            {
#if DEBUG
                var cellView = Get<Mono<CellView>>(cellEntity).Value;
                cellView.SetLabel(cellEntity.ToString());
                EcsSystems.LinkDebugGameobject(World, cellEntity, cellView.gameObject);
#endif
            }
        }
    }
}
