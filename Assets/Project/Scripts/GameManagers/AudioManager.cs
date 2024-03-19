using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	//public Audio[] sounds;

	public static AudioManager Instance;



	//	foreach (var sound in sounds)
	//	{
	//		sound.source = gameObject.AddComponent<AudioSource>();
	//		sound.source.clip = sound.clip;

	//		sound.source.volume = sound.volume;
	//		sound.source.pitch = sound.pitch;
	//		sound.source.loop = sound.loop;
	//		sound.source.outputAudioMixerGroup = sound.mixer;

	//	}
	//}

	//private void Start()
	//{
	//	PlaySound("BackgroundMusic");
	//}

	//public void PlaySound(string name)
	//{
	//	Audio audio = Array.Find(sounds, sound => sound.name == name);
	//	if (audio == null)
	//	{
	//		Debug.LogError("Audio: " + name + " not found");
	//		return;
	//	}

	//	audio.source.Play();
	//}

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip victoryMusic; // La nueva música que deseas reproducir

	[SerializeField]
	private float fadeDuration = 1.0f; // La duración del fade in/out en segundos

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		//DontDestroyOnLoad(gameObject);
	}

	public void ChangeToVictoryTheme()
	{
		StartCoroutine(FadeOutThenIn());
	}

	private IEnumerator FadeOutThenIn()
	{
		float tiempoInicio = Time.time;
		float volumenInicial = audioSource.volume;

		while (Time.time < tiempoInicio + fadeDuration)
		{
			float t = (Time.time - tiempoInicio) / fadeDuration;
			audioSource.volume = Mathf.Lerp(volumenInicial, 0, t);
			yield return null;
		}

		// Cambiar la música
		audioSource.clip = victoryMusic;
		audioSource.Play();

		tiempoInicio = Time.time;

		while (Time.time < tiempoInicio + fadeDuration)
		{
			float t = (Time.time - tiempoInicio) / fadeDuration;
			audioSource.volume = Mathf.Lerp(0, volumenInicial, t);
			yield return null;
		}

		// Ajustar el volumen al valor final
		audioSource.volume = volumenInicial;
	}
}
