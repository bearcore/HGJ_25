using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string UseText;
    public UnityEvent OnUsed;
    public UnityEvent OnHoverStated;
    public UnityEvent OnHoverEnded;

    private void Awake()
    {
        OnUsed.AddListener(() =>
        {
            Debug.Log(gameObject.name + " was used");
        });

        OnHoverStated.AddListener(() =>
        {
            Debug.Log(gameObject.name + " hover started");
        });

        OnHoverEnded.AddListener(() =>
        {
            Debug.Log(gameObject.name + " hover ended");
        });
    }
}
