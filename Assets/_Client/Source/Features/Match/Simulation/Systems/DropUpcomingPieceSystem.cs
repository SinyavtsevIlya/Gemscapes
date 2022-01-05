using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class DropUpcomingPieceSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<FallingTag>()
            .With<MovableTag>()
            .End())
            {
                ref var grid = ref Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);
                var cellGravity = Get<GravityDirection>(cellEntity).Value;
                var cellPosition = Get<CellPosition>(cellEntity).Value;

                if (grid.TryGetCell(cellPosition - cellGravity, out var previousCellEntity))
                {
                    if (World.TryGet<PieceLink>(previousCellEntity, out var upperPieceLink))
                    {
                        if (upperPieceLink.Value.Unpack(World, out var upperPieceEntity))
                        {
                            if (!Has<FallingTag>(upperPieceEntity))
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
