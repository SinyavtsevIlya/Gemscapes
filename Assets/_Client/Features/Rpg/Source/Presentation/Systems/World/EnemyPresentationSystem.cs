using System;
using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Rpg
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class EnemyPresentationSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var enemyEntity in Filter()
            .With<Health>()
            .With<CreatedEvent>()
            .End())
            {
                ref var health = ref Get<Health>(enemyEntity);
                


            }
        }
    }
}
