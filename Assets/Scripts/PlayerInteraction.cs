using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float InteractionDistance = 1f;
    public Interactable LastHitInteractable;
    public InteractionHintUI interactionHintUI;

    private InputAction _useAction;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        interactionHintUI = Instantiate(Resources.Load<GameObject>("InteractableHintUI")).GetComponent<InteractionHintUI>();
        _useAction = InputSystem.actions.FindAction("Interact");
        _useAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        var lastHitInteractable = null as Interactable;
        var hits = Physics.RaycastAll(_mainCamera.transform.position, _mainCamera.transform.forward, InteractionDistance);
        foreach (var hit in hits)
        {
            lastHitInteractable = hit.collider.GetComponentInChildren<Interactable>();
        }

        if(lastHitInteractable == null)
        {
            if(LastHitInteractable != null)
            {
                LastHitInteractable.OnHoverEnded.Invoke();
                interactionHintUI.Hide();
            }
            LastHitInteractable = null;
        }

        if(LastHitInteractable != lastHitInteractable)
        {
            LastHitInteractable = lastHitInteractable;
            interactionHintUI.Show(lastHitInteractable.UseText);
            LastHitInteractable.OnHoverStated.Invoke();
        }

        if(LastHitInteractable != null)
        {
            if (_useAction.WasPerformedThisFrame())
            {
                LastHitInteractable.OnUsed.Invoke();
            }
        }
    }
}
