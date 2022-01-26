using Nanory.Lex;

namespace Client.Rpg    
{
    public sealed class DamageSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var damagableEntity in Filter()
            .With<DamageEvent>()
            .With<Health>()
            .End())
            {
                ref var damageEvent = ref Get<DamageEvent>(damagableEntity);
                ref var health = ref Get<Health>(damagableEntity);
            }
        }
    }
}
