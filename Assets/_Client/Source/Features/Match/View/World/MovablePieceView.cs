using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

namespace Client.Match
{
    public class MovablePieceView : MonoBehaviour, IPointerClickHandler
    {
        #region Dependencies
        [SerializeField] private TMP_Text _label;
        [SerializeField] private bool _lerpEnabled;
        [SerializeField] private AnimationCurve _dumperCurve;
        [SerializeField] private float _dumperDuration;
        #endregion

        #region State
        private Vector2 _from;
        private Vector2 _to;
        private float _t;
        [SerializeField] private float _currentDumperPhase = 1;
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

        public void SetAsStopped()
        {
            _currentDumperPhase = 0;
        }

        public void SetLabel(string labelText)
        {
            _label.text = labelText;
        } 
        #endregion

        #region Messages
        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }

        private void Update()
        {
            if (_lerpEnabled)
            {
                _t += Time.deltaTime / Time.fixedDeltaTime;
                transform.position = Vector2.Lerp(_from, _to, _t);
            }
            if (_currentDumperPhase <= _dumperDuration)
            {
                transform.position += new Vector3(0f, _dumperCurve.Evaluate(_currentDumperPhase), 0f);
                _currentDumperPhase += Time.deltaTime;
            }
        } 
        #endregion
    }
}