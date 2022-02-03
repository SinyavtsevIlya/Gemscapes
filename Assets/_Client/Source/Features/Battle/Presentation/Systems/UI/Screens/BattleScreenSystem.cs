using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Battle
{
    [UpdateInGroup(typeof(ScreenSystemGroup))]
    public sealed class BattleScreenSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var ownerEntity in Filter()
            .With<ScreensStorage>()
            .With<CreatedEvent>()
            .End())
            {
                var screen = this.GetScreen<BattleScreen>(ownerEntity);
                screen.CloseButton.onClick.AddListener(() => 
                {
                    var later = GetCommandBufferFrom<BeginSimulationECBSystem>();
                    later.Add<FinishBattleRequest>(ownerEntity);
                });
            }

            foreach (var ownerEntity in Filter()
            .With<OpenEvent<BattleScreen>>()
            .End())
            {
                var screen = Get<OpenEvent<BattleScreen>>(ownerEntity).Value;
                
            }

            foreach (var ownerEntity in Filter()
            .With<CloseEvent<BattleScreen>>()
            .End())
            {
                var screen = Get<CloseEvent<BattleScreen>>(ownerEntity).Value;

            }
        }
    }
}
