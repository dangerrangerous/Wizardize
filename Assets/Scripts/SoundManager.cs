﻿using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public AudioSource fxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;

	public float lowPitchRange = 0.8f;
	public float highPitchRange = 1.2f;
	// Use this for initialization
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle(AudioClip clip)
	{
		fxSource.clip = clip;
		fxSource.Play ();
	}

	public void RandomizeSfx(params AudioClip [] clips)
	{
		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		fxSource.pitch = randomPitch;
		fxSource.clip = clips [randomIndex];
		fxSource.Play ();
	}


}
