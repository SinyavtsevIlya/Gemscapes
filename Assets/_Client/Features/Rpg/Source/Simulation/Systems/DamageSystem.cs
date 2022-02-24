using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Rpg    
{
    public sealed class DamageSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var damagableEntity in Filter()
            .With<DamageEvent>()
            .With<Health>()
            .End())
            {
                ref var damageEvent = ref Get<DamageEvent>(damagableEntity);
                ref var health = ref Get<Health>(damagableEntity);

                health.Value -= damageEvent.Value;

                later.Add<Health.Changed>(damagableEntity);

                if (health.Value <= 0)
                {
                    later.Add<DestroyedEvent>(damagableEntity);

                    if (damageEvent.Source.Unpack(World, out var sourceEntity))
                    {
                        Add<DestroyerLink>(damagableEntity).Value = World.PackEntity(sourceEntity);
                    }
                }
            }
        }
    }
}
