using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Match
{
    public class PieceAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(int entity, GameObjectConversionSystem converstionSystem)
        {
            var world = converstionSystem.World;

            var pieceEntity = world.NewEntity();
            
            var pieceView = GetComponent<MovablePieceView>();
            pieceView.SetLabel(pieceEntity.ToString());
            world.Add<Mono<MovablePieceView>>(pieceEntity).Value = pieceView;
        }
    }
}
