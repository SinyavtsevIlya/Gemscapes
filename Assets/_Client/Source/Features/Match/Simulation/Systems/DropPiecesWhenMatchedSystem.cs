using Nanory.Lex;

namespace Client.Match
{
    [Battle]
    public sealed class DropPiecesWhenMatchedSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<DestroyedEvent>()
            .End())
            {
                UnityEngine.Debug.Log("try unpin");
                ref var cellEntity = ref Get<CellLink>(pieceEntity).Value;
                //TODO: for now let's just pretend there is only top down gravity
                ref var grid = ref Get<Grid>(pieceEntity);

                if (grid.TryGetCell(Get<CellPosition>(cellEntity).Value + UnityEngine.Vector2Int.up, out var upperCellEntity))
                {
                    if (TryGet<PieceLink>(upperCellEntity, out var upperPieceLink))
                    {
                        if (upperPieceLink.Value.Unpack(World, out var upperPieceEntity))
                        {
                            if (!Has<FallingTag>(upperPieceEntity))
                            {
                                if (!Has<MatchedTag>(upperPieceEntity))
                                {
                                    later.Add<FallingTag>(upperPieceEntity);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
