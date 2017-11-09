using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainInput;

/**Script for generating shoots */
public class Shooter : MonoBehaviour {

	public MainInputManager mainInput;
	public SpaceBullet weakShot;
	public SpaceBullet chargedShot;

	public AudioClip chargeSound;
	public AudioClip fullCharge;
	public AudioClip badCharge;
	

	private Transform rotDirector;

	private Animator anim;

	public bool isPlayer; 
	public float fireDelay = 0.3f;

	public float chargeThreshold = 0.5f;

	public float chargeShotDelay = 2f;
	private float cooldownTimer = 0f;
	private float chargeTime = 0f;

	private bool isFullyCharged;
	private bool isBadCharged;
	
	void Awake () {
		rotDirector = GetComponentsInChildren<Transform>()[0];
		anim = GetComponentsInChildren<Animator> ()[0];
		isFullyCharged = false;
		isBadCharged = false;
	}
	void FixedUpdate () {
		cooldownTimer -= Time.deltaTime;

		if(mainInput.button_ADown > chargeThreshold){
			chargeTime += Time.deltaTime;
			if(!SoundManager.instance.isPlayerPlaying())
				SoundManager.instance.PlayPlayerOnce(chargeSound);
			anim.SetBool("charging",true);
		}

		if(mainInput.button_AUp && cooldownTimer <= 0 && GameManager.instance.shotsLeft > 0){
			cooldownTimer = fireDelay;
			anim.SetBool("charging",false);
			fire();
			chargeTime = 0f;
		}

		if(mainInput.button_ADown < chargeThreshold){
			anim.SetBool("charging",false);
			chargeTime = 0f;
			isBadCharged = false;
			isFullyCharged = false;
			SoundManager.instance.stopPlayerSounds();
		}

		//Charge sound alerts
		if(chargeTime >= chargeShotDelay * 2 && !isBadCharged){
			SoundManager.instance.PlayOnce(badCharge);
			isBadCharged = true;
		}
		else if (chargeTime >= chargeShotDelay && !isFullyCharged){
			SoundManager.instance.PlayOnce(fullCharge);
			isFullyCharged = true;
		}	
	}

	private void fire(){

		Quaternion rot = rotDirector.transform.rotation;
		float z  = rot.eulerAngles.z;
		Quaternion shootDirection = Quaternion.Euler(0,0,z);
		//Badly Charged Shot
		if(chargeTime >= chargeShotDelay * 2){
			Instantiate(weakShot,transform.position,shootDirection);
			GameManager.instance.spendShot();
		}
		//Charged Shot
		else if(chargeTime >= chargeShotDelay){
			Instantiate(chargedShot,transform.position,shootDirection);
			GameManager.instance.spendShot();
		}
		//Weak Shot
		else{
			Instantiate(weakShot,transform.position,shootDirection);
			GameManager.instance.spendShot();
		}
	}

}
