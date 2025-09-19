using TMPro;
using UnityEngine;

public class InteractionHintUI : MonoBehaviour
{
    public CanvasGroup Fade;
    public TextMeshProUGUI Text;

    private Coroutine _fadeRoutine;

    public void Show(string text)
    {
        Text.text = text;
        if (_fadeRoutine != null)
            CoroutineHelper.StopGlobalCoroutine(_fadeRoutine);
        _fadeRoutine = Lerp.FromTo(0.2f, t =>
        {
            Fade.alpha = t;
        }, Fade.alpha, 1);
    }

    public void Hide()
    {
        if (_fadeRoutine != null)
            CoroutineHelper.StopGlobalCoroutine(_fadeRoutine);
        _fadeRoutine = Lerp.FromTo(0.2f, t =>
        {
            Fade.alpha = t;
        }, Fade.alpha, 0);
    }
}