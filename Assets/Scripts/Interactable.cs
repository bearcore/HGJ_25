using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string UseText;
    public UnityEvent OnUsed;

    private void Awake()
    {
        OnUsed.AddListener(() =>
        {
            Debug.Log(gameObject.name + " was used");
        });
    }
}
