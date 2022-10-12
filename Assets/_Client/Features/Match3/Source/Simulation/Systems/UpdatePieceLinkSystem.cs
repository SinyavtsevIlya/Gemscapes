using Nanory.Lex;
using UnityEngine;

namespace Client.Match3
{
    public sealed class UpdatePieceLinkSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var pieceEntity in Filter()
            .With<MovableTag>()
            .With<Position>()
            .With<CellPositionUpdatedEvent>()
            .End())
            {
                ref var position = ref Get<Position>(pieceEntity).Value;
                ref var grid = ref Get<Grid>(pieceEntity);
                ref var cells = ref Get<CellPositionUpdatedEvent>(pieceEntity);

                if (TryGet<PieceLink>(cells.PreviousCell, out var pieceLink) 
                    && pieceLink.Value.Unpack(World, out var previosCellPiece)
                    && previosCellPiece == pieceEntity)
                {
                    Del<PieceLink>(cells.PreviousCell);
                }

                GetOrAdd<PieceLink>(cells.CurrentCell).Value = World.PackEntity(pieceEntity);
            }
        }
    }
}
