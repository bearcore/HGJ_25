using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraShakeController : MonoBehaviour
{
    public SnowSound SnowSound;
    public CinemachineVirtualCamera ShakeCamera;
    public Volume PostProcess;

    private bool _isInside = true;
    private Coroutine _fadeRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var noise = ShakeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (SnowSound.InsideCount > 0 && !_isInside)
        {
            if (_fadeRoutine != null)
                CoroutineHelper.StopGlobalCoroutine(_fadeRoutine);
            _fadeRoutine = Lerp.FromTo(0.5f, t =>
            {
                noise.m_AmplitudeGain = Mathf.Lerp(3f, 0.5f, t);
                noise.m_FrequencyGain = Mathf.Lerp(1.5f, 0.3f, t);
                PostProcess.profile.TryGet<Vignette>(out var vignette);
                vignette.intensity.value = Mathf.Lerp(0.417f, 0.1f, t);
            });

            _isInside = true;
        }
        if(SnowSound.InsideCount == 0 && _isInside)
        {
            if (_fadeRoutine != null)
                CoroutineHelper.StopGlobalCoroutine(_fadeRoutine);
            _fadeRoutine = Lerp.FromTo(0.5f, t =>
            {
                noise.m_AmplitudeGain = Mathf.Lerp(0.5f, 3f, t);
                noise.m_FrequencyGain = Mathf.Lerp(0.3f, 1.5f, t);
                PostProcess.profile.TryGet<Vignette>(out var vignette);
                vignette.intensity.value = Mathf.Lerp(0.1f, 0.417f, t);
            });

            _isInside = false;
        }

        _isInside = SnowSound.InsideCount > 0;
    }

}
