using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{

    public class SelectableButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public Action Selected;
        public Action Deselected;

        public void OnSelect(BaseEventData eventData)
        {
            Selected?.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Deselected?.Invoke();
        }
    }
}
