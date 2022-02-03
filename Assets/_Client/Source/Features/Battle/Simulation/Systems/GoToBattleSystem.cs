using Nanory.Lex;
using Client.Match3.ToBattle;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine;
using Client.Rpg;

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
                SceneManager.UnloadSceneAsync("Wolf");
                this.OpenScreen<CoreScreen>(playerEntity);
            }

            foreach (var playerEntity in Filter()
            .With<BattleRequest>()
            .End())
            {
                // TODO: get enemy from selected enemy link
                var enemyName = "Wolf";
                
                var sceneHandle = SceneManager.LoadSceneAsync(enemyName, LoadSceneMode.Additive);
                sceneHandle.completed += (_) => 
                {
                    var scene = SceneManager.GetSceneByName(enemyName);
                    Assert.IsTrue(scene.IsValid(), $"No {enemyName} scene was found.");

                    var sceneObjects = scene.GetRootGameObjects();

                    // TODO: this is a temp solution for debugging.
                    // Enemy board scene should be loaded asynchronously 
                    // using special Match3.BoardCompression approach. 
                    var boardGameObject = sceneObjects[1];

                    var boardAuthoring = boardGameObject.GetComponent<BoardOwnerLinkAuthoring>();
                    boardAuthoring.SetOwner(World.PackEntityWithWorld(playerEntity));

                    this.OpenScreen<BattleScreen>(playerEntity);
                };
            }
        }
    }
}
