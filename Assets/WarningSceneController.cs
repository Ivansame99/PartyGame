using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class WarningSceneController : MonoBehaviour
{

	[Header("Circle Transition")]
	[SerializeField]
	private Material transitionMaterial;
	[SerializeField]
	private float transitionTime = 3f;
	[SerializeField]
	private string propertyName = "_Progress";

	private bool startedTransition=false;

	void Update()
	{
		if ((Gamepad.current != null && Gamepad.current.buttonSouth.isPressed) || (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame))
		{
			if (!startedTransition)
			{
				StartCoroutine(CloseTranition());
				startedTransition = true;
			}
		}
	}

	private IEnumerator CloseTranition()
	{
		float currentTime = transitionTime;
		while (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			transitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime / transitionTime));
			yield return null;
		}

		SceneManager.LoadScene("Menu");
	}
}
