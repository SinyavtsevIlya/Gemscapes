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
                if (swapRequest.PieceA.Unpack(World, out var pieceAEntity))
                {
                    var cellEntity = Get<Grid>(pieceAEntity).GetCellByPiece(World, pieceAEntity);
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
}
