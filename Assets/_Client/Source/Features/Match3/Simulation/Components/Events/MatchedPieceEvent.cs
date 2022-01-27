using Nanory.Lex;

namespace Client.Match3
{
    /// <summary>
    /// Should be added on piece entity itself
    /// </summary>
    [OneFrame]
    public struct MatchedPieceEvent
    {
        [OneFrame] public struct TimerCompleted { }
    }
}