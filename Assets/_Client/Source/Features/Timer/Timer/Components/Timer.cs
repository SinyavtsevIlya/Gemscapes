using Nanory.Lex;

namespace Client.Match
{
    public struct Timer 
    {
        public int CurrentTime;
        public int Duration;
        public int IsInfinity;
        public int TimerContextComponentIndex;

        public Timer(int duration, bool isInfinity, int timerContextComponentIndex)
        {
            Duration = duration;
            CurrentTime = duration;
            TimerContextComponentIndex = timerContextComponentIndex;
            IsInfinity = isInfinity ? 1 : 0;
        }
    }

    public struct TimerOwnerLink
    {
        public EcsPackedEntity Value;
    }
}