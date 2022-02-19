using Nanory.Lex;

namespace Client
{
    [OneFrame]
    public struct PieceSwappedEvent
    {
        public int PieceA;
        public int PieceB;
    }
}