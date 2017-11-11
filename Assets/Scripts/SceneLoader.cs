using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	private GameObject loadingBar;
	private GameObject contMenu;

	void Start(){

	}

	public void myOwnLoadScene(int index){
		SceneManager.LoadScene (index);
		if(index > 0)
			GameManager.instance.resetValues();
	}


	public void restartLevel (){
		GameManager.instance.restartGame();
	}
	
	public void continueGame (){
		GameManager.instance.continueGame();
	}
	

	//QUIT-EXIT
	public void FinishGame(){
		Application.Quit ();
	}

}
