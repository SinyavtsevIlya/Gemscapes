using Nanory.Lex;

namespace Client.Rpg
{
    [OneFrame]
    public struct DamageEvent
    {
        public int Value;
        public EcsPackedEntity Source;
    }
}