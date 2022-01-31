using Nanory.Lex;
using Client.Match3.ToBattle;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine;

namespace Client.Battle
{
    public sealed class GoToBattleSystem : EcsSystemBase
    {
        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                SceneManager.UnloadSceneAsync("Wolf");
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
                    Debug.Log(boardAuthoring);
                    boardAuthoring.SetOwner(World.PackEntityWithWorld(playerEntity));
                };
            }
        }
    }
}
