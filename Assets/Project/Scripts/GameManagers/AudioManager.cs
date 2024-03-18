using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public Audio[] sounds;

	public static AudioManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		} else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		foreach (var sound in sounds)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;

			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
			sound.source.loop = sound.loop;
			sound.source.outputAudioMixerGroup = sound.mixer;

		}
	}

	private void Start()
	{
		PlaySound("BackgroundMusic");
	}

	public void PlaySound(string name)
	{
		Audio audio = Array.Find(sounds, sound => sound.name == name);
		if (audio == null)
		{
			Debug.LogError("Audio: " + name + " not found");
			return;
		}

		audio.source.Play();
	}
}
