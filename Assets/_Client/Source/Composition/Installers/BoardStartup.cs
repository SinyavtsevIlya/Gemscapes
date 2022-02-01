using UnityEngine;
using Nanory.Lex;

using m3 =          Client.Match3.Feature;
using lifecycle =   Nanory.Lex.Lifecycle.Feature;

namespace Client
{
    class BoardStartup : MonoBehaviour
    {
        private EcsWorldBase _world;
        private EcsSystems _systems;
        private EcsSystemSorter _sorter;
        private EcsSystemGroup _presentation;
        private EcsSystemGroup _playback;

        [SerializeField] private int _jumpTicks;
        [SerializeField] private int _tickId;
        [SerializeField] private bool _tickManually;

        protected virtual string WorldName => "Board";

        protected virtual System.Type[] FeatureTypes => new System.Type[] 
        {
            typeof(m3),
            typeof(lifecycle)
        };

        private void Start()
        {
            _world = new EcsWorldBase(default, WorldName);
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter(_world);
            var scanner = new EcsTypesScanner();
            var systemTypes = scanner.ScanSystemTypes(FeatureTypes);
            var featuredSystems = _sorter.GetSortedSystems(systemTypes);
            _systems.Add(featuredSystems);
#if DEBUG
            _systems.Add(new Nanory.Lex.UnityEditorIntegration.EcsWorldDebugSystem(featuredSystems));
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
            Run();
            SetPresentationActive(false);

            while (_tickId < _jumpTicks - 1)
            {
                Run();
            }

            // NOTE: We want the last tick to be visualized
            // to immediately see playback state
            SetPresentationActive(true);
            Run();
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
            Debug.Log("run simulation");
            _systems.Run();
            _tickId++;
        }
    }
}