using Nanory.Lex;

namespace Client.Match3
{
    public sealed class RequestPieceGenerationOnFallSystem : EcsSystemBase
    {
        private readonly System.Random _random;

        public RequestPieceGenerationOnFallSystem()
        {
            _random = new System.Random(0);
        }        

        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var generatorEntity in Filter()
            .With<GeneratorTag>()
            .With<CellPosition>()
            .Without<PieceLink>()
            .Without<GeneratePieceRequest>()
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
                            position.Value.RawValue -= gravityDirection.Value * position.Value.Divisor;
                            later.AddOrSet<GeneratePieceRequest>(generatorEntity) = new GeneratePieceRequest()
                            {
                                Velocity = velocity,
                                Position = position
                            };
                        }
                    }
                }
            }       
        }
    }
}

