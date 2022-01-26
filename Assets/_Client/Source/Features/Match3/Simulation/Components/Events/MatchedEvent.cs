using Nanory.Lex;

namespace Client.Match3
{
    /// <summary>
    /// Should be added on piece entity itself
    /// </summary>
    [OneFrame]
    public struct MatchedEvent
    {
        [OneFrame] public struct TimerCompleted { }
    }
}