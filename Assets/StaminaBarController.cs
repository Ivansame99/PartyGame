using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    [SerializeField]
    private Image ProgressImage;
    [SerializeField]
    private float DefaultSpeed = 1f;
    [SerializeField]
    private UnityEvent<float> OnProgress;
    [SerializeField]
    private UnityEvent OnCompleted;

    private Coroutine AnimationCoroutine;

    public bool canRecover=true;

    private void Start()
    {
        if (ProgressImage.type != Image.Type.Filled)
        {
            Debug.LogError($"{name}'s ProgressImage is not of type \"Filled\" so it cannot be used as a progress bar. Disabling this Progress Bar.");
            enabled = false;
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(this.gameObject);
#endif
        }
    }

    public void SetProgress(float Progress)
    {
        SetProgress(Progress, DefaultSpeed);
    }

    public void RecoverBar(float Progress)
    {
        ProgressImage.fillAmount = Progress;
    }

    public void WasteBar(float Progress)
    {
        ProgressImage.fillAmount = Progress;
    }

    public void SetProgress(float Progress, float Speed)
    {
        if (Progress < 0 || Progress > 1)
        {
            Debug.LogWarning($"Invalid progress passed, expected value is between 0 and 1, got {Progress}. Clamping.");
            Progress = Mathf.Clamp01(Progress);
        }
        if (Progress != ProgressImage.fillAmount)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            canRecover = false;
            AnimationCoroutine = StartCoroutine(AnimateProgress(Progress, Speed));
        }
    }

    private IEnumerator AnimateProgress(float Progress, float Speed)
    {
        float time = 0;
        float initialProgress = ProgressImage.fillAmount;

        while (time < 1)
        {
            ProgressImage.fillAmount = Mathf.Lerp(initialProgress, Progress, time);
            time += Time.deltaTime * Speed;

            OnProgress?.Invoke(ProgressImage.fillAmount);
            yield return null;
        }

        ProgressImage.fillAmount = Progress;
        OnProgress?.Invoke(Progress);
        OnCompleted?.Invoke();
        canRecover = true;
    }
}
