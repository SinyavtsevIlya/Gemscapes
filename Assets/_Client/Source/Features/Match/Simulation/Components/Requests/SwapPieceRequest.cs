using Nanory.Lex;

namespace Client
{
    [OneFrame]
    [Battle]
    public struct SwapPieceRequest
    {
        public int PieceA;                
        public int PieceB;                
    }
}