using Rebel_Mage.Configs.Source;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rebel_Mage.UI.Spell_Window
{
    public class SlotSpellInput : MonoBehaviour
    {
        [Header("Components")]
        public TextMeshProUGUI ButtonInput;
        public TypeSpell TypeSpellSlot;
        public Image SpellImage;
        public ClickableSpellInput ClickableButton;

        [Header("For empty slot")]
        public Sprite EmptySlotImage;

        private Image _backgroundCell;
        private SpellInputPanel _panel;
        private SpellConfig _spell;

        public void SpellSlotInit(string buttonInputName, TypeSpell typeSpell)
        {
            ButtonInput.text = buttonInputName;
            TypeSpellSlot = typeSpell;
        }

        public void InitSlot(SpellInputPanel panel)
        {
            _panel = panel;

            ClickableButton.OnClickButton ??= OnSelectedSlot;
        }

        public void SetSlot(SpellConfig spell)
        {
            SpellImage.sprite = spell.SpellImage;
            _spell = spell;
        }

        public void SetEmptySlot()
        {
            SpellImage.sprite = EmptySlotImage;
            _spell = null;
        }

        private void OnSelectedSlot(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _panel.SlotAction(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _panel.RemoveSpellFromInput(this);
            }
        }

        public void SelectedSlot()
        {
            _backgroundCell ??= GetComponent<Image>();

            _backgroundCell.color = Color.green;
        }

        public void UnselectedSlot()
        {
            _backgroundCell ??= GetComponent<Image>();

            _backgroundCell.color = Color.white;
        }

        public void WaitingSlot()
        {
            _backgroundCell ??= GetComponent<Image>();

            _backgroundCell.color = Color.yellow;
        }
        public SpellConfig GetSpell()
        {
            return _spell;
        }
    }
}
