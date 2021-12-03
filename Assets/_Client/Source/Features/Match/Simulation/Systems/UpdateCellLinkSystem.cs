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
                var roundedPiecePosition = Vector2Int.RoundToInt(position);

                if (grid.TryGetCell(roundedPiecePosition, out var cellEntity))
                {
                    if (cellLink.Value == cellEntity)
                        continue;

                    later.Add<CellLinkUpdatedEvent>(pieceEntity) = new CellLinkUpdatedEvent()
                    {
                        PreviousCell = cellLink.Value,
                        CurrentCell = cellEntity
                    };

                    Del<PieceLink>(cellLink.Value);
                    Add<PieceLink>(cellEntity).Value = World.PackEntity(cellEntity);

                    cellLink.Value = cellEntity;
                }
            }
        }
    }
}
