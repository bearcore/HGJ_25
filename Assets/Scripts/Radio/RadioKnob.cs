using Cinemachine;
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

    private InputAction _lookAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interactable.OnUsed.AddListener(StartInteracting);
        _lookAction = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsInteracting) return;

        var look = _lookAction.ReadValue<Vector2>();
        Debug.Log(look.y);
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0f, 0f, -look.y * RotationStrength);
        CurrentRotation = transform.eulerAngles.z;

        CurrentFrequency += -look.y * RotationStrength;
        CurrentFrequency = Mathf.Clamp(CurrentFrequency, MinFrequency, MaxFrequency);

        var rounded = (Mathf.Round(math.remap(0, 360, 50, 250, CurrentFrequency) * 10f) / 10f);
        FrequencyUI.text = rounded.ToString().Contains(".") ? rounded + "00" : rounded + ".000";
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
