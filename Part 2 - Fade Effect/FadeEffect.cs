using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    [SerializeField]
    private float _fadeDelay = 0.07f;
    private Material _material;

    private bool _isFadingOut = false;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    public void Fade(bool fadeOut)
    {
        if (fadeOut && _isFadingOut)
            return;
        if (!fadeOut && !_isFadingOut)
            return;
        _isFadingOut = fadeOut;
        StopAllCoroutines();
        string val = _isFadingOut ? "OUT" : "in";
        Debug.Log($"Starting fade {val} coroutine");
        StartCoroutine(PlayEffect(fadeOut));
    }

    private IEnumerator PlayEffect(bool fadeOut)
    {
        float startAlpha = _material.GetFloat("_Alpha");
        float endAlpha = fadeOut ? 1.0f : 0.0f;
        float remainingTime
            = _fadeDelay * Mathf.Abs(endAlpha - startAlpha);

        float elapsedTime = 0;
        while (elapsedTime < _fadeDelay)
        {
            elapsedTime += Time.deltaTime;
            float tempVal = Mathf.Lerp(startAlpha, endAlpha,
                elapsedTime / remainingTime);

            _material.SetFloat("_Alpha", tempVal);
            yield return null;
        }
        _material.SetFloat("_Alpha", endAlpha);
    }
}
