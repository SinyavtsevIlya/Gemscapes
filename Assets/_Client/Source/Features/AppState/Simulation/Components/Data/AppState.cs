namespace Client
{
    public struct AppState
    {
        public Type Value;

        public enum Type
        {
            Preload,
            Lobby,
            Core,
            Battle
        }   
    }
}