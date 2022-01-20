using Nanory.Lex;

namespace Client.Match
{
    [M3]
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
                var pieceAEntity = swapPieceRequest.PieceA;
                var pieceBEntity = swapPieceRequest.PieceB;
                var grid = Get<Grid>(pieceAEntity);

                Swap<Position>(pieceAEntity, pieceBEntity);
                Swap<Velocity>(pieceAEntity, pieceBEntity);
                Swap<GravityCellLink>(pieceAEntity, pieceBEntity);

                SwapTag<FallingTag>(pieceAEntity, pieceBEntity);

                var cellAEntity = grid.GetCellByPiece(World, pieceAEntity);
                var cellBEntity = grid.GetCellByPiece(World, pieceBEntity);

                Swap<PieceLink>(cellAEntity, cellBEntity);

                later.Add<MatchRequest>(Get<BoardLink>(cellAEntity).Value);
            }
        }
    }
}
