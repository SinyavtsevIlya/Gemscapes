using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client
{
    public class ScreenStorageAuthoring : MonoBehaviour, IConvertToEntity
    {
        [SerializeField] private Transform _root;
        [SerializeField] private MonoBehaviour[] _screensPrefabs;
        [SerializeField] private Canvas _canvasPrefab;

        public MonoBehaviour[] ScreenInstances { get; private set; }

        private void Awake()
        {
            var canvasInstance = Object.Instantiate(_canvasPrefab, _root);
            ScreenInstances = new MonoBehaviour[_screensPrefabs.Length];
            for (int idx = 0; idx < _screensPrefabs.Length; idx++)
            {
                ScreenInstances[idx] = Object.Instantiate(_screensPrefabs[idx], canvasInstance.transform);
                ScreenInstances[idx].gameObject.SetActive(false);
            }
        }

        public void Convert(int entity, ConvertToEntitySystem converstionSystem)
        {
            converstionSystem.World.Dst.InitializeScreenStorage(entity, ScreenInstances);
        }
    }
}
