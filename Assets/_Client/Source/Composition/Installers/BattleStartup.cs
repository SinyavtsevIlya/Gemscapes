﻿using UnityEngine;
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

        [SerializeField] private int _jumpTicks;
        [SerializeField] private int _tickId;
        [SerializeField] private bool _tickManually;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter<Battle>(_world);
            _systems.Add(_sorter.GetSortedSystems());

#if DEBUG
            _systems.Add(new Nanory.Lex.UnityEditorIntegration.EcsWorldDebugSystem());
#endif

            _systems.Init();
        }

        private void FixedUpdate()
        {
#if DEBUG
            if (_tickManually)
            {
                return;
            }
#endif

            Run();
        }

#if DEBUG
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                while (_jumpTicks > _tickId)
                {
                    Run();
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                _tickManually = !_tickManually;
            }

            if (_tickManually)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    Run();
                }
            }
        } 
#endif

        private void OnDestroy()
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

        private void Run()
        {
            _systems?.Run();
            _tickId++;
        }
    }
}