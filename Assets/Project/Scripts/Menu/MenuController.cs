using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	[SerializeField]
	private GameObject defaultButton;

	[SerializeField]
	private GameObject cursor;

	[SerializeField]
	private Sprite buttonPressedSprite;

	[Header("Circle Transition")]
	[SerializeField]
	private Material transitionMaterial;
	[SerializeField]
	private float transitionTime = 3f;
	[SerializeField]
	private string propertyName = "_Progress";

	private EventSystem eventSystem;

	private GameObject lastButtonSelected;

	private void Awake()
	{
		eventSystem = EventSystem.current;
	}

	public void Start()
	{
		transitionMaterial.SetFloat(propertyName, 1);
		Destroy(GameObject.Find("PlayerMultiManager"));
	}

	private void Update()
	{
		CheckIfAnyButtonSelected();
		if (eventSystem.currentSelectedGameObject != lastButtonSelected)
		{
			lastButtonSelected = eventSystem.currentSelectedGameObject;

			cursor.transform.position = lastButtonSelected.GetComponent<ButtonCursorPos>().cursorPos.position;
		}
	}

	void CheckIfAnyButtonSelected()
	{
		if (eventSystem.currentSelectedGameObject == null) eventSystem.SetSelectedGameObject(defaultButton);
	}

	public void UiPlayButton()
	{
		eventSystem.enabled = false;
		lastButtonSelected.GetComponent<Image>().sprite = buttonPressedSprite;
		StartCoroutine(CloseTranition());
	}

	public void UiSettingsButton()
	{
		Debug.Log("To do");
	}

	public void UiCreditsButton()
	{
		SceneManager.LoadScene("Credits");
	}

	public void UiExitButton()
	{
		eventSystem.enabled = false;
		lastButtonSelected.GetComponent<Image>().sprite = buttonPressedSprite;
		Invoke(nameof(ExitGame), 0.2f);
	}

	private void ExitGame()
	{
		Application.Quit();
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

		SceneManager.LoadScene("HUB");
	}
}
