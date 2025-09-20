using UnityEngine;
using UnityEngine.UI;

public class SpectrumBarsUI : MaskableGraphic
{
    [Header("Bar layout")]
    [Min(4)] public int barCount = 128;
    [Range(0f, 0.9f)] public float gap = 0.2f; // fraction of bar width used as spacing
    public float topPadding = 2f;
    public float bottomPadding = 2f;
    public ReadFmodFrequency ReadFmodFrequency;

    [Header("Signal processing")]
    [Tooltip("Average this many FFT bins per bar (linear) if logCurve=0.\nWhen logCurve>0, this is used as a small smoothing window around the sampled bin.")]
    [Min(1)] public int binsPerBar = 8;
    [Tooltip("0 = linear frequency. Higher values = more bars focused on low freqs.")]
    [Range(-4f, 4f)] public float logCurve = 2.0f;
    [Tooltip("Exponential smoothing for bar heights (0 = snappy, 1 = frozen).")]
    [Range(0f, 0.99f)] public float smoothing = 0.5f;


    [Header("Mapping")]
    public bool useLinear = false;     // If true, bypass dB mapping
    public float linearGain = 50f;     // Scales raw magnitudes
    public bool debugFill = false;     // Forces visible bars to test UI

    [Header("Amplitude mapping (dB)")]
    [Tooltip("Minimum dB (floor). Typical: -80 to -60.")]
    public float minDb = -80f;
    [Tooltip("Gain to make things pop. Typical: +20 to +40 dB.")]
    public float gainDb = 30f;

    float[] _fft;           // latest input
    float[] _heights;       // smoothed linear 0..1 per bar
    float[] _work;          // temp
    bool _dirtyData;

    protected override void Awake()
    {
        base.Awake();
        if (_heights == null || _heights.Length != barCount) _heights = new float[barCount];
    }

    public void SetSpectrum(float[] fftMagnitudes)
    {
        if (fftMagnitudes == null || fftMagnitudes.Length == 0) return;
        _fft = fftMagnitudes;
        _dirtyData = true;
        SetVerticesDirty(); // <- THIS makes OnPopulateMesh run next UI rebuild
    }

    void Update()
    {
        var success = ReadFmodFrequency.TryGetAveragedSpectrum(out var spectrum);
        SetSpectrum(spectrum);

        if (_dirtyData && _fft != null)
        {
            UpdateBars(_fft);
            _dirtyData = false;
            SetVerticesDirty(); // triggers OnPopulateMesh
        }
    }

    void Ensure(int n)
    {
        if (_heights == null || _heights.Length != n) _heights = new float[n];
        if (_work == null || _work.Length != n) _work = new float[n];
    }

    void UpdateBars(float[] fft)
    {
        Ensure(barCount);
        int bins = fft.Length;

        // Build per-bar value by sampling/averaging FFT bins.
        for (int i = 0; i < barCount; i++)
        {
            // Frequency mapping: linear -> log-ish.
            // t in [0..1], warp by logCurve, then map to bin index.
            float t = (barCount <= 1) ? 0f : (float)i / (barCount - 1);
            if (logCurve > 0f) t = Mathf.Pow(t, 1f / (1f + logCurve)); // denser at low end

            float fIdx = t * (bins - 1);
            int center = Mathf.Clamp(Mathf.RoundToInt(fIdx), 0, bins - 1);

            // Average a small window around center
            int half = Mathf.Max(1, binsPerBar / 2);
            int start = Mathf.Clamp(center - half, 0, bins - 1);
            int end = Mathf.Clamp(center + half, 0, bins - 1);

            double sum = 0;
            int count = 0;
            for (int k = start; k <= end; k++) { sum += fft[k]; count++; }
            float mag = (count > 0) ? (float)(sum / count) : 0f;

            float norm;
            if (useLinear)
            {
                norm = Mathf.Clamp01(mag * linearGain);
            }
            else
            {
                float db = 20f * Mathf.Log10(Mathf.Max(1e-7f, mag)) + gainDb;
                norm = Mathf.Clamp01(Mathf.InverseLerp(minDb, 0f, db));
            }

            // DEBUG: force visible bars to confirm UI draws
            if (debugFill) norm = 0.6f;

            // Smooth (old -> new). This is the important fix:
            _heights[i] = Mathf.Lerp(_heights[i], norm, 1f - smoothing);
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        Rect r = GetPixelAdjustedRect();
        float width = r.width - 0.0001f;
        float height = Mathf.Max(0f, r.height - topPadding - bottomPadding);

        if (_heights == null || _heights.Length != barCount || barCount < 1 || height <= 0f)
            return;

        float barFull = width / barCount;
        float barGap = barFull * gap;
        float barW = Mathf.Max(1f, barFull - barGap);

        // Two triangles (4 verts) per bar
        for (int i = 0; i < barCount; i++)
        {
            float x0 = r.xMin + i * barFull + barGap * 0.5f;
            float x1 = x0 + barW;
            float y0 = r.yMin + bottomPadding;
            float y1 = y0 + height * _heights[i];

            int idx = i * 4;
            UIVertex v = UIVertex.simpleVert; v.color = color;

            v.position = new Vector2(x0, y0); vh.AddVert(v);
            v.position = new Vector2(x0, y1); vh.AddVert(v);
            v.position = new Vector2(x1, y1); vh.AddVert(v);
            v.position = new Vector2(x1, y0); vh.AddVert(v);

            vh.AddTriangle(idx + 0, idx + 1, idx + 2);
            vh.AddTriangle(idx + 2, idx + 3, idx + 0);
        }
    }
}
