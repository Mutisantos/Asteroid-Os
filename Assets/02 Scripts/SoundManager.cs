﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**Singleton for handling sound effects and music through the game 
 * Esteban.Hernandez
 */
public class SoundManager : MonoBehaviour {

	public AudioSource fxSource;
	public AudioSource playerSource;
	public AudioSource musicSource;

	public AudioSource asteroidSource;

	public static SoundManager instance = null;

	public AudioClip[] bgSceneMusic;

	public float lowPitch = 0.95f;
	public float highPitch  = 1.05f;


	void Awake() {
		MakeSingleton ();
	}

	private void MakeSingleton() {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	public void PlayOnce (AudioClip clip){
		fxSource.clip = clip;
		fxSource.pitch = 1f;
		fxSource.Play ();
	}

	public void PlayMeteorOnce (AudioClip clip){
		asteroidSource.clip = clip;
		asteroidSource.pitch = 1f;
		asteroidSource.Play ();
	}


	public void PlayPlayerOnce (AudioClip clip){
		playerSource.Stop ();
		playerSource.clip = clip;
		playerSource.Play ();
	}

	public bool isEffectPlaying(){
		return fxSource.isPlaying;
	}

	public bool isPlayerPlaying(){
		return playerSource.isPlaying;
	}

	public void stopPlayerSounds(){
		playerSource.Stop();
	}

	public void RandomizePlayerFx (params AudioClip [] clips){
		
		int random = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (this.lowPitch, this.highPitch);

		playerSource.clip = clips[random];
		playerSource.pitch = randomPitch;
		playerSource.Play ();
	}

	public void RandomizeFx (params AudioClip [] clips){

		int random = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (this.lowPitch, this.highPitch);

		fxSource.clip = clips[random];
		fxSource.pitch = randomPitch;
		fxSource.Play ();
	}

	public void changeMusic(int index){
		musicSource.Stop ();
		musicSource.clip = bgSceneMusic [index];
		musicSource.Play ();
	}

	public void stopMusic(){
		musicSource.Stop ();
	}
}
