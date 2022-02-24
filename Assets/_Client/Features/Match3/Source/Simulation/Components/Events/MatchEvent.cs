using Nanory.Lex;
using System.Collections.Generic;

namespace Client.Match3
{
    [OneFrame]
    public struct MatchEvent
    {
        public List<int> MatchedPieces;
    }
}