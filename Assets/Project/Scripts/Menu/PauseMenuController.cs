using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PauseMenuController : MonoBehaviour
{
    private Animator anim;
    bool pause = false;
	[SerializeField]
	private GameObject defaultButton;

	[SerializeField]
	private SettingsController settingsController;

	[SerializeField]
	private GameObject cursor;

	[SerializeField]
	private Music music;

	[Header("Circle Transition")]
	[SerializeField]
	private Material transitionMaterial;
	[SerializeField]
	private float transitionTime = 2f;
	[SerializeField]
	private string propertyName = "_Progress";

	private EventSystem eventSystem;

	private GameObject lastButtonSelected;

	private void Awake()
    {
        anim = GetComponent<Animator>();
		eventSystem = EventSystem.current;
	}

    void Update()
    {
		if (settingsController.setingsOn) return;

		if ((Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame) || (Keyboard.current!=null && Keyboard.current.escapeKey.wasPressedThisFrame))
		{
            if (pause)
            {
				//Time.timeScale = 1.0f;
				eventSystem.SetSelectedGameObject(null);
				pause = false;
				anim.SetBool("PauseAppear", false);
				StartCoroutine(SetTimeNormal());
			} else
            {
				eventSystem.SetSelectedGameObject(null);
				lastButtonSelected = defaultButton;
				StartCoroutine(SetStartCursosPos());
				Time.timeScale = 0.00001f;
				anim.SetBool("PauseAppear", true);
			}
		}

		if (pause)
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

	public void UiContinueButton()
    {
		pause = false;
		anim.SetBool("PauseAppear", false);
		StartCoroutine(SetTimeNormal());
	}

	public void UiSettingsButton()
	{
		settingsController.Show();
	}

	public void UiExitButton()
	{
		eventSystem.enabled = false;
		StartCoroutine(CloseTranition());
		//GameManager.Instance.gmSceneManager.ChangeSceneToMenu(true);
	}

	private IEnumerator CloseTranition()
	{
		float currentTime = transitionTime;
		while (currentTime > 0)
		{
			currentTime -= Time.unscaledDeltaTime;
			transitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime / transitionTime));
			yield return null;
		}

		Time.timeScale = 1.0f;
		if(music!=null) music.StopMusic();
		SceneManager.LoadScene("Menu");
	}

	private IEnumerator SetStartCursosPos()
	{
		yield return new WaitForSecondsRealtime(1f);
		cursor.transform.position = lastButtonSelected.GetComponent<ButtonCursorPos>().cursorPos.position;
		eventSystem.SetSelectedGameObject(defaultButton);
		pause = true;
	}

	private IEnumerator SetTimeNormal()
	{
		yield return new WaitForSecondsRealtime(0.5f);
		Time.timeScale = 1;
	}
}
