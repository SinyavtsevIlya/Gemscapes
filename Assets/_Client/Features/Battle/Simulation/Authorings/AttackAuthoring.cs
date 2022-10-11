using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Battle
{
    public class AttackAuthoring : MonoBehaviour, IConvertToEntity
    {
        [SerializeField] int _value;

        public void Convert(int entity, ConvertToEntitySystem converstionSystem)
        {
            converstionSystem.World.Add<Attack>(entity).Value = _value;
        }
    }
}
