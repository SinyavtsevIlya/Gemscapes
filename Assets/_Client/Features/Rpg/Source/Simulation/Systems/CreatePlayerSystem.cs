using Nanory.Lex;
using Nanory.Lex.Conversion;
using Nanory.Lex.Lifecycle;
using UnityEngine;

namespace Client.Rpg
{
    public sealed class CreatePlayerSystem : EcsSystemBase
    {
        protected override void OnCreate()
        {
            var playerPrefab = Object.FindObjectOfType<PlayerAuthoring>().gameObject;
            World.Convert(playerPrefab.GetComponent<ConvertToEntity>(), ConversionMode.Instanced);
        }
        protected override void OnUpdate()
        {
            foreach (var playerEntity in Filter()
            .With<ScreensStorage>()
            .With<CreatedEvent>()
            .End())
            {
                this.OpenScreen<CoreScreen>(playerEntity);
            }
        }
    }
}
