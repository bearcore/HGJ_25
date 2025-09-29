using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;
using System;

public class AudioTest : MonoBehaviour
{
    public RadioKnob Knob;
    public EventReference StaticFmodEvent;
    public List<RadioFrequencyAudio> Frequencies;
    public EventReference NumberVAEvent;
    public EventReference LettersVAEvent;
    public GameObject SoundLocation;

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
            frequency.Instance.set3DAttributes(RuntimeUtils.To3DAttributes(SoundLocation));

            frequency.NumberVAInstance = RuntimeManager.CreateInstance(NumberVAEvent);
            frequency.NumberVAInstance.set3DAttributes(RuntimeUtils.To3DAttributes(SoundLocation));
            frequency.LettersVAInstance = RuntimeManager.CreateInstance(LettersVAEvent);
            frequency.LettersVAInstance.set3DAttributes(RuntimeUtils.To3DAttributes(SoundLocation));
            frequency.Initialize();
        }
        _staticInstance = RuntimeManager.CreateInstance(StaticFmodEvent);
        _staticInstance.set3DAttributes(RuntimeUtils.To3DAttributes(SoundLocation));
        _staticInstance.start();

    }

    public void AddFrequency(RadioFrequencyAudio frequency)
    {
        frequency.NumberVAInstance = RuntimeManager.CreateInstance(NumberVAEvent);
        frequency.NumberVAInstance.set3DAttributes(RuntimeUtils.To3DAttributes(SoundLocation));
        frequency.LettersVAInstance = RuntimeManager.CreateInstance(LettersVAEvent);
        frequency.LettersVAInstance.set3DAttributes(RuntimeUtils.To3DAttributes(SoundLocation));
        frequency.Initialize();

        Frequencies.Add(frequency);
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
            if(frequency.IsActive)
                otherFreqSum += distance;
        }

        otherFreqSum = Mathf.Min(1f, otherFreqSum);
        _staticInstance.setParameterByName("Radio Whitenoise", otherFreqSum);
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
    public List<char> Numbers;
    public float DelayBetween = 1f;
    public bool IsActive = false;

    public EventInstance NumberVAInstance;
    public EventInstance LettersVAInstance;
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
        if (IsActive)
        {
            if (Char.IsDigit(Numbers[index]))
            {
                NumberVAInstance.setParameterByName("VANumber", (int)char.GetNumericValue(Numbers[index]));
                NumberVAInstance.start();
                NumberVAInstance.setVolume(VolumeReadout);
            }
            else
            {
                LettersVAInstance.setParameterByName("VANumber", char.ToLower(Numbers[index]) - 'a' + 1);
                LettersVAInstance.start();
                LettersVAInstance.setVolume(VolumeReadout);
            }
        }

        index++;
        if (index > Numbers.Count - 1)
        {
            index = 0;
            Lerp.Delay(DelayBetween, () =>
            {
                LettersVAInstance.setParameterByName("VANumber", 0);
                LettersVAInstance.start();
                LettersVAInstance.setVolume(VolumeReadout);
                Lerp.Delay(DelayBetween + 2.5f, () =>
                {
                    PlayNumber();
                });
            });
        }
        else
        {
            Lerp.Delay(DelayBetween, () =>
            {
                PlayNumber();
            });
        }
    }
}