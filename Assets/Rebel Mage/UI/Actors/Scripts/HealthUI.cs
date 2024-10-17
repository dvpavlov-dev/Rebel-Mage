using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rebel_Mage.Player
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject UIOther;
        public Image ForegroundHp;
        public TextMeshProUGUI HpText;

        private float _maxHp;
        
        private void Update()
        {
            if (Camera.main != null)
            {
                UIOther.transform.LookAt(Camera.main.transform);
            }
        }

        public void Constructor(float _maxHp)
        {
            this._maxHp = _maxHp;
            HpText.text = this._maxHp.ToString();

            // if (Camera.main != null)
            // {
            //     cameraPosition = Camera.main.transform;
            //     Debug.Log($"CameraPos: {cameraPosition}");
            // }
        }

        public void UpdateHp(float currentHp)
        {
            ForegroundHp.fillAmount = currentHp / _maxHp;
            HpText.text = currentHp.ToString();
        }
    }
}