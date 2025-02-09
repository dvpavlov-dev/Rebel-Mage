using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rebel_Mage.UI
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject UIOther;
        public Image ForegroundHp;
        public TextMeshProUGUI HpText;

        private float _maxHp;
        
        private void LateUpdate()
        {
            if (Camera.main != null)
            {
                UIOther.transform.LookAt(Camera.main.transform);
            }
        }

        public void Constructor(float maxHp)
        {
            _maxHp = maxHp;
            HpText.text = _maxHp.ToString(CultureInfo.CurrentCulture);
        }

        public void UpdateHp(float currentHp)
        {
            ForegroundHp.fillAmount = currentHp / _maxHp;
            HpText.text = currentHp.ToString(CultureInfo.CurrentCulture);
        }
    }
}