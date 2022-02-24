using Nanory.Lex;

namespace Client.Match3
{
    [OneFrame]
    public struct PieceSwappedEvent
    {
        public int PieceA;
        public int PieceB;
    }
}