using Nanory.Lex;

namespace Client.AppState
{
    [OneFrame]
    public struct ChangeStateRequest
    {
        public AppState.Type Value;
    }
}