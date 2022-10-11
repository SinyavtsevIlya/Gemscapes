using UnityEngine;
using UnityEngine.UI;
using Nanory.Lex.Stats;
using DG.Tweening;

namespace Client.Rpg.View
{
    public class ProgressBarWithLabel : MonoBehaviour, IStatView
    {
        private int _maxValue;
        private Color _endColor;
        public Image ProgressBar;
        public TMPro.TMP_Text Label;

        public IStatView SetMaxValue(int maxValue)
        {
            _maxValue = maxValue;
            _endColor = ProgressBar.color;
            return this;
        }

        public IStatView SetValue(int value)
        {   
            ProgressBar.color = Color.white;
            ProgressBar.DOKill();
            ProgressBar.DOColor(_endColor, 1f).SetEase(Ease.OutExpo);
            ProgressBar.DOFillAmount((float)value / _maxValue, 1f).SetEase(Ease.OutExpo);
            Label.text = $"{value}<color=#FFB4B4> / {_maxValue}</color>";
            return this;
        }

        public void Dispose()
        {
        }
    }
}