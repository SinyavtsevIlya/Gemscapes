using UnityEngine;
using Nanory.Lex;
using System.Collections.Generic;

namespace Client
{
    class ProjectStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystemSorter _sorter;

        void Start()
        {
            _world = new EcsWorldBase();
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter(_world);
            var featuredSystems = _sorter.GetFeaturedSystems<AppState.Feature>();
            _systems.Add(featuredSystems);

#if UNITY_EDITOR
            _systems.Add(new Nanory.Lex.UnityEditorIntegration.EcsWorldDebugSystem(featuredSystems));
#endif

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