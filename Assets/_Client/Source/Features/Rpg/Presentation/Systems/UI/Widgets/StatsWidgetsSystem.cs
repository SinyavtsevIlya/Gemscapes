using Nanory.Lex;
using Nanory.Lex.Stats;

namespace Client.Rpg
{
    [UpdateInGroup(typeof(WidgetSystemGroup))]
    public class HealthWidgetSystem : StatWidgetSystem<Health, Health.Changed, MaxHealth, MaxHealth.Changed, View.ProgressBarWithLabel> { }
}
