using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client
{
    public class ConvertToEntity : MonoBehaviour, IConvertToEntity
    {
        [SerializeField] bool _convertAndDestroy;

        public void Convert(int entity, ConvertToEntitySystem converstionSystem)
        {
            var convertables = GetComponents<IConvertToEntity>();
            foreach (var convertable in convertables)
            {
                if (convertable == this)
                    continue;

                convertable.Convert(entity, converstionSystem);
            }

            var isPrefab = gameObject.scene.name == null;
            if (_convertAndDestroy && !isPrefab)
                Destroy(this.gameObject);
        }
    }
}
