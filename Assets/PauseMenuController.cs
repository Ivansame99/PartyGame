using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    private Animator anim;
    bool pause = false;
	[SerializeField]
	private GameObject defaultButton;

	[SerializeField]
	private GameObject cursor;

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

    void Start()
    {
        
    }

    void Update()
    {
		if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
		{
            if (pause)
            {
				Time.timeScale = 1.0f;
				pause = false;
				anim.SetBool("PauseAppear", false);
			} else
            {
				eventSystem.SetSelectedGameObject(defaultButton);
				cursor.transform.position = defaultButton.GetComponent<ButtonCursorPos>().cursorPos.position;
				Time.timeScale = 0.0f;
                pause = true;
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
		Time.timeScale = 1.0f;
	}

	public void UiSettingsButton()
	{
        Debug.Log("To do");
	}

	public void UiExitButton()
	{
		eventSystem.enabled = false;
		StartCoroutine(CloseTranition());
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
		SceneManager.LoadScene("Menu");
	}
}
