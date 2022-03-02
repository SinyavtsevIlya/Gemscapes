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

                ref var gravity = ref Get<GravityDirection>(pieceEntity).Value;
                ref var grid = ref Get<Grid>(pieceEntity);
                var roundedPosition = Get<CellPosition>(cellEntity).Value;

                if (grid.TryGetCell(roundedPosition - gravity, out var cellEntityToDispatch))
                {
                    DispatchGravity(cellEntityToDispatch);
                }
            }

            foreach (var pieceEntity in Filter()
            .With<DestroyedEvent>()
            .End())
            {
                ref var gravity = ref Get<GravityDirection>(pieceEntity).Value;
                ref var grid = ref Get<Grid>(pieceEntity);
                var roundedPosition = Get<Position>(pieceEntity).Value.ToVector2Int();

                if (grid.TryGetCell(roundedPosition - gravity, out var cellEntityToDispatch))
                {
                    DispatchGravity(cellEntityToDispatch);
                }
            }
        }

        private void DispatchGravity(int dispatchCell)
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            ref var gravity = ref Get<GravityDirection>(dispatchCell);
            var gravitiesOutputs = Get<Buffer<GravityOutputLink>>(dispatchCell).Values;

            for (int idx = 0; idx < gravitiesOutputs.Count; idx++)
            {
                var outputCellEntity = gravitiesOutputs[idx].Value;

                if (!TryGet<PieceLink>(outputCellEntity, out var pieceLink) || 
                    (pieceLink.Value.Unpack(World, out var pieceEntity)) && Has<DestroyedEvent>(pieceEntity))
                {
                    later.Set<GravityDirection>(dispatchCell) = Get<Buffer<GravityDirection>>(dispatchCell).Values[idx];
                    break;
                }
            }
        }
    }
}
