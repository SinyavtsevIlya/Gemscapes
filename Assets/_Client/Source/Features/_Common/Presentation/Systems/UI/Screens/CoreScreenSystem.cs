using UnityEngine;
using Nanory.Lex;

namespace Client
{
    [UpdateInGroup(typeof(ScreenSystemGroup))]
    public sealed class CoreScreenSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var ownerEntity in Filter()
            .With<ScreensStorage>()
            .With<CreatedEvent>()
            .End())
            {
                var screen = this.GetScreen<CoreScreen>(ownerEntity);
                screen.Button.onClick.AddListener(() => this.OpenScreen<AbilitiesScreen>(ownerEntity));
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
