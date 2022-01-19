using Nanory.Lex;
using System.Collections.Generic;
using System;

namespace Client.Match.Tests
{
    public partial class GravityTests
    {
        public class TestStartup<TWorld> where TWorld : TargetWorldAttribute
        {
            public EcsWorld World;
            public EcsSystems Systems;
            protected EcsSystemSorter<TWorld> Sorter;

            public TestStartup()
            {
                World = new EcsWorld();
                Systems = new EcsSystems(World);
                Sorter = new EcsSystemSorter<TWorld>(World);
                Systems.Add(Sorter.GetSortedSystems());
                Systems.Init();
            }

            public void OnStep(Action<IEcsRunSystem> step)
            {
                var systemGroups = new List<EcsSystemGroup>();
                Systems.AllSystems.FindAllSystemsNonAlloc(systemGroups);

                foreach (var systemGroup in systemGroups)
                {
                    systemGroup.Stepped += step;
                }
            }
        }
    }
}

