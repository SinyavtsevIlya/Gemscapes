using Nanory.Lex;
using Client.Battle;

namespace Client.Match3.ToBattle
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
                             .AddOrSet<MelleeAttackRequest>(enemyEntity);
                    }
                }
            }
        }
    }
}
