using UnityEngine;
using Nanory.Lex;

namespace Client.Match
{
    public class Feature : FeatureBase { }
}

namespace Client
{
    class BattleStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystemSorter _sorter;
        private EcsSystemGroup _presentation;
        private EcsSystemGroup _playback;

        [SerializeField] private int _jumpTicks;
        [SerializeField] private int _tickId;
        [SerializeField] private bool _tickManually;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter(_world);
            _systems.Add(_sorter.GetSortedSystems<Match.Feature, Nanory.Lex.Lifecycle.Feature>());
#if DEBUG
            _systems.Add(new Nanory.Lex.UnityEditorIntegration.EcsWorldDebugSystem());
#endif
            _presentation = _systems.AllSystems.FindSystem<PresentationSystemGroup>();
            _playback = _systems.AllSystems.FindSystem<Match.PlaybackSimulationSystemGroup>();
            _systems.Init();

            Playback();
        }

#if DEBUG
        public int GetTickId() => _tickId;
#endif

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