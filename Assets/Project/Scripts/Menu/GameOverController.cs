using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
	[SerializeField]
	private GameObject defaultButton;

	[SerializeField]
	private GameObject cursor;

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

	public void UiRestartButton()
    {
		StartCoroutine(CloseTranition("Arena1"));
    }

    public void UiExitMenu()
    {
		StartCoroutine(CloseTranition("Menu"));
    }

	private IEnumerator CloseTranition(string levelName)
	{
		float currentTime = transitionTime;
		while (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			transitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime / transitionTime));
			yield return null;
		}

		SceneManager.LoadScene(levelName);
	}
}
