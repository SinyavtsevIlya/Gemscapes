using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

namespace Client.Match
{
    public class MovablePieceView : MonoBehaviour, IPointerClickHandler
    {
        #region Dependencies
        [SerializeField] private TMP_Text _label;
        [SerializeField] private bool _lerpEnabled;
        [SerializeField] private AnimationCurve _damperCurve;
        [SerializeField] private AnimationCurve _matchCurve;
        [SerializeField] private float _damperDuration;
        #endregion

        #region State
        private Vector2 _from;
        private Vector2 _to;
        private float _t;
        private float _currentDamperPhase;
        #endregion

        #region API
        public event Action Clicked;

        public void SetPosition(Vector2 value)
        {
            if (_lerpEnabled)
            {
                _t = 0f;
                _from = transform.position;
                _to = value;
            }
            else
            {
                transform.position = value;
            }
        }

        public void Destroy(float duration)
        {
            transform.DOScale(0f, duration)
                .SetEase(_matchCurve)
                .OnComplete(OnMatchAnimationComplete);
        }

        public void SetAsStopped()
        {
            _currentDamperPhase = 0;
        }

        public void SetLabel(string labelText)
        {
            _label.text = labelText;
        }
        #endregion

        #region Private
        private void OnMatchAnimationComplete()
        {
            Destroy(gameObject);
        }
        #endregion

        #region Messages
        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }

        private void Awake()
        {
            _currentDamperPhase = _damperDuration;
        }

        private void Update()
        {
            if (_lerpEnabled)
            {
                _t += Time.deltaTime / Time.fixedDeltaTime;
                transform.position = Vector2.Lerp(_from, _to, _t);
            }
            if (_currentDamperPhase <= _damperDuration)
            {
                transform.position += new Vector3(0f, _damperCurve.Evaluate(_currentDamperPhase), 0f);
                _currentDamperPhase += Time.deltaTime;
            }
        } 
        #endregion
    }
}