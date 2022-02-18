using Nanory.Lex.Stats;
using Nanory.Lex;

namespace Client.Rpg
{
    public struct MaxHealth : IStat
    {
        [OneFrame] public struct Changed { }

        public int Value;
        public int StatValue { get => Value; set => Value = value; }
    }
}