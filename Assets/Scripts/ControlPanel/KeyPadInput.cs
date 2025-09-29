using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using TMPro;
using System.Linq;

public class KeyPad : MonoBehaviour
{
    public TextMeshProUGUI OutputText;

    public List<string> targetList;
    private List<string> inputList;
    private bool changed = false;

    private ControlPanel cp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cp = gameObject.GetComponentInParent<ControlPanel>();
        //targetList = new List<string>() {"1","D","3","s" };
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
        Debug.Log("TargetList: " + string.Join(", ", targetList));
        if (inputList == null || targetList == null)
        {
            Debug.LogWarning("ValidateSequenze: inputList oder targetList ist null.");
            cp.OnInvalid.Invoke();
            //ClearSequence();
            return;
        }

        if (inputList.Count != targetList.Count)
        {
            Debug.LogWarning($"ValidateSequenze: Unterschiedliche L?nge (input={inputList.Count}, target={targetList.Count}).");
            cp.OnInvalid.Invoke();
            //ClearSequence();
            return;
        }

        var cmp = StringComparison.OrdinalIgnoreCase;

        for (int i = 0; i < inputList.Count; i++)
        {
            string src = inputList[i] ?? string.Empty; // z.B. "2abc"
            string needle = targetList[i] ?? string.Empty; // z.B. "2"


            if (src.IndexOf(needle, cmp) < 0) // Falls das geforderte Zeichen nicht gedr?ckt wurde
            {
                Debug.Log($"ValidateSequenze: Fehler bei Index {i}: \"{src}\" enth?lt nicht \"{needle}\".");
                cp.SetValid(false);
                //ClearSequence();
                return;
            }
        }
        // an dieser stelle kommt der code nur an, wenn beide listen gleich lang sind und alle pr?fungen erfolgreich waren. => Korrekt
        cp.SetValid(true);
        //ClearSequence();
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
            ValidateSequence();
            changed = true;
        }

        if (inputList.Count > 4)
        {
            ClearSequence();
        }

        OutputText.text = string.Join("", inputList.Select(x => x.First()));
        if (cp.IsValid)
        {
            OutputText.color = Color.green;
        }
    }
    public void SetTargetSequence(List<string> targetList)
    {
        this.targetList = targetList;
    }
}
