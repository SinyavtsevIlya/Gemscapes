using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    public sealed class GeneratePieceSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var generatorEntity in Filter()
            .With<GeneratorTag>()
            .With<CellPosition>()
            .Without<PieceLink>()
            .End())
            {
                ref var grid = ref Get<Grid>(generatorEntity);
                ref var cellPosition = ref Get<CellPosition>(generatorEntity);
                ref var gravityDirection = ref Get<GravityDirection>(generatorEntity);
                ref var boardEntity = ref Get<BoardLink>(generatorEntity).Value;

                if (grid.TryGetCell(cellPosition.Value + gravityDirection.Value, out var tendingCellEntity))
                {
                    if (TryGet<PieceLink>(tendingCellEntity, out var pieceLink))
                    {
                        if (pieceLink.Value.Unpack(World, out var pieceEntity))
                        {
                            var velocity = Get<Velocity>(pieceEntity);
                            var position = Get<Position>(pieceEntity);

                            ref var availablePieces = ref Get<AvailablePieces>(boardEntity).Buffer;
                            var index = Random.Range(0, availablePieces.Values.Count);
                            var piecePrefab = availablePieces.Values[index];
                            var newPieceEntity = World.Instantiate(piecePrefab);

                            Get<Velocity>(newPieceEntity) = velocity;
                            Get<Position>(newPieceEntity) = position;
                            Get<Position>(newPieceEntity).Value.Value -= gravityDirection.Value * position.Value.Divisor;
                        }
                    }
                }
            }       
        }
    }
}
