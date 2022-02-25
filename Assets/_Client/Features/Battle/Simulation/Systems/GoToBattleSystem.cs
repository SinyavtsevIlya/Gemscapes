using Nanory.Lex;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using Client.Rpg;
using Client.Match3ToBattle;

namespace Client.Battle
{
    public sealed class GoToBattleSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            foreach (var playerEntity in Filter()
            .With<FinishBattleRequest>()
            .With<AttackableLink>()
            .End())
            {
                SetEventSystemActive(true);
                SceneManager.UnloadSceneAsync("Wolf");
                this.OpenScreen<CoreScreen>(playerEntity);
            }

            foreach (var playerEntity in Filter()
            .With<BattleRequest>()
            .End())
            {
                // TODO: get enemy name from the selected enemy link
                var enemyName = "Wolf";

                // TODO: this is a temp solution for debugging.
                // Enemy board scene should be loaded asynchronously 
                // using special Match3.BoardCompression approach. 
                var sceneHandle = SceneManager.LoadSceneAsync(enemyName, LoadSceneMode.Additive);

                SetEventSystemActive(false);

                sceneHandle.completed += (_) =>
                {
                    var scene = SceneManager.GetSceneByName(enemyName);
                    Assert.IsTrue(scene.IsValid(), $"No {enemyName} scene was found.");
                    var sceneObjects = scene.GetRootGameObjects();
                    foreach (var go in sceneObjects)
                    {
                        if (go.TryGetComponent<BoardOwnerLinkAuthoring>(out var boardAuthoring))
                        {
                            boardAuthoring.SetOwner(World.PackEntityWithWorld(playerEntity));
                            this.OpenScreen<BattleScreen>(playerEntity);
                            break;
                        }
                    }
                };
            }
        }

        // NOTE: We don't want two event systems persisting in the exact time.
        // And we also want to keep battle scenes testable.
        private static void SetEventSystemActive(bool value)
        {
            var activeScene = SceneManager.GetSceneByName(UnityIdents.Scenes.Rpg);
            foreach (var rootObject in activeScene.GetRootGameObjects())
            {
                if (rootObject.TryGetComponent<UnityEngine.EventSystems.EventSystem>(out var eventSystem))
                {
                    eventSystem.enabled = value;
                    break;
                }
            }
        }
    }
}
