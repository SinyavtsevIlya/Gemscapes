using Nanory.Lex;
using Client.Battle;

namespace Client.Match3.ToBattle
{
    public sealed class RequestAttackOnMatchSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var matchEntity in Filter()
            .With<MatchEvent>()
            .End())
            {
                ref var matchEvent = ref Get<MatchEvent>(matchEntity);
                var boardEntity = Get<BoardLink>(matchEntity).Value;
                ref var playerEntityLink = ref Get<BoardOwnerLink>(boardEntity).Value;

                if (playerEntityLink.Unpack(out var battleWorld, out var playerEntity))
                {
                    (battleWorld as EcsWorldBase)
                        .GetCommandBufferFrom<BeginSimulationECBSystem>()
                        .AddOrSet<MelleeAttackRequest>(playerEntity);
                }
            }
        }
    }
}
