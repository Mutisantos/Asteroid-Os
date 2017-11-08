using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainInput{
	public class MainInputManager : MonoBehaviour {


		public float horizontal;
		public float vertical;
		public bool downDown;
		public bool button_ADown;
		public bool playable;
		public bool pc; // Plataforma PC


		void Update () {
			if (playable && pc) {//Inputs de teclado
				horizontal = Input.GetAxisRaw ("Horizontal");
				vertical = Input.GetAxisRaw ("Vertical");
				button_ADown = Input.GetButtonDown ("A_Btn");
				downDown = Input.GetButtonDown ("Down_Btn");
			}
		}
	}
}
