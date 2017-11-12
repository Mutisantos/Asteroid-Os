using UnityEngine;
using System.Collections;


/** Singleton for controlling lives, score, score multipliers
* player status (alive/dead), shots able to make
* and updating the HUD 
* Esteban.Hernandez
*/
public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public AudioClip extraLifeClip;
	private HUDController hudController;

	private MeteorController meteorController;
	private Player player;
	public int maxLives = 3;
	private int lives = 0;
	public int shotsLeft = 5;
	public float multiplier = 1f;
	private int multiplierCount = 0;
	public int maxMultipCount = 5;
	[SerializeField]
	private int score = 0;
	private bool alive = true;
	private int level = 1;
	private Vector2 respawnPoint;
	private float scoreForLifeCounter;
	void Awake() {
		MakeSingleton ();
		lives = maxLives;
		scoreForLifeCounter = 4f;
		multiplierCount = 0;
	}

	private void MakeSingleton() {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	public void assignPlayer(Player player){	
		this.player = player;
	}
	public void assignMeteorController(MeteorController meteorController){	
		this.meteorController = meteorController;
	}

	public void assignHUDController(HUDController hudController){
		this.hudController = hudController;
		this.hudController.updateLives(this.lives);
		this.hudController.updateScore(this.score);		
	}

	public int getLives(){
		return lives;
	}

	public void addLives(int lives){
		this.lives += lives ;
		hudController.updateLives(this.lives);
		if(lives < 0){ //Dying resets the multiplier
			clearMultiplier();
		}
	}

	public int getScore(){
		return score;
	}
	//Increase or decrease score by points
	public void addScore(int points){
		score += (int) Mathf.Floor(points * multiplier);
		hudController.updateScore(score);
		if(points > 0)
			addMultiplier();
		if(score > Mathf.Pow(10f,scoreForLifeCounter)){//Add a life every power of 10 > 1000
			addLives(1);
			scoreForLifeCounter++;
			SoundManager.instance.PlayOnce(extraLifeClip);
		}
	}

	// Increases the multiplier every succesful shot 
	private void addMultiplier(){
		multiplierCount++;
		if(multiplierCount > maxMultipCount)
			multiplierCount = 0;
		multiplier += 0.25f;
		hudController.updateMultiplier((int)Mathf.Floor(multiplier));
	}

	// Resets the multiplier when the player dies or a shot misses
	public void clearMultiplier(){
		multiplierCount = 0;
		multiplier = 1;
		hudController.updateMultiplier(1);
	}

	public bool isAlive(){
		return this.alive;
	}

	public void setAlive(bool alive){
		this.alive = alive;
	}

	public Vector2 getRespawnPoint(){
		return this.respawnPoint;
	}

	public void setRespawnPoint(Vector2 respawnPoint){
		this.respawnPoint = respawnPoint;
	}

	public int getMultiplierCount(){
		return multiplierCount;
	}
	public void spendShot(){
		shotsLeft--;
	}

	public void addShots(){
		shotsLeft++;
	}

	public int getLevel(){
		return this.level;
	}

	public void addLevel(){
		level++;
	}

	public void endGame(){
		Time.timeScale = 0;
		hudController.setHiddenPanelVisible(true);//Show game over Screen
		SoundManager.instance.stopMusic ();
	}

	//Restarts the game from the first level
	public void restartGame(){
		resetValues();
		this.level = -1;
		clearMeteors();
		resetScene();
	}

	//Keeps playing from the current level
	public void continueGame(){
		resetValues();
		resetScene();
	}
	//Restarts game values 
	public void resetValues(){
		Time.timeScale = 1;				
		score = 0; 
		lives = maxLives; 
		alive = true;
		SoundManager.instance.changeMusic(0);
		multiplierCount = 0;
		multiplier = 1;
	}
	
	private void resetScene(){
		clearMultiplier();
		player.StopAllCoroutines();
		player.StartCoroutine (player.respawnPlayer ());
		hudController.updateLives(this.lives);
		hudController.updateScore(this.score);
		hudController.setHiddenPanelVisible(false);
	}

	//Destroys all the meteors present in the scene
	private void clearMeteors(){
		meteorController.clearMeteors();
	}

} 
