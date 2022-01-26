using Nanory.Lex;

namespace Client.Match3
{
    public static class TickTimerExtensions
    {
        /// <summary>
        /// Creates a child timer entity, and after specified tick-duration destroys or restarts it.
        /// </summary>
        /// <typeparam name="TCompleted">Context Tag which can be queried. Should be struct</typeparam>
        /// <param name="ecb">Entity Command Buffer to perform operations</param>
        /// <param name="ticksDelay">Timer duration in fixed ticks</param>
        /// <param name="ownerEntity">Owner of the timer</param>
        /// <param name="notifyOwner">If true, adds TimerCompletedEvent on the owner, otherwise - on the timer itself</param>
        /// <param name="isInfinity">If true, restarts the timer when time is out</param>
        /// <returns></returns>
        public static int AddDelayed<TCompleted>(this EntityCommandBuffer ecb,
                                                 int ticksDelay,
                                                 int ownerEntity,
                                                 bool isInfinity = false) where TCompleted : struct
        {
            var timerEntity = ecb.DstWorld.NewEntity();

            // NOTE: This is required for proper
            // registering the component in the pool
            ecb.DstWorld.GetPool<TCompleted>();
            ecb.BufferWorld.GetPool<TCompleted>();

            var timerContextComponentIndex = EcsComponent<TCompleted>.TypeIndex;
            ecb.Add<Timer>(timerEntity) = new Timer(ticksDelay, isInfinity, timerContextComponentIndex);
            ecb.Add<TimerOwnerLink>(timerEntity) = new TimerOwnerLink { Value = ecb.DstWorld.PackEntity(ownerEntity) };

            return timerEntity;
        }
    }
}
