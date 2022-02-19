using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public sealed class UnregisterDestroyedPiecesSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var pieceEntity in Filter()
            .With<DestroyedEvent>()
            .With<Position>()
            .End())
            {
                var grid = Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);
                Del<PieceLink>(cellEntity);

                ref var pieceGravityDirection = ref Get<GravityDirection>(Get<GravityCellLink>(pieceEntity).Value).Value;
                if (grid.TryGetCell(Get<CellPosition>(cellEntity).Value + pieceGravityDirection, out var intendingCellEntity))
                {
                    Del<IntendingPieceLink>(intendingCellEntity);
                }
            }
        }
    }
}
