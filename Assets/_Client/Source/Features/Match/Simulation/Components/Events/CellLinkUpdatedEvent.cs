using Nanory.Lex;

namespace Client
{
    [OneFrame]
    [Battle]
    public struct CellLinkUpdatedEvent
    {
        public int PreviousCell;
        public int CurrentCell;
    }
}