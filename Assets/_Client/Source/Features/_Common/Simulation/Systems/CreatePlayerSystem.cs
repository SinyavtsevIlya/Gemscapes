using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine;

namespace Client
{
    public sealed class CreatePlayerSystem : EcsSystemBase
    {
        protected override void OnCreate()
        {
            var playerPrefab = GameObject.FindObjectOfType<ScreenStorageAuthoring>().gameObject;
            World.Convert(playerPrefab);
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
