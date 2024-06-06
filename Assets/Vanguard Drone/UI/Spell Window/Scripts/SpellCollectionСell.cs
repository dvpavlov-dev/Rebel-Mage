using PushItOut.Spell_system.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PushItOut.UI.Spell_Window
{
    public class SpellCollectionСell : MonoBehaviour
    {
        public TextMeshProUGUI SpellName;
        public Image SpellImage;

        private Image _backgroundCell;

        private SpellCollection _collection;
        private SpellConfig _spell;

        public void InitSpellSetCell(SpellConfig spell, SpellCollection collection)
        {
            SpellName.text = spell.SpellName;
            SpellImage.sprite = spell.SpellImage;
            _collection = collection;
            _spell = spell;
        }

        public SpellConfig GetSpell()
        {
            return _spell;
        }

        public void OnSelectedCell()
        {
            _collection.CellAction(this);
        }

        public void SelectedCell()
        {
            _backgroundCell ??= GetComponent<Image>();

            _backgroundCell.color = Color.green;
        }

        public void UnselectedCell()
        {
            _backgroundCell ??= GetComponent<Image>();

            _backgroundCell.color = Color.white;
        }
    }
}
