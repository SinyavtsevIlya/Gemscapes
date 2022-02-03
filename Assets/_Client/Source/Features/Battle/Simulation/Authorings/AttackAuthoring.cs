using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Battle
{
    public class AttackAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] int _value;

        public void Convert(int entity, GameObjectConversionSystem converstionSystem)
        {
            converstionSystem.World.Add<Attack>(entity).Value = _value;
        }
    }
}
