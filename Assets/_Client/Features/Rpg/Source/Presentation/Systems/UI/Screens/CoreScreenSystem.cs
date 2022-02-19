using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Lifecycle;
using Client.Battle; // TODO: think of moving battle widget to battle feature

namespace Client.Rpg
{
    [UpdateInGroup(typeof(ScreenSystemGroup))]
    public sealed class CoreScreenSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var ownerEntity in Filter()
            .With<ScreensStorage>()
            .With<CreatedEvent>()
            .End())
            {
                var screen = this.GetScreen<CoreScreen>(ownerEntity);
                screen.BattleButton.onClick.AddListener(() => 
                {
                    // this.OpenScreen<AbilitiesScreen>(ownerEntity);
                    later.Add<BattleRequest>(ownerEntity);                                                        
                });
            }

            foreach (var ownerEntity in Filter()
            .With<OpenEvent<CoreScreen>>()
            .End())
            {
                var screen = Get<OpenEvent<CoreScreen>>(ownerEntity).Value;
                this.BindWidget(ownerEntity, screen.AbilityWidget);
            }

            foreach (var ownerEntity in Filter()
            .With<CloseEvent<CoreScreen>>()
            .End())
            {
                var screen = Get<CloseEvent<CoreScreen>>(ownerEntity).Value;
                this.UnbindWidget(ownerEntity, screen.AbilityWidget);
            }
        }
    }
}
