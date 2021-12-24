using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
#if UNITY_EDITOR
using Nanory.Lex.UnityEditorIntegration;
#endif

namespace Client.Match
{
    public class PieceAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(int pieceEntity, GameObjectConversionSystem converstionSystem)
        {
            var world = converstionSystem.World;
            
            //pieceView.SetLabel(pieceEntity.ToString());
            world.Add<MovableTag>(pieceEntity);

            var isPrefab = gameObject.scene.name == null;

            if (isPrefab)
            {
                world.Add<GameObjectReference>(pieceEntity).Value = gameObject;
            }
        }
    }
}
