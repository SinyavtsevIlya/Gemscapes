using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public sealed class DispatchGravitySystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var pieceEntity in Filter()
            .With<CellPositionUpdatedEvent>()
            .Without<FallingTag>()
            .End())
            {
                ref var cells = ref Get<CellPositionUpdatedEvent>(pieceEntity);

                ref var gravity = ref Get<GravityDirection>(cells.PreviousCell);
                var gravitiesOutputs = Get<Buffer<GravityOutputLink>>(cells.PreviousCell).Values;

                for (int idx = 0; idx < gravitiesOutputs.Count; idx++)
                {
                    var outputCellEntity = gravitiesOutputs[idx].Value;

                    if (!Has<PieceLink>(outputCellEntity))
                    {
                        later.Set<GravityDirection>(cells.PreviousCell) = Get<Buffer<GravityDirection>>(cells.PreviousCell).Values[idx];
                        //gravity = Get<Buffer<GravityDirection>>(cells.PreviousCell).Values[idx];
                        break;
                    }
                }
            }
        }
    }
}
