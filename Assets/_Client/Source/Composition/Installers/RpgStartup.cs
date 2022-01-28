using UnityEngine;
using Nanory.Lex;
using System.Collections.Generic;

using lifecycle = Nanory.Lex.Lifecycle.Feature;
using rpg = Client.Rpg.Feature;

namespace Client
{
    class RpgStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystemSorter _sorter;

        void Start()
        {
            _world = new EcsWorldBase();
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter(_world);
            _systems.Add(_sorter.GetFeaturedSystems<rpg, lifecycle>());

#if UNITY_EDITOR
            _systems.Add(new Nanory.Lex.UnityEditorIntegration.EcsWorldDebugSystem());
#endif

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