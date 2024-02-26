using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsContainer;

	[SerializeField]
	private GameObject defaultButton;

	[SerializeField]
	private GameObject cursor;

	private Animator anim;

    [HideInInspector]
    public bool setingsOn;

	private EventSystem eventSystem;

	private GameObject lastButtonSelected;

	private void Awake()
    {
        anim = GetComponent<Animator>();
		eventSystem = EventSystem.current;
	}
    // Start is called before the first frame update
    void Start()
    {
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
	}

	void CheckIfAnyButtonSelected()
	{
		Debug.Log(eventSystem.currentSelectedGameObject);
		if (eventSystem.currentSelectedGameObject == null) eventSystem.SetSelectedGameObject(defaultButton);
	}

	public void UiChangeGraphicsQuality(int value)
    {
        QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel());
    }

    public void Show()
    {
        settingsContainer.SetActive(true);
        anim.SetTrigger("Appear");
        setingsOn = true;
		eventSystem.SetSelectedGameObject(defaultButton);
	}
}
