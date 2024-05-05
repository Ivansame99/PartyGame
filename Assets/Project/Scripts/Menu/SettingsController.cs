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

	[SerializeField]
	private AudioMixer audioMixer;

	[SerializeField]
	private AudioSource sliderChangeValueSFX;

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

	private void Awake()
    {
        anim = GetComponent<Animator>();
		eventSystem = EventSystem.current;
	}
    // Start is called before the first frame update
    void Start()
    {
		dropDown.GetComponent<TMP_Dropdown>().value = PlayerPrefs.GetInt("GraphicsQuality", 2);
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
			if(lastButtonSelected.GetComponent<ButtonCursorPos>()!=null) cursor.transform.position = lastButtonSelected.GetComponent<ButtonCursorPos>().cursorPos.position;
		}

		if (Gamepad.current != null && Gamepad.current.buttonEast.isPressed)
		{
			Hide();
			soundManager.CancelButtonSound();
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
		//if (!sliderChangeValueSFX.isPlaying && setingsOn) sliderChangeValueSFX.Play();
		audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
		PlayerPrefs.SetFloat("MasterVolume", value);
	}

	public void UiChangeMusicVolume(float value)
	{
		//if (!sliderChangeValueSFX.isPlaying && setingsOn) sliderChangeValueSFX.Play();
		audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
		PlayerPrefs.SetFloat("MusicVolume", value);
	}

	public void UiChangeSFXVolume(float value)
	{
		if (!sliderChangeValueSFX.isPlaying && setingsOn) sliderChangeValueSFX.Play();
		audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
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
