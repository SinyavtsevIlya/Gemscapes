using Nanory.Lex;

namespace Client
{
    [OneFrame]
    [Battle]
    public struct CellPositionUpdatedEvent
    {
        public int PreviousCell;
        public int CurrentCell;
    }
}