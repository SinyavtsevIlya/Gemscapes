using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Rpg
{
    [UpdateInGroup(typeof(ScreenSystemGroup))]
    public sealed class AbilitiesScreenSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var ownerEntity in Filter()
            .With<ScreensStorage>()
            .With<CreatedEvent>()
            .End())
            {
                var screen = this.GetScreen<AbilitiesScreen>(ownerEntity);
                screen.Button.onClick.AddListener(() => this.OpenScreen<CoreScreen>(ownerEntity));
            }

            foreach (var ownerEntity in Filter()
            .With<OpenEvent<AbilitiesScreen>>()
            .End())
            {
                var screen = Get<OpenEvent<AbilitiesScreen>>(ownerEntity).Value;
                this.BindWidget(ownerEntity, screen.AbilityWidget);
            }

            foreach (var ownerEntity in Filter()
            .With<CloseEvent<AbilitiesScreen>>()
            .End())
            {
                var screen = Get<CloseEvent<AbilitiesScreen>>(ownerEntity).Value;
                this.UnbindWidget(ownerEntity, screen.AbilityWidget);
            }
        }
    }
}
