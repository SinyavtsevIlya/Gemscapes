﻿using Nanory.Lex;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    [UpdateInGroup(typeof(TimersSystemGroup))]
    public class TickTimerSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            var beginSim_ECB = GetCommandBufferFrom<BeginSimulationECBSystem>();
            var beginSimDestructionECB = GetCommandBufferFrom<BeginSimulationDestructionECBSystem>();

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
                        beginSimDestructionECB.DelEntity(timerEntity);
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
