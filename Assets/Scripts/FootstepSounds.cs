using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    public EventReference FootstepEvent;
    private EventInstance _footstepInstance;

    public SnowSound SnowSound;

    public float FootstepDistance = 1f;
    public float CrossedDistance = 0f;
    private Vector3 _lastFramePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _footstepInstance = RuntimeManager.CreateInstance(FootstepEvent);
        _lastFramePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CrossedDistance += Vector3.Distance(_lastFramePosition, transform.position);
        if(CrossedDistance > FootstepDistance)
        {
            CrossedDistance -= FootstepDistance;
            PlayFootstepSound();
        }


        _lastFramePosition = transform.position;
    }

    private void PlayFootstepSound()
    {
        if(SnowSound.InsideCount > 0)
        {
            _footstepInstance.setParameterByName("Floor Type", 0);
        }
        else
        {
            _footstepInstance.setParameterByName("Floor Type", 1);
        }
        _footstepInstance.start();
        Debug.Log("Footstep " + (SnowSound.InsideCount > 0 ? "Inside" : "Outside"));
    }
}
