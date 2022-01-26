using Nanory.Lex;
using UnityEngine;

namespace Client.Match3
{
    
    [UpdateInGroup(typeof(TimersSystemGroup))]
    public class TickTimerSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var beginSim_ECB = GetCommandBufferFrom<BeginSimulationECBSystem>();

            foreach (var timerEntity in Filter()
            .With<Timer>()
            .With<TimerOwnerLink>()
            .End())
            {
                ref var timer = ref Get<Timer>(timerEntity);
                ref var timerOwnerLink = ref Get<TimerOwnerLink>(timerEntity);

                timer.CurrentTime--;

                if (timer.CurrentTime <= 0)
                {
                    if (timerOwnerLink.Value.Unpack(World, out var ownerEntity))
                    {
                        beginSim_ECB.Add(ownerEntity, timer.TimerContextComponentIndex);
                    }
                        
                    if (timer.IsInfinity == 0)
                    {
                        beginSim_ECB.DelEntity(timerEntity);
                    }
                    else
                    {
                        timer.CurrentTime = timer.Duration;
                    }
                }
            }
        }
    }
}
