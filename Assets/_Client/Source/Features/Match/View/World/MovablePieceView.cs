using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

namespace Client.Match
{
	public class MovablePieceView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] TMP_Text _label;

        public event Action Clicked;

        public void SetLabel(string labelText)
        {
            _label.text = labelText;
        }

        public void SetPosition(Vector2 value)
        {
            transform.position = value;
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
    }
}