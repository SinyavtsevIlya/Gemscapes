using UnityEngine;
using UnityEngine.UI;
using Nanory.Lex.Stats;

namespace Client.Rpg.View
{
    public class ProgressBarWithLabel : MonoBehaviour, IStatView
    {
        private int _maxValue;

        public Image ProgressBar;
        public TMPro.TMP_Text Label;

        public IStatView SetMaxValue(int maxValue)
        {
            _maxValue = maxValue;
            return this;
        }

        public IStatView SetValue(int value)
        {
            ProgressBar.fillAmount = (float) value / _maxValue;
            Label.text = value.ToString();
            return this;
        }
    }
}