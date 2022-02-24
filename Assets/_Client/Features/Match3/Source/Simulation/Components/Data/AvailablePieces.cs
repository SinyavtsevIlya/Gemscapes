using Nanory.Lex;

namespace Client.Match3
{
    public struct AvailablePieces : IEcsAutoReset<AvailablePieces>
    {
        /// <summary>
        /// Entity-prefabs of pieces that can be generated on a board.
        /// </summary>
        public Buffer<int> Buffer;
        /// <summary>
        /// Corresponding piece generation probability weights.
        /// </summary>
        public Buffer<int> Weights;

        public void AutoReset(ref AvailablePieces c)
        {
            c.Buffer.AutoReset(ref c.Buffer);
            c.Weights.AutoReset(ref c.Weights);
        }
    }
}