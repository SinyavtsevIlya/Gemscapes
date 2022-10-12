using Nanory.Lex;
using Client.Rpg;

namespace Client.Battle
{
    public sealed class AttackSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var attackerEntity in Filter()
            .With<MelleeAttackRequest>()
            .With<AttackableLink>()
            .With<Attack>()
            .End())
            {
                ref var attackRequest = ref Get<MelleeAttackRequest>(attackerEntity);
                ref var attackableEntity = ref Get<AttackableLink>(attackerEntity).Value;

                if (attackableEntity.Unpack(World, out var targetEntity))
                {
                    var isBlocked = TryGet<Blocks>(targetEntity, out var blocks);

                    if (isBlocked)
                    {
                        blocks.Count--;

                        if (blocks.Count == 0)
                        {
                            Later.Del<Blocks>(targetEntity);
                        }
                    }
                    else
                    {
                        Later.AddOrSet<DamageEvent>(targetEntity) = new DamageEvent()
                        {
                            Source = World.PackEntity(attackerEntity),
                            Value = Get<Attack>(attackerEntity).Value
                        };
                    }

                 
                }

            }            
        }
    }
}
