using Nanory.Lex;

namespace Client.Match
{
    /// <summary>
    /// Should be added on piece entity itself
    /// </summary>
    [OneFrame]
    [M3]
    public struct MatchedEvent
    {
        [OneFrame] [M3] public struct TimerCompleted { }
    }
}