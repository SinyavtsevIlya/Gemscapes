using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Match3ToBattle
{
    public class BoardOwnerLinkAuthoring : MonoBehaviour, IConvertToEntity
    {
        private EcsPackedEntityWithWorld _ownerEntity;

        public void SetOwner(EcsPackedEntityWithWorld ownerEntity) => _ownerEntity = ownerEntity;

        public void Convert(int boardEntity, ConvertToEntitySystem converstionSystem)
        {
            converstionSystem.World.Add<BoardOwnerLink>(boardEntity).Value = _ownerEntity;
        }
    }
}
