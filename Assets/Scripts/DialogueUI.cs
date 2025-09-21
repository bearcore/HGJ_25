using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public Dialogue _dialogue;
    public TextMeshProUGUI Text;
    public CanvasGroup Fade;

    private string _currentLine;

    public void Play(Dialogue dialogue)
    {
        _dialogue = dialogue;
        ShowLine(0);
    }

    private void ShowLine(int lineIndex)
    {
        if (lineIndex >= _dialogue.Lines.Count)
        {
            Lerp.FromTo(3f, t =>
            {
                Fade.alpha = t;
            }, 1f, 0f);
            return;
        }

        _currentLine = _dialogue.Lines[lineIndex];
        Lerp.To(5f, t =>
        {
            Text.text = new string(_currentLine.ToCharArray().Take((int)(t * _currentLine.Length)).ToArray());
        }, onDone: () =>
        {
            Lerp.Delay(2f, () =>
            {
                ShowLine(lineIndex + 1);
            });
        });
    }
}
