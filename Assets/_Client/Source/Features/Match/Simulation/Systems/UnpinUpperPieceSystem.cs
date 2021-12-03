using Nanory.Lex;
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

                if (grid.TryGetCell(Get<CellPosition>(cellLinkUpdatedEvent.PreviousCell).Value + Vector2Int.up, out var upperCellEntity))
                {
                    if (World.TryGet<PieceLink>(upperCellEntity, out var upperPieceLink))
                    {
                        Debug.Log($"upperCellEntity is {upperCellEntity}");
                        if (upperPieceLink.Value.Unpack(World, out var upperPieceEntity))
                        {
                            if (!Has<FallingTag>(upperPieceEntity))
                            {
                                
                            }
                            Add<FallingTag>(upperPieceEntity);
                        }
                    }
                }
            }
        }
    }
}
