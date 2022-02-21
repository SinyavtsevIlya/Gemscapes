using Nanory.Lex;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public sealed class RecordInputSystem : EcsSystemBase
    {
        private int _tick;

        protected override void OnUpdate()
        {
            _tick++;

            foreach (var requestEntity in Filter()
            .With<SwapPieceRequest>()
            .End())
            {
                ref var swapRequest = ref Get<SwapPieceRequest>(requestEntity);
                var cellEntity = Get<Grid>(swapRequest.PieceA).GetCellByPiece(World, swapRequest.PieceA);
                var boardEntity = Get<BoardLink>(cellEntity).Value;
                var inputRecord = Get<InputRecord>(boardEntity);

                inputRecord.Frames.Add(new InputRecord.Frame() 
                {
                    Tick = _tick,
                    Swap = swapRequest 
                });
            }
        }
    }
}
