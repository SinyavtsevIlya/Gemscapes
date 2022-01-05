using Nanory.Lex;

namespace Client.Match
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

                var grid = Get<Grid>(swapPieceRequest.PieceA);

                var positionA = Get<Position>(swapPieceRequest.PieceA);
                var positionB = Get<Position>(swapPieceRequest.PieceB);

                Get<Position>(swapPieceRequest.PieceA) = positionB;
                Get<Position>(swapPieceRequest.PieceB) = positionA;

                var cellAEntity = grid.GetCellByPiece(World, swapPieceRequest.PieceA);
                var cellBEntity = grid.GetCellByPiece(World, swapPieceRequest.PieceB);

                var pieceLinkA = Get<PieceLink>(cellAEntity);
                var pieceLinkB = Get<PieceLink>(cellBEntity);

                Get<PieceLink>(cellAEntity) = pieceLinkB;
                Get<PieceLink>(cellBEntity) = pieceLinkA;

                later.Add<MatchRequest>(Get<BoardLink>(cellAEntity).Value);
            }
        }
    }
}
