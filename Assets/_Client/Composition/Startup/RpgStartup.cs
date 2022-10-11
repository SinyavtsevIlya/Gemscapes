using UnityEngine;
using Nanory.Lex;
using System.Collections.Generic;
using System;

using lifecycle = Nanory.Lex.Lifecycle.Feature;
using timer = Nanory.Lex.Timer.Feature;
using rpg = Client.Rpg.Feature;
using battle = Client.Battle.Feature;

namespace Client
{
    class RpgStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystemSorter _sorter;

        protected virtual Type[] FeatureTypes => new Type[]
        {
            typeof(rpg),
            typeof(battle),
            typeof(lifecycle),
            typeof(timer),
            #if UNITY_EDITOR
            typeof(Nanory.Lex.UnityEditorIntegration.Feature)
            #endif
        };

        void Start()
        {
            _world = new EcsWorldBase(default, "Rpg");
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter(_world);
            var scanner = new EcsTypesScanner();
            var systemTypes = scanner.ScanSystemTypes(FeatureTypes);
            var featuredSystems = _sorter.GetSortedSystems(systemTypes);

            _systems.Add(featuredSystems);

            _systems.Init();
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
            _sorter.Dispose();
        }
    }
}