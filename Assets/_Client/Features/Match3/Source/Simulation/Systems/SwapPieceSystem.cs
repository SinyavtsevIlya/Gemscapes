using Nanory.Lex;

namespace Client.Match3
{
    public sealed class SwapPieceSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var swapRequestEntity in Filter()
            .With<SwapPieceRequest>()
            .End())
            {
                ref var swapPieceRequest = ref Get<SwapPieceRequest>(swapRequestEntity);

                if (swapPieceRequest.PieceA.Unpack(World, out var pieceAEntity) &&
                    swapPieceRequest.PieceB.Unpack(World, out var pieceBEntity))
                {
                    var grid = Get<Grid>(pieceAEntity);

                    Swap<Position>(pieceAEntity, pieceBEntity);
                    Swap<Velocity>(pieceAEntity, pieceBEntity);
                    Swap<GravityCellLink>(pieceAEntity, pieceBEntity);

                    SwapTag<FallingTag>(pieceAEntity, pieceBEntity);

                    var cellAEntity = grid.GetCellByPiece(World, pieceAEntity);
                    var cellBEntity = grid.GetCellByPiece(World, pieceBEntity);

                    Swap<PieceLink>(cellAEntity, cellBEntity);
                    Swap<IntendingPieceLink>(cellAEntity, cellBEntity);

                    later.Add<PieceSwappedEvent>(NewEntity()) = new PieceSwappedEvent()
                    {
                        PieceA = pieceAEntity,
                        PieceB = pieceBEntity
                    };

                    later.AddOrSet<MatchRequest>(Get<BoardLink>(cellAEntity).Value);
                }
            }
        }
    }
}
