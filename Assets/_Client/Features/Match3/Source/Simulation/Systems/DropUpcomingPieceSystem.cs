using Nanory.Lex;
using UnityEngine;

namespace Client.Match3
{
    public sealed class DropUpcomingPieceSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var pieceEntity in Filter()
            .With<FallingTag>()
            .With<MovableTag>()
            .End())
            {
                ref var grid = ref Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);

                if (!TryGet<Buffer<GravityInputLink>>(cellEntity, out var gravityInputBuffer))
                    continue;

                foreach (var gravityInput in gravityInputBuffer.Values)
                {
                    var previousCellEntity = gravityInput.Value;

                    if (World.TryGet<PieceLink>(previousCellEntity, out var previousPieceLink)
                        && previousPieceLink.Value.Unpack(World, out var previousPieceEntity)
                        && !Has<FallingTag>(previousPieceEntity))
                    {
                        Add<FallingTag>(previousPieceEntity);
                        Add<FallingStartedEvent>(previousPieceEntity);
                        break;
                    }
                }
            }
        }
    }
}
