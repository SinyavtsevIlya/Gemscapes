using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [M3]
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

                if (TryGet<Buffer<GravityInputLink>>(cellEntity, out var gravityInputBuffer))
                {
                    var previousCellEntity = gravityInputBuffer.Values[0].Value;

                    if (World.TryGet<PieceLink>(previousCellEntity, out var upperPieceLink))
                    {
                        if (upperPieceLink.Value.Unpack(World, out var upperPieceEntity))
                        {
                            if (!Has<FallingTag>(upperPieceEntity))
                            {
                                Add<FallingTag>(upperPieceEntity);
                            }
                        }
                    }
                }
            }
        }
    }
}
