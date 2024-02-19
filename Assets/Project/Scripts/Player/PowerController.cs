using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;
using TMPro;
using Unity.VisualScripting;
using System;

public class PowerController : MonoBehaviour
{
    //Variables de poder
    private float currentPowerLevel;

    [SerializeField]
    private float minPowerLevel = 10;

    [SerializeField]
    private float maxPowerLevel = 300; //Habra que hacer pruebas

    [SerializeField]
    private float powerScale; //Reduce it to more damage

	[SerializeField]
	private float healthScale; //Reduce it to more health scale

	//Variables de escalado
	private float scaleMultiplayer;

    [SerializeField]
    private float maxScaleMultiplier = 2;

    [SerializeField]
    private float minScaleMultiplier = 1;

    private Vector3 originalScale;

    [SerializeField]
    private GameObject powerLevel;

    private TMP_Text powerLevelText;

    private Canvas canvas;

    private GameObject playerUI;
    private TMP_Text playerUIPowerText;

    private bool isEnemy=false;

	public Action<float> OnCurrentPowerChanged;

	void Start()
    {
        powerLevelText = powerLevel.GetComponent<TMP_Text>();
        canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        SetupPowerLevelCanvas();
        originalScale = transform.localScale;
        if(this.gameObject.tag=="Enemy") isEnemy = true;

        if (!isEnemy)
        {
            playerUI = GameObject.FindGameObjectWithTag("UI" + this.gameObject.name);
            if(playerUI!=null) playerUIPowerText = playerUI.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        }

        SetCurrentPowerLevel(minPowerLevel);
    }

    void Update()
    {
        //Formula para obtener el escalado del personaje
        float totalRange = maxPowerLevel - minPowerLevel;
        float scaleMultiplayer = ((currentPowerLevel - minPowerLevel) / totalRange) * (maxScaleMultiplier - minScaleMultiplier) + minScaleMultiplier;
        
        scaleMultiplayer = Mathf.Clamp(scaleMultiplayer, minScaleMultiplier, maxScaleMultiplier);

        this.gameObject.transform.localScale = originalScale * scaleMultiplayer;
        //StartCoroutine(ChangeScale, this.gameObject.transform, originalScale);
    }

    private void SetupPowerLevelCanvas()
    {
        powerLevel.transform.SetParent(canvas.transform);
    }

    public float GetCurrentPowerLevel()
    {
        return currentPowerLevel;
    }

    public void SetCurrentPowerLevel(float value)
    {
        currentPowerLevel = Mathf.RoundToInt(currentPowerLevel += value);
		if (OnCurrentPowerChanged != null) OnCurrentPowerChanged(currentPowerLevel);
		ChangeUIText();
    }

    public void OnDieSetCurrentPowerLevel()
    {
        currentPowerLevel = Mathf.RoundToInt(currentPowerLevel = currentPowerLevel/2);
        if (currentPowerLevel <= 0) currentPowerLevel = 1; //Que no pueda bajar de uno
		if (OnCurrentPowerChanged != null) OnCurrentPowerChanged(currentPowerLevel);
		ChangeUIText();
    }

    private void ChangeUIText()
    {
        powerLevelText.SetText(currentPowerLevel.ToString());
        if(!isEnemy && playerUI!=null) playerUIPowerText.SetText(currentPowerLevel.ToString());
    }

    public float PowerDamage()
    {
        return currentPowerLevel / powerScale;
	}

	public float PowerHealth()
	{
		return currentPowerLevel / healthScale;
	}

	IEnumerator ChangeScale(Transform transform, Vector3 originalScale, Vector3 upScale, float duration)
    {
        //Vector3 initialScale = transform.localScale;

        for (float time = 0; time < duration * 2; time += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, upScale, time);
            yield return null;
        }
    }
}
