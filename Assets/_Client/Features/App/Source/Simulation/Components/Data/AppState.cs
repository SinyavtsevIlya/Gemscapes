﻿namespace Client.AppState
{
    public struct AppState
    {
        public Type Value;

        public enum Type
        {
            Preload,
            Lobby,
            Rpg,
            Battle
        }   
    }
}