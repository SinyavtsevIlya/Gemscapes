using Nanory.Lex;
using Client.Rpg;

namespace Client.Battle
{
    public sealed class AttackSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

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
                            later.Del<Blocks>(targetEntity);
                        }
                    }
                    else
                    {
                        later.AddOrSet<DamageEvent>(targetEntity) = new DamageEvent()
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
