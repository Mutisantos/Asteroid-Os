using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/** Script that makes transitions from one scene to another
 * Esteban.Hernandez
 */
public class SceneLoader : MonoBehaviour {

	void Start(){

	}
	public void myOwnLoadScene(int index){
		SceneManager.LoadScene (index);
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
