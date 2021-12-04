using Nanory.Lex;
using Unity.Mathematics.FixedPoint;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class UnpinUpperPieceSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var pieceEntity in Filter()
            .With<CellLinkUpdatedEvent>()
            .End())
            {
                ref var cellLinkUpdatedEvent = ref Get<CellLinkUpdatedEvent>(pieceEntity);
                ref var grid = ref Get<Grid>(pieceEntity);

                if (grid.TryGetCell(Get<CellPosition>(cellLinkUpdatedEvent.PreviousCell).Value + new fp2(0, 1), out var upperCellEntity))
                {
                    if (World.TryGet<PieceLink>(upperCellEntity, out var upperPieceLink))
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
