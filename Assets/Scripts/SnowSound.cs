using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SnowSound : MonoBehaviour
{
    public EventReference SnowSoundFmod;
    private EventInstance _snowsoundInstance;
    public List<Collider> InsideColliders;
    public int InsideCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _snowsoundInstance = RuntimeManager.CreateInstance(SnowSoundFmod);
        _snowsoundInstance.start();
    }

    // Update is called once per frame
    void Update()
    {
        if(InsideCount > 0)
        {
            _snowsoundInstance.setParameterByName("Blizzard Type", 1);
        }
        else
        {
            _snowsoundInstance.setParameterByName("Blizzard Type", 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (InsideColliders.Contains(other))
        {
            InsideCount++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (InsideColliders.Contains(other))
        {
            InsideCount--;
        }
    }
}
