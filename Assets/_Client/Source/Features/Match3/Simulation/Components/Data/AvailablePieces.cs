using Nanory.Lex;

namespace Client.Match3
{
    public struct AvailablePieces : IEcsAutoReset<AvailablePieces>
    {
        public Buffer<int> Buffer;

        public void AutoReset(ref AvailablePieces c)
        {
            Buffer.AutoReset(ref c.Buffer);
        }
    }
}