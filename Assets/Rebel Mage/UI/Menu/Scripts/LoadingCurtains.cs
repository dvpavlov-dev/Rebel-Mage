using System.Text;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Rebel_Mage.UI
{
    public class LoadingCurtains : MonoBehaviour
    {
        [SerializeField] private RectTransform _loadingAnimateImage;
        [SerializeField] private CanvasGroup _loadingScreen;
        [SerializeField] private TMP_Text _processText;
        [SerializeField] private LoadingProgressView _loadingProgress;
        [SerializeField] private TMP_Text _descriptionText;

        private readonly StringBuilder _progressText = new("0%");
    
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Init()
        {
            _loadingScreen.alpha = 0;
        }

        public void Show()
        {
            Debug.Log("Loading screen show");
        
            _loadingScreen
                .DOFade(1, 1);
        
            _loadingAnimateImage
                .DOLocalRotate(new Vector3(0, 0, -360), 3, RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetEase(Ease.OutBounce);
        }

        public void Hide()
        {
            Debug.Log("Loading screen hide");

            _loadingScreen
                .DOFade(0, 1);
        
            _loadingAnimateImage
                .DOKill();
        }

        public void UpdateProgress(float percentProgress)
        {
            _progressText.Clear();
            _progressText.Insert(0, percentProgress);
            _progressText.Append("%");
        
            UpdateProgressText(_progressText.ToString());
            _loadingProgress.UpdateProgress(percentProgress / 100);
        }

        public void UpdateDescription(string descriptionText)
        {
            _descriptionText.text = descriptionText;
        }
    
        private void UpdateProgressText(string progress)
        {
            _processText.text = progress;
        }
    }
}