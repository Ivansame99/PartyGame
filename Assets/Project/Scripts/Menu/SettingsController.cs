using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
	[SerializeField]
	private GameObject playButton;

	[SerializeField]
	private GameObject settingsContainer;

	private FMOD.Studio.VCA vcaMasterController;
	private FMOD.Studio.VCA vcaMusicController;
	private FMOD.Studio.VCA vcaSFXController;

	[SerializeField]
	private GameObject dropDown;

	[SerializeField]
	private Slider masterSlider;

	[SerializeField]
	private Slider musicSlider;

	[SerializeField]
	private Slider sfxSlider;

	[SerializeField]
	private GameObject cursor;

	private Animator anim;

	[HideInInspector]
	public bool setingsOn;

	private EventSystem eventSystem;

	private GameObject lastButtonSelected;
	[SerializeField] private UISoundManager soundManager;

	private string changeSliderPath = "event:/SFX/UI/UpDown";

	private void Awake()
	{
		anim = GetComponent<Animator>();
		eventSystem = EventSystem.current;
	}
	// Start is called before the first frame update
	void Start()
	{
		dropDown.GetComponent<TMP_Dropdown>().value = PlayerPrefs.GetInt("GraphicsQuality", 1);
		vcaMasterController = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
		vcaMusicController = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
		vcaSFXController = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
		masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
		musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
		settingsContainer.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (!setingsOn) return;

		CheckIfAnyButtonSelected();
		if (eventSystem.currentSelectedGameObject != lastButtonSelected)
		{
			lastButtonSelected = eventSystem.currentSelectedGameObject;
			if (lastButtonSelected.GetComponent<ButtonCursorPos>() != null) cursor.transform.position = lastButtonSelected.GetComponent<ButtonCursorPos>().cursorPos.position;
		}

		if ((Gamepad.current != null && Gamepad.current.buttonEast.isPressed) || Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			Hide();
			if (soundManager != null) soundManager.CancelButtonSound();
		}
	}

	void CheckIfAnyButtonSelected()
	{
		if (eventSystem.currentSelectedGameObject == null) eventSystem.SetSelectedGameObject(dropDown);
	}

	public void UiChangeGraphicsQuality(int value)
	{
		QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel());
		PlayerPrefs.SetInt("GraphicsQuality", value);
	}

	public void UiChangeMasterVolume(float value)
	{
		FMODUnity.RuntimeManager.PlayOneShot(changeSliderPath);
		vcaMasterController.setVolume(value);
		PlayerPrefs.SetFloat("MasterVolume", value);
	}

	public void UiChangeMusicVolume(float value)
	{
		if (value <= 0) return;
		FMODUnity.RuntimeManager.PlayOneShot(changeSliderPath);
		vcaMusicController.setVolume(value);
		PlayerPrefs.SetFloat("MusicVolume", value);
	}

	public void UiChangeSFXVolume(float value)
	{
		if (value <= 0) return;
		FMODUnity.RuntimeManager.PlayOneShot(changeSliderPath);
		vcaSFXController.setVolume(value);
		PlayerPrefs.SetFloat("SfxVolume", value);
	}

	public void Show()
	{
		settingsContainer.SetActive(true);
		anim.SetTrigger("Appear");
		setingsOn = true;
		eventSystem.SetSelectedGameObject(dropDown);
	}

	private void Hide()
	{
		anim.SetTrigger("Disappear");
		setingsOn = false;
		Invoke(nameof(SetActiveFalse), 1.5f);
		eventSystem.SetSelectedGameObject(playButton);
	}

	private void SetActiveFalse()
	{
		settingsContainer.SetActive(false);
	}
}
