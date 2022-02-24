using Nanory.Lex;

namespace Client.Match3
{
    [OneFrame, System.Serializable]
    public struct SwapPieceRequest
    {
        public EcsPackedEntity PieceA;                
        public EcsPackedEntity PieceB;                
    }
}