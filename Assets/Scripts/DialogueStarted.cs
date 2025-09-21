using UnityEngine;

public class DialogueStarted : MonoBehaviour
{
    public bool StartOnEnable = false;
    public Dialogue DialogueToPlay;

    public void OnEnable()
    {
        if (StartOnEnable)
            DialogueToPlay.Play();
    }

    public void PlayDialogue()
    {
        DialogueToPlay.Play();
    }
}