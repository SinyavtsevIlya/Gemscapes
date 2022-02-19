using Nanory.Lex.Stats;
using Nanory.Lex;

namespace Client.Rpg
{
    public struct Health : IStat
    {
        [OneFrame] public struct Changed { }

        public int Value;
        public int StatValue { get => Value; set => Value = value; }

        public static implicit operator int(Health src)
        {
            return src.Value;
        }
    }
}