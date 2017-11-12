using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainInput;

/**Script that enables the player to shoot
 * Esteban.Hernandez
 */
public class Shooter : MonoBehaviour {

	public MainInputManager mainInput;
	public SpaceBullet weakShot;
	public SpaceBullet chargedShot;
	public AudioClip chargeSound;
	public AudioClip fullCharge;
	public AudioClip badCharge;
	private Transform rotDirector;
	private Animator anim;
	public float fireDelay = 0.3f;
	//How much the Shoot Button axis must increase to know if it is a charged shot
	public float chargeThreshold = 0.5f;
	//How much time is needed to make a charged shot
	public float chargeShotDelay = 2f;
	//Shot Cooldown
	private float cooldownTimer = 0f;
	//Charge timer
	private float chargeTime = 0f;
	//Checks if the chargeTime is enough for the charged shot
	private bool isFullyCharged;
	//Checks if the chargeTime is to much for the charged shot
	private bool isBadCharged;
	
	void Awake () {
		rotDirector = GetComponentsInChildren<Transform>()[0];
		anim = GetComponentsInChildren<Animator> ()[0];
		isFullyCharged = false;
		isBadCharged = false;
	}
	void FixedUpdate () {
		playerFire();
	}

	private void playerFire(){
		cooldownTimer -= Time.deltaTime;
		//Charge Shot
		if(mainInput.button_ADown > chargeThreshold){
			chargeTime += Time.deltaTime;
			if(!SoundManager.instance.isPlayerPlaying())
				SoundManager.instance.PlayPlayerOnce(chargeSound);
			anim.SetBool("charging",true);
		}
		//Simple shot
		else if(mainInput.button_ADown > 0 && mainInput.button_ADown < chargeThreshold && cooldownTimer <= 0 && GameManager.instance.shotsLeft > 0){
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
			if(SoundManager.instance.isPlayerPlaying())
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
		//Return axis value to 0 if the shot button is released
		if(mainInput.button_AUp){
			mainInput.button_ADown = 0;
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
		//Simple Shot
		else{
			Instantiate(weakShot,transform.position,shootDirection);
			GameManager.instance.spendShot();
		}
	}

}
