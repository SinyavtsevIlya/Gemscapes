using Nanory.Lex;
using Client.Battle;
using Client.Match3;

namespace Client.Match3ToBattle
{
    public sealed class BlockPlayerWhenShieldsMatchedSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var matchEntity in Filter()
            .With<MatchEvent>()
            .End())
            {
                ref var matchEvent = ref Get<MatchEvent>(matchEntity);
                
                foreach (var matchedPiece in matchEvent.MatchedPieces)
                {
                    
                    if (Has<ShieldTag>(matchedPiece))
                    {
                        var boardEntity = Get<BoardLink>(matchEntity).Value;
                        ref var playerEntityLink = ref Get<BoardOwnerLink>(boardEntity).Value;

                        if (playerEntityLink.Unpack(out var battleWorld, out var playerEntity))
                        {
                            (battleWorld as EcsWorldBase)
                                .GetCommandBufferFrom<BeginSimulationECBSystem>()
                                .AddOrSet<Blocks>(playerEntity).Count++;
                        }
                    }
                    break;
                }
            }
        }
    }
}
