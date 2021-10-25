using System;
using UnityEngine;
using System.Collections;

namespace UDK
{
    public static class MonoBehaviourTweenExt
    {
        public static void Tween(this MonoBehaviour self, float duration, Action<float> onProgress, Action onComplete = null)
        {
            self.StartCoroutine(self.TweenCoroutine(duration, onProgress, onComplete));
        }

        public static IEnumerator TweenCoroutine(this MonoBehaviour self, float duration, Action<float> onProgress, Action onComplete = null)
        {
            float start = Time.time;
            float elapsed = 0;

            while (elapsed < duration)
            {
                elapsed = Time.time - start;
                float normalisedTime = Mathf.Clamp01(elapsed / duration);

                onProgress?.Invoke(normalisedTime);

                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}