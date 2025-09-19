using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float InteractionDistance = 1f;
    public Interactable LastHitInteractable;
    public InteractionHintUI interactionHintUI;

    private void Start()
    {
        interactionHintUI = Instantiate(Resources.Load<GameObject>("InteractableHintUI")).GetComponent<InteractionHintUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var lastHitInteractable = null as Interactable;
        var hits = Physics.RaycastAll(transform.position, transform.forward, InteractionDistance);
        foreach (var hit in hits)
        {
            lastHitInteractable = hit.collider.GetComponentInChildren<Interactable>();
        }

        if(lastHitInteractable == null)
        {
            if(LastHitInteractable != null)
                interactionHintUI.Hide();
            LastHitInteractable = null;
        }

        if(LastHitInteractable != lastHitInteractable)
        {
            LastHitInteractable = lastHitInteractable;
            interactionHintUI.Show(lastHitInteractable.UseText);
        }

        if(LastHitInteractable != null)
        {
            if (Input.GetKeyDown("E"))
            {
                LastHitInteractable.OnUsed.Invoke();
            }
        }
    }
}
