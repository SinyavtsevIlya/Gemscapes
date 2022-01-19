using Nanory.Lex;

namespace Client.Match
{
    [M3]
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
            }
        }
    }
}
