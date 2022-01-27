using Nanory.Lex;
using Client.Battle;
using Client.Match3;

namespace Client.Match3ToBattle
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
                    // TODO: find out how to send later requests
                    battleWorld.Add<MelleeAttackRequest>(playerEntity);
                }
            }
        }
    }
}
