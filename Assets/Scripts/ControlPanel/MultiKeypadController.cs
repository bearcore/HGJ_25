using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiKeypadController : MonoBehaviour
{
    public List<ControlPanel> Panels;
    public GameObject Outro;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var panel in Panels)
        {
            panel.OnValid.AddListener(OnPanelValid);
        }
    }

    void OnPanelValid()
    {
        var allValid = Panels.All(x => x.IsValid);
        if (allValid)
        {
            Outro.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
