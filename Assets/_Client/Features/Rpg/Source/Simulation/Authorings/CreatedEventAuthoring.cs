using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
using Nanory.Lex.Lifecycle;

namespace Client.Rpg
{
    public class CreatedEventAuthoring : MonoBehaviour, IConvertToEntity
    {
        public void Convert(int entity, ConvertToEntitySystem converstionSystem)
        {
            var later = converstionSystem.GetCommandBufferFrom<BeginSimulationECBSystem>();
            later.Add<CreatedEvent>(entity);
        }
    }
}
