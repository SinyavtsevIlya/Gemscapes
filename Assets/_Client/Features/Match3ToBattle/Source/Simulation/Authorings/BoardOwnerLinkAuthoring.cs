using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Match3.ToBattle
{
    public class BoardOwnerLinkAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        private EcsPackedEntityWithWorld _ownerEntity;

        public void SetOwner(EcsPackedEntityWithWorld ownerEntity) => _ownerEntity = ownerEntity;

        public void Convert(int boardEntity, GameObjectConversionSystem converstionSystem)
        {
            converstionSystem.World.Add<BoardOwnerLink>(boardEntity).Value = _ownerEntity;
        }
    }
}
