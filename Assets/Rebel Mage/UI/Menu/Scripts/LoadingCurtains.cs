using UnityEngine;
using DG.Tweening;
using TMPro;

public class LoadingCurtains : MonoBehaviour
{
    [SerializeField] private CanvasGroup _loadingScreen;
    [SerializeField] private TMP_Text _processText;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Show()
    {
        _loadingScreen.DOFade(1, 1);
    }

    public void Hide()
    {
        _loadingScreen.DOFade(0, 1);
    }

    public void UpdateProgressText(string progress)
    {
        _processText.text = progress;
    }
}
