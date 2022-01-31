using Nanory.Lex;

namespace Client.Battle
{
    public sealed class AttackSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var attackerEntity in Filter()
            .With<MelleeAttackRequest>()
            .End())
            {
                ref var attackRequest = ref Get<MelleeAttackRequest>(attackerEntity);

                UnityEngine.Debug.Log("Attack!");
            }            
        }
    }
}
