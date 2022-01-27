using Nanory.Lex;

namespace Client.Rpg
{
    [OneFrame]
    public struct DamageEvent
    {
        public FInt Value;
        public EcsPackedEntity Source;
    }
}