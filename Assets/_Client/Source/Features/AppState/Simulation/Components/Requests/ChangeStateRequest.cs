using Nanory.Lex;

namespace Client
{
    [OneFrame]
    public struct ChangeStateRequest
    {
        public AppState.Type Value;
    }
}