using Nanory.Lex;

namespace Client.Match
{
    [M3]
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

                if (TryGet<Buffer<GravityInputLink>>(cellEntity, out var gravityInputBuffer))
                {
                    var previousCellEntity = gravityInputBuffer.Values[0].Value;

                    if (TryGet<PieceLink>(previousCellEntity, out var upperPieceLink))
                    {
                        if (upperPieceLink.Value.Unpack(World, out var upperPieceEntity))
                        {
                            if (!Has<FallingTag>(upperPieceEntity))
                            {
                                if (!Has<MatchedTag>(upperPieceEntity))
                                {
                                    UnityEngine.Debug.Log("Add falling");
                                    Add<FallingTag>(upperPieceEntity);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
