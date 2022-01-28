using UnityEngine;
using Nanory.Lex;
using m3 =          Client.Match3.Feature;
using battle =      Client.Battle.Feature;
using m3toBattle =  Client.Match3.ToBattle.Feature;
using lifecycle =   Nanory.Lex.Lifecycle.Feature;

namespace Client
{
    class BattleStartup : MonoBehaviour
    {
        private EcsWorldBase _world;
        private EcsSystems _systems;
        private EcsSystemSorter _sorter;
        private EcsSystemGroup _presentation;
        private EcsSystemGroup _playback;

        [SerializeField] private int _jumpTicks;
        [SerializeField] private int _tickId;
        [SerializeField] private bool _tickManually;

        private void Start()
        {
            _world = new EcsWorldBase();
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter(_world);
            _systems.Add(_sorter.GetFeaturedSystems<m3, battle, m3toBattle, lifecycle>());
#if DEBUG
            _systems.Add(new Nanory.Lex.UnityEditorIntegration.EcsWorldDebugSystem());
#endif
            _presentation = _systems.AllSystems.FindSystem<PresentationSystemGroup>();
            _playback = _systems.AllSystems.FindSystem<Match3.PlaybackSimulationSystemGroup>();
            _systems.Init();

            Playback();
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

        private void Playback()
        {
            SetPresentationActive(false);

            while (_jumpTicks > _tickId)
            {
                _systems.Run();
                _tickId++;
            }

            SetPresentationActive(true);
        }

        private void SetPresentationActive(bool value)
        {
            _presentation.IsEnabled = value;
            _playback.IsEnabled = !value;
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
            _systems.Run();
            _tickId++;
        }
    }
}