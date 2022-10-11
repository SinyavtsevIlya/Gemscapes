using UnityEngine;
using Nanory.Lex;
using System;

using m3 =          Client.Match3.Feature;
using lifecycle =   Nanory.Lex.Lifecycle.Feature;

namespace Client.Match3
{
    class BoardStartup : MonoBehaviour
    {
        private EcsWorldBase _world;
        private EcsSystems _systems;
        private EcsSystemSorter _sorter;
        private EcsSystemGroup _presentation;
        private EcsSystemGroup _playback;
        private int _nextRecordFrameId;

        [SerializeField] private int _jumpTicks;
        [SerializeField] private int _tickId;
        [SerializeField] private bool _tickManually;
        [SerializeField] private TextAsset _recordAsset;

        private Nullable<InputRecord> _record;
        private Nullable<InputRecord> Record
        {
            get
            {
                if (_record == null)
                {
                    if (_recordAsset != null)
                    {
                        _record = JsonUtility.FromJson<InputRecord>(_recordAsset.text);
                    }
                    return _record;
                }

                return _record;
            }
        } 

        protected virtual string WorldName => "Board";

        protected virtual Type[] FeatureTypes => new Type[] 
        {
            typeof(m3),
            typeof(lifecycle),
            #if UNITY_EDITOR
            typeof(Nanory.Lex.UnityEditorIntegration.Feature),
            #endif
        };

        private void OnGUI()
        {
            if (Record.HasValue)
            {
                DisplayReplayLabel();
            }
        }

        private void Start()
        {
            _world = new EcsWorldBase(default, WorldName);
            _systems = new EcsSystems(_world);

            _sorter = new EcsSystemSorter(_world);
            var scanner = new EcsTypesScanner();
            var systemTypes = scanner.ScanSystemTypes(FeatureTypes);
            var featuredSystems = _sorter.GetSortedSystems(systemTypes);
            _systems.Add(featuredSystems);
            _presentation = _systems.AllSystems.FindSystem<PresentationSystemGroup>();
            _playback = _systems.AllSystems.FindSystem<PlaybackSimulationSystemGroup>();
            _systems.Init();
#if UNITY_EDITOR
            if (Record.HasValue)
            {
                DisableInput();
            }
#endif
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
                Playback();
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
            if (!Record.HasValue)
                return;


            Run();
            SetPresentationActive(false);

            while (_tickId < Record.Value.Frames[Record.Value.Frames.Count - 1].Tick)
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
            LogInputRecord();

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
            try
            {
                TryApplyInputRecord();
                _systems.Run();
                _tickId++;
            }
            catch (Exception ex)
            {
                LogInputRecord();
#if UNITY_EDITOR
                Debug.LogException(ex);
#else
                
#endif
            }
        }

        private void LogInputRecord()
        {
            foreach (var recordEntity in _world.Filter<InputRecord>().End())
            {
                ref var inputRecord = ref _world.Get<InputRecord>(recordEntity);
                var serializedRecord = JsonUtility.ToJson(inputRecord);
                var fileName = "record.json";
                var rootpath = Application.isEditor ? "Assets/Records/" : Application.persistentDataPath;
                System.IO.File.WriteAllText(System.IO.Path.Combine(rootpath, fileName), serializedRecord);
            };
        }

        private void TryApplyInputRecord()
        {
            if (!Record.HasValue)
                return;

            if (_nextRecordFrameId == Record.Value.Frames.Count)
                return;

            var nextFrame = Record.Value.Frames[_nextRecordFrameId];

            if (_tickId == nextFrame.Tick)
            {
                _nextRecordFrameId++;

                _world
                    .GetCommandBufferFrom<BeginSimulationECBSystem>()
                    .Add<SwapPieceRequest>(_world.NewEntity()) = new SwapPieceRequest()
                    {
                        PieceA = _world.PackEntity(nextFrame.Swap.A),
                        PieceB = _world.PackEntity(nextFrame.Swap.B)
                    };
            }
        }

        private void DisplayReplayLabel()
        {
            var style = new GUIStyle();
            style.normal.textColor = Color.Lerp(Color.red, Color.clear, (Mathf.Sin(Time.time * 2f) + 1f) / 2f);
            style.fontSize = 100;
            style.alignment = TextAnchor.MiddleCenter;
            var rect = new Rect(Screen.width / 2, Screen.height / 3, 0, 0);
            GUI.Label(rect, "REPLAY", style);

            var tickLabelStyle = new GUIStyle(style);
            tickLabelStyle.normal.textColor = Color.white;
            tickLabelStyle.fontSize = 50;
            var tickLabelRect = new Rect(rect);
            tickLabelRect.y += 80;
            GUI.Label(tickLabelRect, $"tick: {_tickId}", tickLabelStyle);
        }

        private static void DisableInput()
        {
            Debug.Log("disable input");
            FindObjectOfType<UnityEngine.EventSystems.EventSystem>().enabled = false;
        }
    }
}