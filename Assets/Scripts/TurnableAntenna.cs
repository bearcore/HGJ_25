using UnityEngine;
using UnityEngine.Events;

public class TurnableAntenna : MonoBehaviour
{
    public float CorrectAngle = 275f;
    public float RotationPerPress = 45f;
    public Interactable Interactable;
    public bool IsInCorrectRotation = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interactable.OnUsed.AddListener(OnUsed);
    }

    private void OnUsed()
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(0f, RotationPerPress);
        IsInCorrectRotation = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, CorrectAngle)) < 25f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
