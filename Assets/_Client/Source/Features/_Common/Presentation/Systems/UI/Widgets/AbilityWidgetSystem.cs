﻿using UnityEngine;
using Nanory.Lex;

namespace Client
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