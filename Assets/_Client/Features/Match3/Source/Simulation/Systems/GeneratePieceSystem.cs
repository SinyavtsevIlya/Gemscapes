using Nanory.Lex;
using Nanory.Lex.Lifecycle;
using UnityEngine;

namespace Client.Match3
{
}

namespace Client.Match3
{
    public sealed class GeneratePieceSystem : EcsSystemBase
    {
        private readonly System.Random _random;

        public GeneratePieceSystem()
        {
            _random = new System.Random(0);
        }

        protected override void OnUpdate()
        {
            foreach (var cellEntity in Filter()
            .With<GeneratePieceRequest>()
            .End())
            {
                ref var generatePieceRequest = ref Get<GeneratePieceRequest>(cellEntity);
                ref var grid = ref Get<Grid>(cellEntity);
                ref var cellPosition = ref Get<CellPosition>(cellEntity);
                ref var gravityDirection = ref Get<GravityDirection>(cellEntity);
                ref var boardEntity = ref Get<BoardLink>(cellEntity).Value;

                ref var availablePieces = ref Get<AvailablePieces>(boardEntity).Buffer;
                var index = _random.Next(0, availablePieces.Values.Count);
                var piecePrefab = availablePieces.Values[index];
                var newPieceEntity = World.Instantiate(piecePrefab);

                // TODO: replace with AuthoringUtility

                var velocity = generatePieceRequest.Velocity;
                var position = generatePieceRequest.Position;

                Add<PieceTypeId>(newPieceEntity).Value = piecePrefab;
                Add<Grid>(newPieceEntity) = grid;
                velocity.Value.RawValue /= 2;
                Add<Velocity>(newPieceEntity) = velocity;
                Add<GravityDirection>(newPieceEntity) = Get<GravityDirection>(cellEntity);
                Add<Position>(newPieceEntity) = position;
                Later.Add<PieceLink>(cellEntity).Value = World.PackEntity(newPieceEntity);
                Add<FallingTag>(newPieceEntity);
                Add<FallingStartedEvent>(newPieceEntity);
                Add<CreatedEvent>(newPieceEntity);
            }
        }
    }
}

