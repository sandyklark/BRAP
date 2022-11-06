using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuestItem : MonoBehaviour
    {
        public Image checkbox;
        public TextMeshProUGUI descriptionText;

        public void SetDescription(string description)
        {
            descriptionText.text = description;
        }

        public void SetComplete(bool complete)
        {
           var col = complete ? Color.green : Color.white;
           col.a = 0.4f;
           checkbox.color = col;
        }
    }
}
