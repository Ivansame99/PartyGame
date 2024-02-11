using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensity : MonoBehaviour
{
	public static LightIntensity Instance;
	private Light directionalLight;

	private void Awake()
	{
		Instance = this;
		directionalLight = GetComponent<Light>();
	}

	private void ChangeIntensity(float targetIntensity, float duration)
	{
		StartCoroutine(IChangeIntensityOverTime(targetIntensity, duration));
	}

	IEnumerator IChangeIntensityOverTime(float targetIntensity, float duration)
	{
		float startIntensity = directionalLight.intensity;
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			// Calcula el nuevo valor de intensidad utilizando la interpolación lineal
			float newIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);

			// Aplica el nuevo valor de intensidad a la luz direccional
			directionalLight.intensity = newIntensity;

			// Incrementa el tiempo transcurrido
			elapsedTime += Time.deltaTime;

			// Espera un frame antes de continuar con la siguiente iteración del bucle
			yield return null;
		}

		// Asegúrate de que la intensidad final sea la deseada
		directionalLight.intensity = targetIntensity;
	}

	public static void ChangeIntensityOverTime(float targetIntensity, float duration) => Instance.ChangeIntensity(targetIntensity, duration);
}
