using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PowerController : MonoBehaviour
{
    //Variables de poder
    [SerializeField]
    private float currentPowerLevel;
    private float minPowerLevel = 0;
    private float maxPowerLevel = 300; //Habra que hacer pruebas

    //Variables de escalado
    private float scaleMultiplayer;
    private float maxScaleMultiplier = 2;
    private float minScaleMultiplier = 1;

    private Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        //ChangePowerLevel();
    }

    // Update is called once per frame
    void Update()
    {

        //Formula para obtener el escalado del personaje
        float totalRange = maxPowerLevel - minPowerLevel;
        float scaleMultiplayer = ((currentPowerLevel - minPowerLevel) / totalRange) * (maxScaleMultiplier - minScaleMultiplier) + minScaleMultiplier;
        
        scaleMultiplayer = Mathf.Clamp(scaleMultiplayer, minScaleMultiplier, maxScaleMultiplier);
        //Debug.Log("Valor calculado para variable2: " + scaleMultiplayer);

        this.gameObject.transform.localScale = originalScale * scaleMultiplayer;
    }

    public void ChangePowerLevel()
    {
        this.gameObject.transform.localScale = originalScale * scaleMultiplayer;
    }
}
