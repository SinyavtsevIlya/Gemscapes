using UnityEngine;
using UnityEngine.UI;
using Client.Rpg.View;


namespace Client.Battle
{
    public class BattleScreen : MonoBehaviour
    {
        public Button CloseButton;
        public ProgressBarWithLabel HealthWidget;
        public ProgressBarWithLabel EnemyHealthWidget;
        public RectTransform StatsWidgetsArea;
    }
}