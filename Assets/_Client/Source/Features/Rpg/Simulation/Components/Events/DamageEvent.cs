using Nanory.Lex;

namespace Client.Rpg
{
    [OneFrame]
    public struct DamageEvent
    {
        public float Value;
        public EcsPackedEntity Source;
    }
}