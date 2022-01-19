using Nanory.Lex;

namespace Client.Match
{
    [OneFrame]
    [M3]
    public struct CellPositionUpdatedEvent
    {
        public int PreviousCell;
        public int CurrentCell;
    }
}