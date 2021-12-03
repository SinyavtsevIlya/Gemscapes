using UnityEngine;
using TMPro;

namespace Client.Match
{
	public class MovablePieceView : MonoBehaviour
    {
        [SerializeField] TMP_Text _label;

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
    }
}