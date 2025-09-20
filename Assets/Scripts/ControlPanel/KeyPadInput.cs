using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class KeyPad : MonoBehaviour
{
    private List<string> targetList;
    private List<string> inputList;
    private bool changed = false;

    private ControlPanel cp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cp = gameObject.GetComponentInParent<ControlPanel>();
        targetList = new List<string>() {"1","D","3","s" };
        inputList = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if(changed)
        {
            Debug.Log("InputList: " + string.Join(", ", inputList));
            changed = false;
        }
    }
    
    private void ValidateSequence()
    {
        if (inputList == null || targetList == null)
        {
            Debug.LogWarning("ValidateSequenze: inputList oder targetList ist null.");
            cp.OnInvalid.Invoke();
            ClearSequence();
            return;
        }

        if (inputList.Count != targetList.Count)
        {
            Debug.LogWarning($"ValidateSequenze: Unterschiedliche Länge (input={inputList.Count}, target={targetList.Count}).");
            cp.OnInvalid.Invoke();
            ClearSequence();
            return;
        }

        var cmp = StringComparison.OrdinalIgnoreCase;

        for (int i = 0; i < inputList.Count; i++)
        {
            string src = inputList[i] ?? string.Empty; // z.B. "2abc"
            string needle = targetList[i] ?? string.Empty; // z.B. "2"


            if (src.IndexOf(needle, cmp) < 0) // Falls das geforderte Zeichen nicht gedrückt wurde
            {
                Debug.Log($"ValidateSequenze: Fehler bei Index {i}: \"{src}\" enthält nicht \"{needle}\".");
                cp.OnInvalid.Invoke();
                ClearSequence();
                return;
            }
        }
        // an dieser stelle kommt der code nur an, wenn beide listen gleich lang sind und alle prüfungen erfolgreich waren. => Korrekt
        cp.OnValid.Invoke();
        ClearSequence();
    }

    private void ClearSequence()
    {
        inputList.Clear();
        changed = true;
    }

    public void OnKeyInput(string keyInput)
    {
        if (keyInput.Equals("Key_Confirm"))
        {
            ValidateSequence();

        } else if (keyInput.Equals("Key_Reset"))
        {
            ClearSequence();
            changed = true;
        } else
        {
            inputList.Add(keyInput);
            changed = true;

        }
    }
    public void SetTargetSequence(List<string> targetList)
    {
        this.targetList = targetList;
    }
}
