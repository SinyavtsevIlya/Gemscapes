using Nanory.Lex;

namespace Client.Match
{
    [OneFrame]
    [Battle]
    public struct SwapPieceRequest
    {
        public int PieceA;                
        public int PieceB;                
    }
}