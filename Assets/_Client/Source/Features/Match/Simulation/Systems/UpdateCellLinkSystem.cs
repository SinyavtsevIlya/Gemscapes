using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class UpdateCellLinkSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<CellLink>()
            .With<Position>()
            .End())
            {
                ref var cellLink = ref Get<CellLink>(pieceEntity);
                ref var position = ref Get<Position>(pieceEntity).Value;
                ref var grid = ref Get<Grid>(pieceEntity);
                var roundedPiecePosition = position.ToVector2Int();

                if (grid.TryGetCell(roundedPiecePosition, out var currentCellEntity))
                {
                    if (cellLink.Value == currentCellEntity)
                        continue;

                    later.Add<CellLinkUpdatedEvent>(pieceEntity) = new CellLinkUpdatedEvent()
                    {
                        PreviousCell = cellLink.Value,
                        CurrentCell = currentCellEntity
                    };

                    Del<PieceLink>(cellLink.Value);
                    if (!Has<PieceLink>(currentCellEntity))
                    {
                        Add<PieceLink>(currentCellEntity).Value = World.PackEntity(pieceEntity);
                    }

                    cellLink.Value = currentCellEntity;
                }
            }
        }
    }
}
