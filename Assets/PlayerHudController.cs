using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHudController : MonoBehaviour
{
	#region Inspector Variables
	[SerializeField]
	private HealthBarController healthBarC;

	[SerializeField]
	private GameObject healthBar;

	[SerializeField]
	private GameObject powerLevel;

	[SerializeField] 
	private GameObject floatingDamageText;
	#endregion

	#region Variables
	private Animator healthBarAnimator;
	private Canvas canvas;
	private HealthBarController playerUIHealth;
	private Animator playerUIHealthAnimator;
	private GameObject playerUI;
	private TMP_Text powerLevelText;
	private TMP_Text playerUIPowerText;
	#endregion

	#region Life Cycle

	private void Awake()
	{
		canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
		powerLevelText = powerLevel.GetComponent<TMP_Text>();
	}

	void Start()
	{
		SetupPowerLevelCanvas();

		healthBarAnimator = healthBar.gameObject.GetComponent<Animator>();
		SetupHealthBar();

		playerUI = GameObject.FindGameObjectWithTag("UI" + this.gameObject.name);
		if (playerUI != null)
		{
			playerUIHealthAnimator = playerUI.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
			playerUIHealth = playerUI.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<HealthBarController>();
			playerUIPowerText = playerUI.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
		}
	}
	#endregion

	#region Public Methods
	public void ReceivedDamage(float damage, float health, float maxHealth)
	{
		healthBarAnimator.SetTrigger("Damage");
		if (playerUI != null) playerUIHealthAnimator.SetTrigger("Damage");
		TMP_Text text = Instantiate(floatingDamageText, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
		text.text = ((int)damage).ToString();
		ChangeUIHealth(health, maxHealth);
	}

	public void ChangeUIHealth(float health, float maxHealth)
	{
		healthBarC.SetProgress(health / maxHealth, 2);
		if (playerUI != null) playerUIHealth.SetProgress(health / maxHealth, 2);
	}

	public void ChangeUIPower(float powerLevel)
	{
		powerLevelText.SetText(powerLevel.ToString());
		if (playerUI != null) playerUIPowerText.SetText(powerLevel.ToString());
	}

	public void DisableHud()
	{
		healthBar.gameObject.SetActive(false);
		powerLevel.SetActive(false);
	}

	public void EnableHud()
	{
		healthBar.gameObject.SetActive(true);
		powerLevel.SetActive(true);
	}
	#endregion

	#region Private Methods
	private void SetupHealthBar()
	{
		healthBar.transform.SetParent(canvas.transform);
	}

	private void SetupPowerLevelCanvas()
	{
		powerLevel.transform.SetParent(canvas.transform);
	}
	#endregion
}
