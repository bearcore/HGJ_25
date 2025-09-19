using UnityEngine;

public class RadioController : MonoBehaviour
{
    private RadioColumn[] columns;
    private float[] values;

    [SerializeField] private float fallOffFactor = 0.02f;
        
    [SerializeField]
    private float center = 2;
    
    [SerializeField]
    private float jitterStrength = 0.1f;

    [SerializeField] private bool generateMode = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        columns = this.GetComponentsInChildren<RadioColumn>();
        values = new float[columns.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (generateMode)
        {
            var bell_center = center;
                    
            for (int i = 0; i < values.Length; i++)
            {
                var randomJitter = (Random.value - 0.5f) * jitterStrength;
                var distance = Mathf.Abs(bell_center - i);
                values[i] = -distance*distance*fallOffFactor + 1 + randomJitter;
                columns[i].SetValue(values[i]);
            }
        }
    }

    /// <summary>
    /// Set values between 0.0 and 1.0, array needs to have length of 21 
    /// </summary>
    /// <param name="values"></param>
    public void SetValues(float[] values)
    {
        this.values = values;
    }
}
