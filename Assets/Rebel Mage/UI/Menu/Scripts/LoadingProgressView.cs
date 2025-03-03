using UnityEngine;
using UnityEngine.UI;

namespace Rebel_Mage.UI
{
    public class LoadingProgressView : MonoBehaviour
    {
        [SerializeField] private Image _progressImage;

        public void UpdateProgress(float percentProgress)
        {
            _progressImage.fillAmount = percentProgress;
        }
    }
}