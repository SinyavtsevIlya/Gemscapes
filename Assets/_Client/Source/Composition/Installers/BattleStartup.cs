using UnityEngine;
using Nanory.Lex;
using System.Collections.Generic;

namespace Client
{
    public class Battle : TargetWorldAttribute { }

    class BattleStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystemSorter<Battle> _sorter;

        void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter<Battle>(_world);
            _systems.Add(_sorter.GetSortedSystems());

#if UNITY_EDITOR
            //_systems.Add(new Nanory.Lex.UnityEditorIntegration.EcsWorldDebugSystem());
#endif

            _systems.Init();
        }

        void FixedUpdate()
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
        }
    }
}