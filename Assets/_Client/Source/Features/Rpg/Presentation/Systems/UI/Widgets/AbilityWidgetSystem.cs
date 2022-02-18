using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Rpg
{
    [UpdateInGroup(typeof(PrimaryWidgetSystemGroup))]
    public sealed class AbilityWidgetSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var ownerEntity in Filter()
            .With<BindEvent<AbilityWidget>>()
            .End())
            {
                var widget = Get<BindEvent<AbilityWidget>>(ownerEntity).Value;

            }

            foreach (var ownerEntity in Filter()
            .With<UnbindEvent<AbilityWidget>>()
            .End())
            {
                var widget = Get<UnbindEvent<AbilityWidget>>(ownerEntity).Value;

            }
        }
    }
}
