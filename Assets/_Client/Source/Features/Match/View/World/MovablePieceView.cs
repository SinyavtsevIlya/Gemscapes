using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

namespace Client.Match
{
    public class MovablePieceView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private bool _lerpEnabled;

        private Vector2 _from;
        private Vector2 _to;
        private float _t;

        private Vector2 _previousPosition;
        private bool _dumpingEnabled;
        private Vector2 _springOrigin;

        public event Action Clicked;

        public void SetLabel(string labelText)
        {
            _label.text = labelText;
        }

        public void SetPosition(Vector2 value)
        {
            _previousPosition = transform.position;

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

        public void SetMovableState(bool state)
        {
            if (state == false)
            {
                
            }

            _dumpingEnabled = state;
        }

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

            if (_dumpingEnabled)
            {
                var delta = (Vector2)transform.position - _previousPosition;

                
            }
        }
    }
}