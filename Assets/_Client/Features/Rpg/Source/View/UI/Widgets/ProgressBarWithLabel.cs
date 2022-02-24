using UnityEngine;
using UnityEngine.UI;
using Nanory.Lex.Stats;
using DG.Tweening;

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
            var endColor = ProgressBar.color;
            ProgressBar.color = Color.white;
            ProgressBar.DOColor(endColor, 1f).SetEase(Ease.OutExpo);
            ProgressBar.DOFillAmount((float)value / _maxValue, 1f).SetEase(Ease.OutExpo);
            Label.text = value.ToString();
            return this;
        }
    }
}