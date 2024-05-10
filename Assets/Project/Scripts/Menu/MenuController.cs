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

	[SerializeField]
	private SettingsController settingsController;

	[Header("Circle Transition")]
	[SerializeField]
	private Material transitionMaterial;
	[SerializeField]
	private float transitionTime = 3f;
	[SerializeField]
	private string propertyName = "_Progress";

	private EventSystem eventSystem;

	private GameObject lastButtonSelected;

	bool start;

    [SerializeField]
    private Music menuMusic;

    private void Awake()
	{
		eventSystem = EventSystem.current;
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.1f);
		transitionMaterial.SetFloat(propertyName, 1);
		Destroy(GameObject.Find("PlayerMultiManager"));
		cursor.transform.position = defaultButton.GetComponent<ButtonCursorPos>().cursorPos.position;
		start = true;
	}

	private void Update()
	{
		if (settingsController.setingsOn) return;

		if (start)
		{
			CheckIfAnyButtonSelected();

			if (eventSystem.currentSelectedGameObject != lastButtonSelected)
			{
				lastButtonSelected = eventSystem.currentSelectedGameObject;

				cursor.transform.position = lastButtonSelected.GetComponent<ButtonCursorPos>().cursorPos.position;
			}
		}
	}

	void CheckIfAnyButtonSelected()
	{
		if (eventSystem.currentSelectedGameObject == null) eventSystem.SetSelectedGameObject(defaultButton);
	}

	public void UiPlayButton()
	{
		eventSystem.enabled = false;
		StartCoroutine(CloseTranition());
	}

	public void UiSettingsButton()
	{
		settingsController.Show();
	}

	public void UiCreditsButton()
	{
        menuMusic.StopMusic();
        SceneManager.LoadScene("Credits");
	}

	public void UiExitButton()
	{
		eventSystem.enabled = false;
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
		menuMusic.StopMusic();

        SceneManager.LoadScene("HUB");
	}
}
