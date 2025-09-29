using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnableAntenna : MonoBehaviour
{
    public float RotationPerPress = 22.5f;
    public Interactable Interactable;
    public AudioTest AudioTest;
    public List<AntennaTarget> Targets;
    public List<GameObject> Indicators;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interactable.OnUsed.AddListener(OnUsed);
    }

    private void OnUsed()
    {
        var anyMatch = false;
        transform.eulerAngles = transform.eulerAngles + new Vector3(0f, RotationPerPress);
        AntennaTarget activeTarget = null;
        foreach (var target in Targets)
        {
            var match = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, target.TargetAngle)) < 25f;
            AudioTest.Frequencies[target.UnlockIndex].IsActive = match;

            if (match)
            {
                activeTarget = target;
                anyMatch = true;
            }

            target.ConsoleIndicators.ForEach(x => x.SetActive(match));
            target.InactiveIndicators.ForEach(x => x.SetActive(!match));
        }

        foreach (var indicator in Indicators)
        {
            indicator.SetActive(anyMatch);
            if(activeTarget != null)
            {
                indicator.GetComponent<MeshRenderer>().material = activeTarget.Lightcolor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class AntennaTarget
{
    public float TargetAngle;
    public int UnlockIndex;
    public Material Lightcolor;
    public List<GameObject> ConsoleIndicators;
    public List<GameObject> InactiveIndicators;
}