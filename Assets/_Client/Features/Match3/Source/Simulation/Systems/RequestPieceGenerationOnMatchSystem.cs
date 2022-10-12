using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public sealed class RequestPieceGenerationOnMatchSystem : EcsSystemBase
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
                var cellPosition = Get<CellPosition>(cellEntity).Value;

                if (!Has<GeneratorTag>(cellEntity))
                    continue;

                Later.AddOrSet<GeneratePieceRequest>(cellEntity) = new GeneratePieceRequest()
                {
                    Velocity = new Velocity() { Value = new Vector2IntFixed(0, 0, SimConstants.GridSubdivison) },
                    Position = new Position() { Value = new Vector2IntFixed(cellPosition.x, cellPosition.y, SimConstants.GridSubdivison) }
                };
            }
        }
    }
}
