﻿using UnityEngine;
using UnityEngine.UI;

/** Script for handling HUD and GUI elements 
 * Esteban.Hernandez
 */
public class HUDController : MonoBehaviour {

	public Text scoreText;
	public Text livesText;
	public Text multiplierText;
	public Slider multiplierBar;
	public Image multiplierFill;

	public bool inGameScene;
	public GameObject hiddenContainer;


	void Start(){
		if(inGameScene){
			multiplierBar.maxValue = GameManager.instance.maxMultipCount;
			GameManager.instance.assignHUDController(this);
		}
	}
	
	void Update () {
		if(inGameScene)
			updateBar();
	}


	private void updateBar(){
		int value = GameManager.instance.getMultiplierCount();
		multiplierBar.value = value;
		switch(value){
			case(0):multiplierFill.color = Color.white;break;
			case(1):multiplierFill.color = Color.blue;break;
			case(2):multiplierFill.color = Color.green;break;
			case(3):multiplierFill.color = Color.yellow;break;
			case(4):multiplierFill.color = new Color32 (229,78,51,255);break;
			case(5):multiplierFill.color = Color.red;break;
			default:break;	
		}
	}

	public void updateScore(int score){
		scoreText.text = score.ToString("D9");
	}

	public void updateLives(int lives){
		livesText.text = lives.ToString();
	}

	public void updateMultiplier(int multiplier){
		multiplierText.text = "x"+multiplier;
	}

	public void setHiddenPanelVisible(bool show){
		hiddenContainer.SetActive(show);
	}


}
