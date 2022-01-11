using Nanory.Lex;

namespace Client.Match
{
    [OneFrame]
    [Battle]
    public struct CellPositionUpdatedEvent
    {
        public int PreviousCell;
        public int CurrentCell;
    }
}