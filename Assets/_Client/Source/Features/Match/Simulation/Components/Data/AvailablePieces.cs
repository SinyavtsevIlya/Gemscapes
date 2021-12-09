using Nanory.Lex;

namespace Client.Match
{
    public struct AvailablePieces : IEcsAutoReset<AvailablePieces>
    {
        public Buffer<int> Buffer;

        public void AutoReset(ref AvailablePieces c)
        {
            c.AutoReset(ref c);
        }
    }
}