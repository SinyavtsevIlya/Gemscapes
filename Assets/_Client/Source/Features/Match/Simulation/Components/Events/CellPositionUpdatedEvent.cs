using Nanory.Lex;

namespace Client.Match
{
    [OneFrame]
    
    public struct CellPositionUpdatedEvent
    {
        public int PreviousCell;
        public int CurrentCell;
    }
}