using Nanory.Lex;

namespace Client.Match3
{
    [OneFrame]
    
    public struct CellPositionUpdatedEvent
    {
        public int PreviousCell;
        public int CurrentCell;
    }
}