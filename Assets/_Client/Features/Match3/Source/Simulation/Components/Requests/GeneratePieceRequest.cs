using Nanory.Lex;

namespace Client.Match3
{
    [OneFrame]
    public struct GeneratePieceRequest
    {
        public Position Position;
        public Velocity Velocity;
    }
}