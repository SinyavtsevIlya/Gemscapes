using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
using Client.Battle;

namespace Client.Rpg
{
    public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(int playerEntity, GameObjectConversionSystem converstionSystem)
        {
            var world = converstionSystem.World;

            // TODO: temporary approach
            var enemyEntity = world.NewEntity();
            world.Add<Health>(enemyEntity).Value = 100;
            world.Add<AttackableLink>(playerEntity).Value = world.Dst.PackEntity(enemyEntity);
        }
    }
}
