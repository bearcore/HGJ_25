using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Lerp
{
    public static Coroutine To(float duration, UnityAction<float> t,
        float target = 1f, bool smooth = false, UnityAction onDone = null, bool useRealtime = false)
    {
        return CoroutineHelper.StartGlobalCoroutine(ToEnumerate(duration, t, target, smooth, onDone, useRealtime));
    }

    public static Coroutine To(float duration, UnityAction<float> t, ref Coroutine coroutine,
        float target = 1f, bool smooth = false, UnityAction onDone = null, bool useRealtime = false)
    {
        CoroutineHelper.StopGlobalCoroutine(coroutine);
        coroutine = CoroutineHelper.StartGlobalCoroutine(ToEnumerate(duration, t, target, smooth, onDone, useRealtime));
        return coroutine;
    }

    public static IEnumerator ToEnumerate(float duration, UnityAction<float> t,
        float target = 1f, bool smooth = false, UnityAction onDone = null, bool useRealtime = false)
    {
        yield return FromToEnumerate(duration, t, 0f, target, smooth, onDone, useRealtime);
    }

    public static Coroutine FromTo(float duration, UnityAction<float> t, ref Coroutine coroutine,
        float start = 0f, float target = 1f, bool smooth = false, UnityAction onDone = null, bool useRealtime = false)
    {
        CoroutineHelper.StopGlobalCoroutine(coroutine);
        coroutine = CoroutineHelper.StartGlobalCoroutine(FromToEnumerate(duration, t, start, target, smooth, onDone, useRealtime));
        return coroutine;
    }

    public static Coroutine FromTo(float duration, UnityAction<float> t,
        float start = 0f, float target = 1f, bool smooth = false, UnityAction onDone = null, bool useRealtime = false)
    {
        return CoroutineHelper.StartGlobalCoroutine(FromToEnumerate(duration, t, start, target, smooth, onDone, useRealtime));
    }

    public static IEnumerator FromToEnumerate(float duration, UnityAction<float> t,
        float start = 0f, float target = 1f, bool smooth = false, UnityAction onDone = null, bool useRealtime = false)
    {
        var elapsedTime = 0f;
        t(start);
        while (elapsedTime < duration)
        {
            // Yield here
            yield return null;

            elapsedTime += useRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (smooth)
                t(Mathf.SmoothStep(start, target, elapsedTime / duration));
            else
                t(Mathf.Lerp(start, target, elapsedTime / duration));
        }
        t(target);
        if (onDone != null) onDone.Invoke();
    }

    public static Coroutine Delay(float duration, UnityAction onDone, ref Coroutine coroutine, bool useRealTime = false)
    {
        CoroutineHelper.StopGlobalCoroutine(coroutine);
        coroutine = CoroutineHelper.StartGlobalCoroutine(DelayEnumerate(duration, onDone, useRealTime));
        return coroutine;
    }

    public static Coroutine Delay(float duration, UnityAction onDone, bool useRealTime = false)
    {
        return CoroutineHelper.StartGlobalCoroutine(DelayEnumerate(duration, onDone, useRealTime));
    }

    public static IEnumerator DelayEnumerate(float duration, UnityAction onDone, bool useRealTime = false)
    {
        if (useRealTime)
            yield return new WaitForSecondsRealtime(duration);
        else
            yield return new WaitForSeconds(duration);
        onDone();
    }

    public static Coroutine WaitForAfterLateUpdate(UnityAction onDone)
    {
        return CoroutineHelper.StartGlobalCoroutine(WaitForAfterLateUpdateEnumerate(onDone));
    }

    private static IEnumerator WaitForAfterLateUpdateEnumerate(UnityAction onDone)
    {
        yield return null;
        onDone();
    }

    /// <summary>
    /// Callback will be called after the frame rendering is completed
    /// </summary>
    /// <param name="onDone"></param>
    /// <returns></returns>
    public static Coroutine WaitForEndOfFrame(UnityAction onDone)
    {
        return CoroutineHelper.StartGlobalCoroutine(WaitForEndOfFrameEnumerate(onDone));
    }

    private static IEnumerator WaitForEndOfFrameEnumerate(UnityAction onDone)
    {
        yield return new WaitForEndOfFrame();
        onDone();
    }

    public static Coroutine WaitForFrames(int frames, UnityAction onDone)
    {
        return CoroutineHelper.StartGlobalCoroutine(WaitForFramesEnumerate(frames, onDone));
    }

    private static IEnumerator WaitForFramesEnumerate(int frames, UnityAction onDone)
    {
        int count = 0;
        while (count < frames)
        {
            yield return new WaitForEndOfFrame();
            count++;
        }
        onDone();
    }

    public static Coroutine DoWhile(Func<bool> criteria, UnityAction onFrame, UnityAction onDone = null)
    {
        return CoroutineHelper.StartGlobalCoroutine(DoWhileEnumerate(criteria, onFrame, onDone));
    }

    private static IEnumerator DoWhileEnumerate(Func<bool> criteria, UnityAction onFrame, UnityAction onDone = null)
    {
        while (criteria())
        {
            if (onFrame != null) onFrame.Invoke();
            yield return null;
        }
        onDone?.Invoke();
    }
}


public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper _instance;

    private static void EnsureExists()
    {
        if (_instance == null)
        {
            var go = new GameObject("CoroutineHelper");
            _instance = go.AddComponent<CoroutineHelper>();
            DontDestroyOnLoad(go);
        }
    }

    public static Coroutine StartGlobalCoroutine(IEnumerator _toRun)
    {
        EnsureExists();
        return _instance.StartCoroutine(_toRun);
    }

    public static void StopGlobalCoroutine(Coroutine coroutine)
    {
        if (_instance != null && coroutine != null)
            _instance.StopCoroutine(coroutine);
    }
}