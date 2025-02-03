using System.Text;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LoadingCurtains : MonoBehaviour
{
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
        _loadingScreen.DOFade(1, 1);
    }

    public void Hide()
    {
        _loadingScreen.DOFade(0, 1);
    }

    public void UpdateProgress(float percentProgress)
    {
        _progressText.Clear();
        _progressText.Insert(0, percentProgress);
        _progressText.Append("%");
        
        UpdateProgressText(_progressText.ToString());
        _loadingProgress.UpdateProgress(percentProgress / 100);
    }
    
    private void UpdateProgressText(string progress)
    {
        _processText.text = progress;
    }
}
