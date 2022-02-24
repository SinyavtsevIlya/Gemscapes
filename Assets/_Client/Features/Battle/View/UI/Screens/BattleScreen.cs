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

        public void MatchStatsWidgetArea()
        {
            var scale = GetComponentInParent<CanvasScaler>().scaleFactor;
            var height = ((Screen.height - Screen.width) / 2 ) * scale;
            StatsWidgetsArea.sizeDelta = new Vector2(StatsWidgetsArea.sizeDelta.x, height);
        }

#if UNITY_EDITOR
        private void Update()
        {
            MatchStatsWidgetArea();
        }
#endif
    }
}