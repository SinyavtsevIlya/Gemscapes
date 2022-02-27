using Nanory.Lex;
using UnityEngine.SceneManagement;

namespace Client.AppState
{
    public sealed class AppStateMachineSystem : EcsSystemBase
    {
        protected override void OnCreate()
        {
            var appEntity = World.NewEntity();
            var later = GetCommandBufferFrom<BeginSimulationECBSystem>();

            Add<AppState>(appEntity).Value = AppState.Type.Preload;
            // Later we can wait for asynchronous loading some assets.
            later.Add<ChangeStateRequest>(appEntity).Value = AppState.Type.Rpg;

            UnityEngine.Application.targetFrameRate = 60;
        }

        protected override void OnUpdate()
        {
            foreach (var stateEntity in Filter()
            .With<ChangeStateRequest>()
            .With<AppState>()
            .End())
            {
                ref var changeStateRequest = ref Get<ChangeStateRequest>(stateEntity);
                ref var currentState = ref Get<AppState>(stateEntity);

                currentState.Value = changeStateRequest.Value;
                SceneManager.LoadScene(changeStateRequest.Value.ToString());

                Del<ChangeStateRequest>(stateEntity);
            }
        }
    }
}
