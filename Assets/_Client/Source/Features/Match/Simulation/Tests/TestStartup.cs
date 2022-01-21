using Nanory.Lex;
using System.Collections.Generic;
using System;

namespace Client.Match.Tests
{
    public class TestStartup<TWorld> where TWorld : TargetWorldAttribute
    {
        public EcsWorld World;
        public EcsSystems Systems;
        protected EcsSystemSorter<TWorld> Sorter;

        private static IEnumerable<Type> _systemTypesCache;
        private static IEnumerable<Type> SystemTypesCache
        {
            get
            {
                if (_systemTypesCache == null)
                {
                    _systemTypesCache = new EcsTypesScanner().ScanSystemTypes(typeof(TWorld));
                }
                return _systemTypesCache;
            }
        }

        public TestStartup()
        {
            World = new EcsWorld();
            Systems = new EcsSystems(World);
            Sorter = new EcsSystemSorter<TWorld>(World, SystemTypesCache);
            Systems.Add(Sorter.GetSortedSystems());
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

