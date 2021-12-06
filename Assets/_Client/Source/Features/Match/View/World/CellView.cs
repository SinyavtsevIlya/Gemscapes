using UnityEngine;
using TMPro;

namespace Client.Match
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;

        public void SetLabel(string labelText)
        {
            _label.text = labelText;
        }
    }
}
