using Nanory.Lex;

namespace Client.Match3
{
    /// <summary>
    /// Event of changing entity's <see cref="CellPosition"/>.
    /// Adds on the piece entity.
    /// </summary>
    [OneFrame]
    public struct CellPositionUpdatedEvent
    {
        public int PreviousCell;
        public int CurrentCell;
    }
}