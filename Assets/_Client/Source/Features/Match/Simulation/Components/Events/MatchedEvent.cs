using Nanory.Lex;

namespace Client
{
    /// <summary>
    /// Should be added on piece entity itself
    /// </summary>
    [OneFrame]
    [Battle]
    public struct MatchedEvent
    {
        [OneFrame] [Battle] public struct TimerCompleted { }
    }
}