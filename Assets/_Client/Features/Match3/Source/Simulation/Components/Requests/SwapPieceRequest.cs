using Nanory.Lex;

namespace Client.Match3
{
    [OneFrame, System.Serializable]
    public struct SwapPieceRequest
    {
        public int PieceA;                
        public int PieceB;                
    }
}