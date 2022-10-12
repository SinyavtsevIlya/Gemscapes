using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public sealed class DropPiecesWhenMatchedSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var pieceEntity in Filter()
            .With<DestroyedEvent>()
            .End())
            {
                ref var grid = ref Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);

                foreach (var gravityInput in Get<Buffer<GravityInputLink>>(cellEntity).Values)
                {
                    var upcomingCellEntity = gravityInput.Value;

                    if (TryGetUpcomingPieceEntity(upcomingCellEntity, out var upcomingPieceEntity))
                    {
                        Add<FallingTag>(upcomingPieceEntity);
                        Add<FallingStartedEvent>(upcomingPieceEntity);
                        break;
                    }
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
