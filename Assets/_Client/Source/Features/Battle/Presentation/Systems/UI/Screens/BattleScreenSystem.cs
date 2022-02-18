using Nanory.Lex;
using Nanory.Lex.Lifecycle;
using Client.Rpg;

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
                    Get<Health>(ownerEntity).Value--;
                    later.Add<Health.Changed>(ownerEntity);
                    later.Add<Changed<Health>>(ownerEntity);
                    later.Add<Changed<Name>>(ownerEntity);
                    return;
                    later.Add<FinishBattleRequest>(ownerEntity);
                });
            }

            foreach (var ownerEntity in Filter()
            .With<OpenEvent<BattleScreen>>()
            .End())
            {
                var screen = Get<OpenEvent<BattleScreen>>(ownerEntity).Value;

                this.BindWidget(ownerEntity, screen.HealthWidget);
            }

            foreach (var ownerEntity in Filter()
            .With<CloseEvent<BattleScreen>>()
            .End())
            {
                var screen = Get<CloseEvent<BattleScreen>>(ownerEntity).Value;

                this.UnbindWidget(ownerEntity, screen.HealthWidget);
            }
        }
    }
}
