using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PushItOut.Player
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject UIOther;
        public Image ForegraundHp;
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
            ForegraundHp.fillAmount = currentHp / _maxHp;
            HpText.text = currentHp.ToString();
        }
    }
}