using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public List<string> Lines;

    public void Play()
    {
        Instantiate(Resources.Load<GameObject>("DialogueUI")).GetComponent<DialogueUI>().Play(this);
    }
}
