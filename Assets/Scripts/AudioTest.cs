using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public RadioKnob Knob;
    public EventReference StaticFmodEvent;
    public List<RadioFrequencyAudio> Frequencies;
    public EventReference NumberVAEvent;

    private EventInstance _staticInstance;
    private EventInstance _numberVAEventInstance;
    private List<EventInstance> _instances;


    void Start()
    {
        foreach (var frequency in Frequencies)
        {
            frequency.Instance = RuntimeManager.CreateInstance(frequency.FmodEvend);
            frequency.Instance.start();
            frequency.Instance.setVolume(0);

            frequency.NumberVAInstance = RuntimeManager.CreateInstance(NumberVAEvent); ;
            frequency.Initialize();
        }
        _staticInstance = RuntimeManager.CreateInstance(StaticFmodEvent);
        _staticInstance.start();

    }

    private void Update()
    {
        var frequencyNum = Knob.CurrentFrequency;
        var otherFreqSum = 0f;
        foreach (var frequency in Frequencies)
        {
            var distance = Mathf.Abs(frequencyNum - frequency.Frequency);
            distance = Mathf.Min(frequency.FrequencyWidth, distance);
            distance = math.remap(frequency.FrequencyWidth, 0, 0, 1, distance);
            //frequency.Instance.setVolume(distance);
            frequency.VolumeReadout = distance;
            otherFreqSum += distance;
        }

        otherFreqSum = Mathf.Min(0.7f, otherFreqSum);
        _staticInstance.setVolume(1f - otherFreqSum);
    }
}

[System.Serializable]
public class RadioFrequencyAudio
{
    public EventReference FmodEvend;
    [HideInInspector]
    public EventInstance Instance;
    public float Frequency = 131.3f;
    public float FrequencyWidth = 1f;
    public float VolumeReadout = 0f;
    public List<int> Numbers;
    public float DelayBetween = 1.5f;

    public EventInstance NumberVAInstance;
    private int index = 0;

    public void Initialize()
    {
        Lerp.Delay(DelayBetween + UnityEngine.Random.Range(-0.5f, 0.5f), () =>
        {
            PlayNumber();
        });
    }

    private void PlayNumber()
    {
        NumberVAInstance.setParameterByName("VANumber", Numbers[index]);
        NumberVAInstance.start();
        NumberVAInstance.setVolume(VolumeReadout);

        index++;
        if (index > Numbers.Count - 1)
            index = 0;

        Lerp.Delay(DelayBetween + UnityEngine.Random.Range(-0.5f, 0.5f), () =>
        {
            PlayNumber();
        });
    }
}