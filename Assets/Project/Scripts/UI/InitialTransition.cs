using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialTransition : MonoBehaviour
{
	[Header("Circle Transition")]
	[SerializeField]
	private Material transitionMaterial;
	[SerializeField]
	private float transitionTime = 3f;
	[SerializeField]
	private string propertyName = "_Progress";

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(InitialTransitionCoroutine());
	}

	private IEnumerator InitialTransitionCoroutine()
	{
		float currentTime = 0;
		while (currentTime < transitionTime)
		{
			currentTime += Time.deltaTime;
			transitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime / transitionTime));
			yield return null;
		}
	}
}
