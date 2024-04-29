using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameEnums;

public class GameOverController : MonoBehaviour
{
	[SerializeField]
	private Image backgroundImage;

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

	[SerializeField]
	private Sprite[] arenasSprite;

	private EventSystem eventSystem;

	private GameObject lastButtonSelected;

	private void Awake()
	{
		eventSystem = EventSystem.current;
	}

	public void Start()
	{
		transitionMaterial.SetFloat(propertyName, 1);
		backgroundImage.sprite = arenasSprite[PlayerPrefs.GetInt("arenaType")];
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
		eventSystem.enabled = false;
		int arenaType = PlayerPrefs.GetInt("arenaType");
		GameEnums.Arenas arena = (GameEnums.Arenas)arenaType;

		switch (arena)
		{
			case Arenas.StandardArena:
				StartCoroutine(CloseTranition(GameEnums.Scenes.Arena1.ToString()));
				break;
			case Arenas.SnowArena:
				StartCoroutine(CloseTranition(GameEnums.Scenes.ArenaSnow.ToString()));
				break;
			default:
				StartCoroutine(CloseTranition(GameEnums.Scenes.Arena1.ToString()));
				break;
		}
    }

    public void UiExitMenu()
    {
		eventSystem.enabled = false;
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
