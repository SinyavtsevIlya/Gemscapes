using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

namespace Client.Match
{
	public class MovablePieceView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] TMP_Text _label;

        Vector2 _from;
        Vector2 _to;
        float _t;

        public event Action Clicked;

        public void SetLabel(string labelText)
        {
            _label.text = labelText;
        }

        public void SetPosition(Vector2 value)
        {
            _t = 0f;
            _from = transform.position;
            transform.position = _from;
            _to = value;
        }

        public void SetMovableState(bool state)
        {
            //GetComponent<MeshRenderer>().material.color = state ? Color.white : Color.black;
        }

        public void SetStartMovementState()
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }

        private void Update()
        {
            _t += Time.deltaTime / Time.fixedDeltaTime;
            transform.position = Vector2.Lerp(_from, _to, _t);
        }
    }
}