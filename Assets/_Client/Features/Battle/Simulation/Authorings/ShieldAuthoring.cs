using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Battle
{
    public class ShieldAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(int entity, GameObjectConversionSystem converstionSystem)
        {
            converstionSystem.World.Add<ShieldTag>(entity);
        }
    }
}
