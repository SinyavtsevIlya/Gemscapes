using Nanory.Lex;
using UnityEngine;

namespace Client.Match3
{
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

                if (!TryGet<Buffer<GravityInputLink>>(cellEntity, out var gravityInputBuffer))
                    continue;

                var previousCellEntity = gravityInputBuffer.Values[0].Value;

                // TODO: iterate throw all gravity inputs and take first busy cell.

                if (World.TryGet<PieceLink>(previousCellEntity, out var upperPieceLink)
                    && upperPieceLink.Value.Unpack(World, out var upperPieceEntity)
                    && !Has<FallingTag>(upperPieceEntity))
                {
                    Add<FallingTag>(upperPieceEntity);
                }
            }
        }
    }
}
