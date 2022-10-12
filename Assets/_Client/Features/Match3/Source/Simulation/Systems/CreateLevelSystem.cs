using Nanory.Lex;
using Nanory.Lex.Conversion;
using Nanory.Lex.Lifecycle;
using UnityEngine;
using UnityEngine.Assertions;

namespace Client.Match3
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public sealed class CreateLevelSystem : EcsSystemBase
    {
        protected override void OnCreate()
        {
            var board =  Object.FindObjectOfType<BoardAuthoring>().gameObject;
            World.Convert(board.GetComponent<ConvertToEntity>(), ConversionMode.Instanced);
        }

        protected override void OnUpdate()
        {
        }
    }
}
