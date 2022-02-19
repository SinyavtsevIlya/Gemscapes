using Nanory.Lex;
using System.Collections.Generic;
using System;

namespace Client.Match3.Tests
{
    public class TestStartup
    {
        public EcsWorld World;
        public EcsSystems Systems;
        protected EcsSystemSorter Sorter;

        public TestStartup(params Type[] featureTypes)
        {
            World = new EcsWorldBase();
            Systems = new EcsSystems(World);
            Sorter = new EcsSystemSorter(World);
            Systems.Add(Sorter.GetSortedSystems(new EcsTypesScanner().ScanSystemTypes(featureTypes)));
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

