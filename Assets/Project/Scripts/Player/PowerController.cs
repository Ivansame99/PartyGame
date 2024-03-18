using UnityEngine;
using TMPro;
using System;

public class PowerController : MonoBehaviour
{
	#region Inspector Variables
	[SerializeField]
	private float initialPowerLevel;

	[SerializeField]
	private float minPowerLevel;

	[SerializeField]
	private float maxPowerLevel;

	[SerializeField]
	private float powerScale;

	[SerializeField]
	private float healthScale;

	[SerializeField]
	private float maxScaleMultiplier = 2;

	[SerializeField]
	private float minScaleMultiplier = 0.7f;

	[SerializeField]
	private GameObject smoke;

	[SerializeField]
	private bool instanciatedEnemy = false;
	#endregion

	#region Variables
	private PlayerHudController playerHudController;
	private EnemyHudController enemyHudController;

	private float currentPowerLevel;
	private bool isEnemy = false;
	private bool maxPowerParticlesSpawned = false;
	#endregion

	#region Actions
	public Action<float> OnCurrentPowerChanged;
	#endregion

	void Start()
	{
		if (this.gameObject.CompareTag("Enemy")) isEnemy = true;

		if (!isEnemy)
		{
			playerHudController = this.GetComponent<PlayerHudController>();
		} else
		{
			enemyHudController = this.GetComponent<EnemyHudController>();
		}

		if(!instanciatedEnemy) InitializePowerLevel(initialPowerLevel);
	}

	public void ChangeScale()
	{
		float scale = MapValues(currentPowerLevel, minPowerLevel, maxPowerLevel, minScaleMultiplier, maxScaleMultiplier);
		float scaleClamped = Mathf.Clamp(scale, minScaleMultiplier, maxScaleMultiplier);
		this.gameObject.transform.localScale = new Vector3(scaleClamped, scaleClamped, scaleClamped);
	}

	public void AddPowerLevel(float value)
	{
		currentPowerLevel = Mathf.RoundToInt(currentPowerLevel += value);
		if (OnCurrentPowerChanged != null) OnCurrentPowerChanged(currentPowerLevel);
		Feedback();
	}

	public void InitializePowerLevel(float value)
	{
		currentPowerLevel = value;
		Feedback();
		if (OnCurrentPowerChanged != null) OnCurrentPowerChanged(currentPowerLevel);
	}

	public void OnDieSetCurrentPowerLevel()
	{
		currentPowerLevel = Mathf.RoundToInt(currentPowerLevel = currentPowerLevel / 2);
		if (currentPowerLevel <= 0) currentPowerLevel = 1; //Que no pueda bajar de uno
		if (OnCurrentPowerChanged != null) OnCurrentPowerChanged(currentPowerLevel);
		Feedback();
	}

	public float PowerDamage()
	{
		return currentPowerLevel / powerScale;
	}

	public float PowerHealth()
	{
		return currentPowerLevel / healthScale;
	}

	public float GetCurrentPowerLevel()
	{
		return currentPowerLevel;
	}

	private float MapValues(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
	}

	private void CheckPowerParticles()
	{
		if (currentPowerLevel >= maxPowerLevel)
		{
			smoke.SetActive(true);
		} else
		{
			smoke.SetActive(false);
		}
	}

	private void Feedback()
	{
		ChangeScale();
		CheckPowerParticles();
		if (isEnemy)
		{
			enemyHudController.ChangeUIPower(currentPowerLevel);
		} else
		{
			playerHudController.ChangeUIPower(currentPowerLevel);
		}
	}
}
