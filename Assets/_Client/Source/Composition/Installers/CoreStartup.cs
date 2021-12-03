using UnityEngine;
using Nanory.Lex;
using System.Collections.Generic;

namespace Client
{
    public class Core : TargetWorldAttribute, IDefaultWorldAttribute { }

    class CoreStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystemSorter<Core> _sorter;

        void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter<Core>(_world);
            _systems.Add(_sorter.GetSortedSystems());

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