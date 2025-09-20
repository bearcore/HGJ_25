using System;
using UnityEngine;
using System.Runtime.InteropServices;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class ReadFmodFrequency : MonoBehaviour
{
    [Header("Attach to a bus (e.g. bus:/SFX) or leave as bus:/ for master")]
    public string busPath = "bus:/";
    [Range(256, 8192)] public int windowSize = 2048;

    Bus bus;
    ChannelGroup cg;
    FMOD.DSP fft;
    bool attached;

    float[] lastBins; // reuse buffer to avoid GC
    public float[] Data;

    void Awake()
    {
        // Get the bus handle now (cheap), but its ChannelGroup may not exist yet
        RuntimeManager.StudioSystem.getBus(busPath, out bus);
    }

    void Update()
    {
        // 1) If not attached yet, try to get a live ChannelGroup and insert FFT
        if (!attached)
        {
            if (bus.hasHandle() && bus.getChannelGroup(out cg) == RESULT.OK && cg.hasHandle())
            {
                // ChannelGroup exists only when that bus has at least one playing voice
                RESULT r = RuntimeManager.CoreSystem.createDSPByType(DSP_TYPE.FFT, out fft);
                LogIfFail("createDSPByType", r);

                // Configure FFT
                fft.setParameterInt((int)DSP_FFT.WINDOWSIZE, windowSize);
                // Optional: fft.setParameterInt((int)DSP_FFT.WINDOWTYPE, (int)DSP_FFT_WINDOW.HANNING);

                r = cg.addDSP(0, fft);
                LogIfFail("addDSP", r);

                attached = (r == RESULT.OK);
            }
            // Not ready yet; just wait for audio to start on that bus
        }
        
    }

    public bool TryGetAveragedSpectrum(out float[] binsOut)
    {
        binsOut = null;

        if (!attached || !fft.hasHandle())
            return false; // not ready (no audio yet or DSP not inserted)

        // 2) Pull parameter data
        RESULT r = fft.getParameterData((int)DSP_FFT.SPECTRUMDATA, out System.IntPtr dataPtr, out _);
        if (r != RESULT.OK || dataPtr == System.IntPtr.Zero)
            return false;

        var data = (FMOD.DSP_PARAMETER_FFT)System.Runtime.InteropServices.Marshal.PtrToStructure(
            dataPtr, typeof(FMOD.DSP_PARAMETER_FFT));

        int bins = data.length;
        int chs = data.numchannels;

        if (bins <= 0 || chs <= 0 || data.spectrum == null)
            return false; // FMOD hasn’t filled a frame yet

        EnsureBuffer(ref lastBins, bins);
        System.Array.Clear(lastBins, 0, bins);

        for (int c = 0; c < chs; c++)
        {
            var chBins = data.spectrum[c];
            if (chBins == null) continue;
            for (int i = 0; i < bins; i++) lastBins[i] += chBins[i];
        }

        if (chs > 1)
        {
            float inv = 1f / chs;
            for (int i = 0; i < bins; i++) lastBins[i] *= inv;
        }

        binsOut = lastBins;
        return true;
    }

    static void EnsureBuffer(ref float[] arr, int len)
    {
        if (arr == null || arr.Length != len) arr = new float[len];
    }

    static void LogIfFail(string op, RESULT r)
    {
        if (r != RESULT.OK) UnityEngine.Debug.LogWarning($"FMOD {op} failed: {r}");
    }

    void OnDestroy()
    {
        if (cg.hasHandle() && fft.hasHandle())
        {
            cg.removeDSP(fft);
            fft.release();
        }
    }
}
