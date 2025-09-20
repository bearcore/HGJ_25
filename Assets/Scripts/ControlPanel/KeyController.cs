using UnityEngine;

public class KeyController : MonoBehaviour
{
    public Interactable Interactable;
    private KeyPad kp;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interactable.OnUsed.AddListener(OnUsed);
        kp = gameObject.GetComponentInParent<KeyPad>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnUsed()
    {
        kp.OnKeyInput(gameObject.name);
    }

    private void OnHoverStated()
    {
        //use for Highlight shader
    }

    private void OnHoverEnded()
    {

    }
}