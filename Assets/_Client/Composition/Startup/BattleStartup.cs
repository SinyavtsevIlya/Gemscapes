using System;
using Client.Match3;

using m3 =          Client.Match3.Feature;
using m3toBattle =  Client.Match3ToBattle.Feature;
using lifecycle =   Nanory.Lex.Lifecycle.Feature;

namespace Client
{
    class BattleStartup : BoardStartup
    {
        protected override Type[] FeatureTypes => new Type[] 
        {
            typeof(m3),
            typeof(m3toBattle),
            typeof(lifecycle)
        };

        protected override string WorldName => "Battle";
    }
}