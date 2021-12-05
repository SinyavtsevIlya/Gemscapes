using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class UnpinUpperPieceSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<FallingTag>()
            .End())
            {
                ref var grid = ref Get<Grid>(pieceEntity);
                ref var cellEntity = ref Get<CellLink>(pieceEntity).Value;
                ref var cellGravity = ref Get<GravityDirection>(cellEntity).Value;
                ref var cellPosition = ref Get<CellPosition>(cellEntity).Value;

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
