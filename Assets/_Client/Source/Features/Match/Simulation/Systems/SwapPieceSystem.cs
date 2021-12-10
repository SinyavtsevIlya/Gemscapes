using Nanory.Lex;

namespace Client
{
    [Battle]
    public sealed class SwapPieceSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationDestructionECBSystem>();

            foreach (var swapRequestEntity in Filter()
            .With<SwapPieceRequest>()
            .End())
            {
                ref var swapPieceRequest = ref Get<SwapPieceRequest>(swapRequestEntity);

                var positionA = Get<Position>(swapPieceRequest.PieceA);
                var positionB = Get<Position>(swapPieceRequest.PieceB);

                Get<Position>(swapPieceRequest.PieceA) = positionB;
                Get<Position>(swapPieceRequest.PieceB) = positionA;

                var cellA = Get<CellLink>(swapPieceRequest.PieceA).Value;
                var cellB = Get<CellLink>(swapPieceRequest.PieceB).Value;

                Get<CellLink>(swapPieceRequest.PieceA).Value = cellB;
                Get<CellLink>(swapPieceRequest.PieceB).Value = cellA;

                var pieceLinkA = Get<PieceLink>(cellA);
                var pieceLinkB = Get<PieceLink>(cellB);

                Get<PieceLink>(cellA) = pieceLinkB;
                Get<PieceLink>(cellB) = pieceLinkA;

                ref var cellEntity = ref Get<CellLink>(swapPieceRequest.PieceA).Value;
                UnityEngine.Debug.Log("Add match request 2");
                later.Add<MatchRequest>(Get<BoardLink>(cellEntity).Value);
            }
        }
    }
}
