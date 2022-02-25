using Nanory.Lex;
using Client.Battle;
using Client.Match3;
using Nanory.Lex.Timer;

namespace Client.Match3ToBattle
{
    public sealed class EnemyAutoAttackSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var swapEventEntity in Filter()
            .With<PieceSwappedEvent>()
            .End())
            {
                ref var pieceSwappedEvent = ref Get<PieceSwappedEvent>(swapEventEntity);

                var cellEntity = Get<Grid>(pieceSwappedEvent.PieceA).GetCellByPiece(World, pieceSwappedEvent.PieceA);
                var boardEntity = Get<BoardLink>(cellEntity).Value;
                ref var playerEntityLink = ref Get<BoardOwnerLink>(boardEntity).Value;
                if (playerEntityLink.Unpack(out var battleWorld, out var playerEntity))
                {
                    if (battleWorld.Get<AttackableLink>(playerEntity).Value.Unpack(battleWorld, out var enemyEntity))
                    {
                        UnityEngine.Debug.Log($"autoattack {pieceSwappedEvent.PieceA}");

                        (battleWorld as EcsWorldBase)
                             .GetCommandBufferFrom<BeginSimulationECBSystem>()
                             .AddDelayed<MelleeAttackRequest>(1f, enemyEntity);
                    }
                }
            }
        }
    }
}
