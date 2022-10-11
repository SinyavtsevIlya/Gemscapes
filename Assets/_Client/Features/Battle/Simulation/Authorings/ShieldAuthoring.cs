using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Battle
{
    public class ShieldAuthoring : MonoBehaviour, IConvertToEntity
    {
        public void Convert(int entity, ConvertToEntitySystem converstionSystem)
        {
            converstionSystem.World.Add<ShieldTag>(entity);
        }
    }
}
