using UnityEngine;
using Nanory.Lex;
using System.Collections.Generic;
using System;

namespace Client
{
    class ProjectStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystemSorter _sorter;

        protected virtual Type[] FeatureTypes => new Type[]
        {
            typeof(AppState.Feature),
            #if UNITY_EDITOR
            typeof(Nanory.Lex.UnityEditorIntegration.Feature),
            #endif
        };

        void Start()
        {
            _world = new EcsWorldBase();
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter(_world);
            var scanner = new EcsTypesScanner();
            var systemTypes = scanner.ScanSystemTypes(FeatureTypes);
            var featuredSystems = _sorter.GetSortedSystems(systemTypes);
            _systems.Add(featuredSystems);

            _systems.Init();

            DontDestroyOnLoad(this);
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