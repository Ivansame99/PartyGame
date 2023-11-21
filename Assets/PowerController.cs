using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;
using TMPro;

public class PowerController : MonoBehaviour
{
    //Variables de poder
    [SerializeField]
    private float currentPowerLevel;

    [SerializeField]
    private float minPowerLevel = 0;

    [SerializeField]
    private float maxPowerLevel = 300; //Habra que hacer pruebas

    //Variables de escalado
    private float scaleMultiplayer;

    [SerializeField]
    private float maxScaleMultiplier = 2;

    [SerializeField]
    private float minScaleMultiplier = 1;

    private Vector3 originalScale;

    //[SerializeField]
    //private  healthBarC;

    [SerializeField]
    private GameObject powerLevel;

    private TMP_Text powerLevelText;

    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        powerLevelText = powerLevel.GetComponent<TMP_Text>();
        canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        SetupPowerLevelCanvas();
        originalScale = transform.localScale;
        //ChangePowerLevel();
    }

    // Update is called once per frame
    void Update()
    {
        powerLevelText.SetText("33");
        //Formula para obtener el escalado del personaje
        float totalRange = maxPowerLevel - minPowerLevel;
        float scaleMultiplayer = ((currentPowerLevel - minPowerLevel) / totalRange) * (maxScaleMultiplier - minScaleMultiplier) + minScaleMultiplier;
        
        scaleMultiplayer = Mathf.Clamp(scaleMultiplayer, minScaleMultiplier, maxScaleMultiplier);

        this.gameObject.transform.localScale = originalScale * scaleMultiplayer;
    }

    public void ChangePowerLevel(float level)
    {
        currentPowerLevel += level;
    }

    private void SetupPowerLevelCanvas()
    {
        powerLevel.transform.SetParent(canvas.transform);
    }
}
