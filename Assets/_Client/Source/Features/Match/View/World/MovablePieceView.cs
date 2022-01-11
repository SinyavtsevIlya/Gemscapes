using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

namespace Client.Match
{
    public class MovablePieceView : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Dependencies
        [SerializeField] private Collider2D _collider;
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
        private Vector2 _dragStartPosition;
        #endregion

        #region API
        public event Action Clicked;
        public event Action<Vector2Int> Draged;

        public void SetFrom(Vector2 from) => _from = from;
        public void SetTo(Vector2 to) => _to = to;

        public void SetPosition(Vector2 value)
        {
            if (_lerpEnabled)
            {
                _t = 0f;
                _from = _to;
                _to = value;
            }
            else
            {
                transform.position = value;
            }
        }

        public void Destroy(float duration)
        {
            _collider.enabled = false;
            Clicked = null;
            Draged = null;

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

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragStartPosition = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var dragDirection = Vector2Int.RoundToInt((eventData.position - _dragStartPosition).normalized);
            Draged?.Invoke(dragDirection);
        }

        private void Awake()
        {
            _currentDamperPhase = _damperDuration;
        }

        private void Update()
        {
            if (!_lerpEnabled)
                return;

            _t += Time.deltaTime / Time.fixedDeltaTime;
            transform.position = Vector2.Lerp(_from, _to, _t);

            transform.position += new Vector3(0f, _damperCurve.Evaluate(_currentDamperPhase), 0f);
            _currentDamperPhase += Time.deltaTime;
        }
        #endregion
    }
}