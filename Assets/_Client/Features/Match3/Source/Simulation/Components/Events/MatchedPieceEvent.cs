using Nanory.Lex;

namespace Client.Match3
{
    /// <summary>
    /// Event of a piece entity becoming a part of the match (see <see cref = "MatchEvent"/>).
    /// Should be added on piece entity itself
    /// </summary>
    [OneFrame]
    public struct MatchedPieceEvent
    {
        [OneFrame] public struct TimerCompleted { }
    }
}