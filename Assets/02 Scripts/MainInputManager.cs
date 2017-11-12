using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/** Script for handling inputs. Supports both standalone and mobile inputs
 * Esteban.Hernandez
 */
namespace MainInput{
	public class MainInputManager : MonoBehaviour {


		public float horizontal;
		public float vertical;
		public bool downDown;

		public bool button_AUp;
		public float button_ADown;
		public bool playable;

		//Mobile input variables
		[SerializeField]
		private bool button_UpDown = false;
		private bool button_DownDown = false;
		[SerializeField]
		private bool button_RightDown = false;
		[SerializeField]
		private bool button_LeftDown = false;

		private bool button_ShooterDown = false;
		public float axisChangePerSecond = 0.1f;

	void Start(){
		horizontal = 0;
		vertical = 0;
		button_ADown = 0;
	}

	public void toggleButtonUp (bool ispressed)
	{
			button_UpDown = ispressed;
	}
	public void toggleButtonDown (bool ispressed)
	{
			button_DownDown = ispressed;
	}

	public void toggleButtoRight (bool ispressed)
	{
			button_RightDown = ispressed;
	}
	public void toggleButtonLeft (bool ispressed)
	{
			button_LeftDown = ispressed;
	}

	public void toggleButtonShooter (bool ispressed)
	{
			button_ShooterDown = ispressed;
	}

	/** Standalone keyboard handle */
	void standaloneUpdate(){
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		button_ADown = Input.GetAxis ("A_Btn");
		button_AUp = Input.GetButtonUp ("A_Btn");
		downDown = Input.GetButtonDown ("Down_Btn");
	}

	/** Mobile touch handle */
	void mobileUpdate(){
		if(button_UpDown){
			if(vertical < 1)
					vertical += axisChangePerSecond;
		}
		else if(button_DownDown){
					downDown = true;
		}
		else{
			vertical = 0;
			downDown = false;
		}

		//Left - Right
		if(button_RightDown){
			if(horizontal < 1)
				horizontal += axisChangePerSecond;
		}
		else if(button_LeftDown){
			if(horizontal > -1)
				horizontal -= axisChangePerSecond;
			}
			else{
				horizontal = 0;
			}
		//Shooter
		if(button_ShooterDown){
			if(button_ADown < 1){
				button_ADown += axisChangePerSecond * 2;
				button_AUp = false;
			}
		}
		else{
			button_AUp = true;
			if(button_ADown > 0){
				button_ADown -= axisChangePerSecond * 2;
				button_AUp = true;
			}
		}
	}
	void Update () {
		#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL 
		standaloneUpdate();
		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE || UNITY_EDITOR	
		mobileUpdate();	
		#endif
		}
	}

}
