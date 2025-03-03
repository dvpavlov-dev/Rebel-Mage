using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rebel_Mage.UI
{
    public class ClickableSpellInput : MonoBehaviour, IPointerClickHandler
    {
        public Action<PointerEventData> OnClickButton;
    
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickButton?.Invoke(eventData);
        }
    }
}