using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public sealed class DropPiecesWhenMatchedSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<DestroyedEvent>()
            .End())
            {
                ref var grid = ref Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);

                if (!TryGet<Buffer<GravityInputLink>>(cellEntity, out var gravityInputBuffer))
                    continue;

                var upcomingCellEntity = gravityInputBuffer.Values[0].Value;
                
                if (TryGetUpcomingPieceEntity(upcomingCellEntity, out var upcomingPieceEntity))
                {
                    Add<FallingTag>(upcomingPieceEntity);
                    Add<FallingStartedEvent>(upcomingPieceEntity);
                }
            }
        }

        private bool TryGetUpcomingPieceEntity(int previousCellEntity, out int upperPieceEntity)
        {
            upperPieceEntity = -1;
            return TryGet<PieceLink>(previousCellEntity, out var upperPieceLink)
                                && upperPieceLink.Value.Unpack(World, out upperPieceEntity)
                                && !Has<FallingTag>(upperPieceEntity)
                                && !Has<MatchedPieceTag>(upperPieceEntity);
        }
    }
}
