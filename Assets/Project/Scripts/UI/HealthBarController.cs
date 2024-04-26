using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
	[SerializeField]
	private Image fillImage;
	[SerializeField]
    private float DefaultSpeed = 1f;
    [SerializeField]
    private UnityEvent<float> OnProgress;
    [SerializeField]
    private UnityEvent OnCompleted;
	[SerializeField]
	private Gradient gradientColor;

	private Slider slider;
	private Coroutine AnimationCoroutine;

    private void Start()
    {
        slider = this.GetComponent<Slider>();
		fillImage.color = gradientColor.Evaluate(1 - slider.value);
	}

    public void SetProgress(float Progress)
    {
        SetProgress(Progress, DefaultSpeed);
    }

    public void SetProgress(float Progress, float Speed)
    {
        if (Progress < 0 || Progress > 1)
        {
            //Debug.LogWarning($"Invalid progress passed, expected value is between 0 and 1, got {Progress}. Clamping.");
            Progress = Mathf.Clamp01(Progress);
        }
        if (Progress != slider.value)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            if(this.gameObject.activeInHierarchy) AnimationCoroutine = StartCoroutine(AnimateProgress(Progress, Speed));
        }
    }

    private IEnumerator AnimateProgress(float Progress, float Speed)
    {
        float time = 0;
        float initialProgress = slider.value;

        while (time < 1)
        {
			slider.value = Mathf.Lerp(initialProgress, Progress, time);
            time += Time.deltaTime * Speed;

            fillImage.color = gradientColor.Evaluate(1- slider.value);
            OnProgress?.Invoke(slider.value);
            yield return null;
        }

		slider.value = Progress;
		fillImage.color = gradientColor.Evaluate(1 - slider.value);
        OnProgress?.Invoke(Progress);
        OnCompleted?.Invoke();
    }
}
