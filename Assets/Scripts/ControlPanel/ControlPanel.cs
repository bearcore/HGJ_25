using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ControlPanel : MonoBehaviour
{
    public UnityEvent OnValid;
    public UnityEvent OnInvalid;
    public bool IsValid;
    public List<GameObject> ActivateOnValid;
    public List<GameObject> DeActivateOnValid;

    private KeyPad keypad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keypad = gameObject.GetComponentInChildren<KeyPad>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetSequence(List<string> targetList)
    {
        keypad.SetTargetSequence(targetList);
    }

    public void SetValid(bool valid)
    {
        IsValid = valid;
        if (IsValid)
        {
            foreach (var activate in ActivateOnValid)
            {
                activate.SetActive(true);
            }
            foreach (var activate in DeActivateOnValid)
            {
                activate.SetActive(false);
            }
            OnValid.Invoke();
        }
        else
            OnInvalid.Invoke();
    }
}
