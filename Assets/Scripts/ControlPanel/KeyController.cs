using UnityEngine;

public class KeyController : MonoBehaviour
{
    public Interactable Interactable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interactable.OnUsed.AddListener(OnUsed);
    }

    private void OnUsed()
    {
        Debug.Log(gameObject.name + " OnUsed in KeyController");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
