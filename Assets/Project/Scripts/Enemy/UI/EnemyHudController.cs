using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHudController : MonoBehaviour
{
	[SerializeField]
	private GameObject powerLevel;

	private Canvas canvas;
	private TMP_Text powerLevelText;

	private void Awake()
	{
		canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
		powerLevelText = powerLevel.GetComponent<TMP_Text>();
	}

	private void Start()
	{
		SetupPowerLevelCanvas();
	}

	public void ChangeUIPower(float powerLevel)
	{
		powerLevelText.SetText(powerLevel.ToString());
	}

	private void SetupPowerLevelCanvas()
	{
		powerLevel.transform.SetParent(canvas.transform);
	}
}
