using Cinemachine;
using FMOD.Studio;
using FMODUnity;
using StarterAssets;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioKnob : MonoBehaviour
{
    public float CurrentRotation;
    public bool IsInteracting;
    public Interactable Interactable;
    public float RotationStrength = 10f;
    public FirstPersonController Controller;
    public CinemachineVirtualCamera VirtualCamera;
    public TextMeshProUGUI FrequencyUI;
    public float CurrentFrequency = 133.0f;
    public float MinFrequency = 50;
    public float MaxFrequency = 250;
    public float ClickSoundRequirement = 0.4f;
    private float _clickSoundSum = 0f;

    public EventReference ClickEvent;
    private EventInstance _clickInstance;

    private InputAction _lookAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interactable.OnUsed.AddListener(ToggleInteraction);
        _lookAction = InputSystem.actions.FindAction("Look");
        _clickInstance = RuntimeManager.CreateInstance(ClickEvent);
        _clickInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        var look = _lookAction.ReadValue<Vector2>();
        if(IsInteracting)
            transform.localEulerAngles = transform.localEulerAngles + new Vector3(0f, 0f, -look.y * RotationStrength * 3f);
        CurrentRotation = transform.eulerAngles.z;

        if (IsInteracting)
            CurrentFrequency += -look.y * RotationStrength;
        CurrentFrequency = Mathf.Clamp(CurrentFrequency, MinFrequency, MaxFrequency);

        if (IsInteracting)
            _clickSoundSum += Mathf.Abs(look.y) * RotationStrength;
        if(_clickSoundSum > ClickSoundRequirement)
        {
            _clickSoundSum -= ClickSoundRequirement;
            _clickInstance.start();
        }

        var rounded = Mathf.Round(CurrentFrequency * 10f) / 10f;
        FrequencyUI.text = rounded.ToString().Contains(".") ? rounded + "00" : rounded + ".000";
    }

    public void ToggleInteraction()
    {
        if (IsInteracting)
            StopInteracting();
        else
            StartInteracting();
    }

    public void StartInteracting()
    {
        IsInteracting = true;
        Controller.enabled = false;
        VirtualCamera.Priority = 10;

    }

    public void StopInteracting()
    {
        IsInteracting = false;
        Controller.enabled = true;
        VirtualCamera.Priority = -1;
    }
}
