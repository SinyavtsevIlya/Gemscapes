using Nanory.Lex;
using System.Collections.Generic;

namespace Client.Match3
{
    /// <summary>
    /// Event of matching a sequence of piece entities.
    /// Added to an empty entity.
    /// </summary>
    [OneFrame]
    public struct MatchEvent
    {
        public List<int> MatchedPieces;
    }
}