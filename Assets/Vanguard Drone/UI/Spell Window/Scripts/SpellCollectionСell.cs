using PushItOut.Spell_system.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vanguard_Drone.Infrastructure;

namespace PushItOut.UI.Spell_Window
{
    public class SpellCollectionСell : MonoBehaviour
    {
        public TextMeshProUGUI LevelDescription;
        public TextMeshProUGUI SpellName;
        public Image SpellImage;

        private Image _backgroundCell;

        private SpellCollection _collection;
        private SpellConfig _spell;
        private RoundProcess _roundProcess;

        public void InitSpellSetCell(SpellConfig spell, SpellCollection collection, RoundProcess roundProcess)
        {
            SpellName.text = spell.SpellName;
            SpellImage.sprite = spell.SpellImage;
            _collection = collection;
            _spell = spell;

            _roundProcess = roundProcess;

            LevelDescription.text = $"Откроется после {_spell.OpenAfterRound} раунда";
            if (_spell.OpenAfterRound > _roundProcess.RoundsCompleted)
            {
                GetComponent<Image>().color = new Color(1, 0.5f, 0.5f, 1);
            }
        }

        public SpellConfig GetSpell()
        {
            return _spell;
        }

        public void OnSelectedCell()
        {
            if (_spell.OpenAfterRound > _roundProcess.RoundsCompleted) return;
            
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
