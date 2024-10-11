using Rebel_Mage.Configs.Source;
using Rebel_Mage.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rebel_Mage.UI.Spell_Window
{
    public class SpellCollectionСell : MonoBehaviour
    {
        public TextMeshProUGUI LevelDescription;
        public TextMeshProUGUI SpellName;
        public Image SpellImage;

        private Image _backgroundCell;

        private SpellCollection _collection;
        private IRoundProcess _roundProcess;
        private SpellConfig _spell;

        public void InitSpellSetCell(SpellConfig spell, SpellCollection collection, IRoundProcess roundProcess)
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

            if (_spell.OpenAfterRound > _roundProcess.RoundsCompleted)
            {
                GetComponent<Image>().color = new Color(1, 0.5f, 0.5f, 1);
            }
            else
            {
                _backgroundCell.color = Color.white;
            }
        }
    }
}
