using Nanory.Lex;

namespace Client
{
    /// <summary>
    /// "Intending piece" - is a piece that is currently moving to a cell.
    /// Intending is meant relative to the cell.
    /// </summary>
    public struct IntendingPieceLink
    {
        public EcsPackedEntity Value;
    }
}