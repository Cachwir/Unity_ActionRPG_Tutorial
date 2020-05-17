using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public delegate void TransitionEffectManager_Callback();

public class TransitionEffectManager : MonoBehaviour {

    public enum TransitionEffect
    {
        BLACK_IN,
        BLACK_OUT
    }

    public TransitionEffect inEffect;
    public float inTransitionDuration;

    public TransitionEffect outEffect;
    public float outTransitionDuration;

    protected TransitionEffect currentTransition;
    protected float currentTransitionDuration;

    protected UIManager uiManager;
    protected CanvasGroup canvasGroup;

    protected RawImage fadeScreen;

    protected float transitionStartTime;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        canvasGroup = uiManager.GetComponent<CanvasGroup>();
        fadeScreen = Helper.GetChildByComponent(uiManager, "RawImage").GetComponent<RawImage>();
    }

    private void FixedUpdate()
    {
        if (transitionStartTime > 0 && currentTransitionDuration + transitionStartTime < Time.fixedTime)
        {
            OnPlayEffect();
        }
    }

    public void PlayInEffect(TransitionEffectManager_Callback callback)
    {
        PlayEffect(inEffect, inTransitionDuration, callback);
    }

    public void PlayOutEffect(TransitionEffectManager_Callback callback)
    {
        PlayEffect(outEffect, outTransitionDuration, callback);
    }

    public void PlayEffect(TransitionEffect effect, float duration, TransitionEffectManager_Callback callback)
    {
        StartCoroutine(CallAfterTime(callback, duration));

        switch (effect)
        {
            case TransitionEffect.BLACK_IN:
                BlackIn(duration);
                break;

            case TransitionEffect.BLACK_OUT:
                BlackOut(duration);
                break;
                
            default:
                currentTransitionDuration = duration;
                currentTransition = effect;
                transitionStartTime = Time.deltaTime;
                break;
        }
    }

    public void OnPlayEffect()
    {
        switch (currentTransition)
        {
            default:
                break;
        }
    }

    public void BlackIn(float duration)
    {
        fadeScreen.CrossFadeAlpha(0f, 0f, true);

        fadeScreen.enabled = true;
        fadeScreen.CrossFadeAlpha(1, duration, true);
    }

    public void BlackOut(float duration)
    {
        fadeScreen.CrossFadeAlpha(1f, 0f, true);

        fadeScreen.enabled = true;
        fadeScreen.CrossFadeAlpha(0, duration, true);

        StartCoroutine(CallAfterTime(delegate ()
        {
            fadeScreen.enabled = false;
        }, duration));
    }

    IEnumerator CallAfterTime(TransitionEffectManager_Callback callback,  float time)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
