using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ControlPanel : MonoBehaviour
{
    public UnityEvent OnValid;
    public UnityEvent OnInvalid;

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

    private void Awake()
    {
        OnValid.AddListener(() =>
        {
            Debug.Log("Code was Valid");
        });

        OnInvalid.AddListener(() =>
        {
            Debug.Log("Code was not Valid");
        });

    }
}
