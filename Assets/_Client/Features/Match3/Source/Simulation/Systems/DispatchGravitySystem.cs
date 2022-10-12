using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public sealed class DispatchGravitySystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var pieceEntity in Filter()
            .With<CellPositionUpdatedEvent>()
            .Without<FallingTag>()
            .End())
            {
                ref var cells = ref Get<CellPositionUpdatedEvent>(pieceEntity);
                var cellEntity = cells.PreviousCell;
                DispatchGravity(cellEntity);

                foreach (var gravityInput in Get<Buffer<GravityInputLink>>(cellEntity).Values)
                {
                    DispatchGravity(gravityInput.Value);
                }
            }

            foreach (var pieceEntity in Filter()
            .With<DestroyedEvent>()
            .End())
            {
                ref var grid = ref Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);

                foreach (var gravityInput in Get<Buffer<GravityInputLink>>(cellEntity).Values)
                {
                    DispatchGravity(gravityInput.Value);
                }
            }

            foreach (var pieceEntity in Filter()
            .With<Position>()
            .End())
            {
                ref var grid = ref Get<Grid>(pieceEntity);
                var cellEntity = grid.GetCellByPiece(World, pieceEntity);

                foreach (var gravityInput in Get<Buffer<GravityInputLink>>(cellEntity).Values)
                {
                    DispatchGravity(gravityInput.Value);
                }
            }
        }

        private void DispatchGravity(int dispatchCell)
        {
            ref var gravity = ref Get<GravityDirection>(dispatchCell);
            var gravitiesOutputs = Get<Buffer<GravityOutputLink>>(dispatchCell).Values;

            for (int idx = 0; idx < gravitiesOutputs.Count; idx++)
            {
                var outputCellEntity = gravitiesOutputs[idx].Value;

                if (!TryGet<PieceLink>(outputCellEntity, out var pieceLink) || 
                    (pieceLink.Value.Unpack(World, out var pieceEntity)) && (Has<DestroyedEvent>(pieceEntity) || Has<FallingTag>(pieceEntity)))
                {
                    Later.Set<GravityDirection>(dispatchCell) = Get<Buffer<GravityDirection>>(dispatchCell).Values[idx];
                    if (TryGet<PieceLink>(dispatchCell, out var dPieceLink))
                    {
                        if (dPieceLink.Value.Unpack(World, out var dPieceEntity))
                        {
                            Later.AddOrSet<FallingTag>(dPieceEntity);
                        }
                    }
                    break;
                }
            }
        }
    }
}
