﻿using Nanory.Lex;
using Nanory.Lex.Lifecycle;
using Client.Rpg;
using UnityEngine;

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
                    Later.Add<FinishBattleRequest>(ownerEntity);
                });
            }

            foreach (var ownerEntity in Filter()
            .With<OpenEvent<BattleScreen>>()
            .End())
            {
                var screen = Get<OpenEvent<BattleScreen>>(ownerEntity).Value;

                this.BindWidget(ownerEntity, screen.HealthWidget);

                if (Get<AttackableLink>(ownerEntity).Value.Unpack(World, out var enemyEntity))
                {
                    this.BindWidget(enemyEntity, screen.EnemyHealthWidget);
                }
            }

            foreach (var ownerEntity in Filter()
            .With<CloseEvent<BattleScreen>>()
            .End())
            {
                var screen = Get<CloseEvent<BattleScreen>>(ownerEntity).Value;

                this.UnbindWidget(ownerEntity, screen.HealthWidget);

                if (Get<AttackableLink>(ownerEntity).Value.Unpack(World, out var enemyEntity))
                {
                    this.UnbindWidget(enemyEntity, screen.EnemyHealthWidget);
                }
            }
        }
    }
}
