using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    [SerializeField] private EventReference sfx; // assign via inspector

    EventInstance instance;

    void Start()
    {
        instance = RuntimeManager.CreateInstance(sfx);
        instance.start();
    }

    void OnDestroy()
    {
        instance.release(); // always release your instances
    }
}
