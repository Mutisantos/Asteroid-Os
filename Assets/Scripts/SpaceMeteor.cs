using UnityEngine;

public class SpaceMeteor : SpaceObject {

	public float speed;
	public float size;

	public int points;

	public int fragments;
	
	public SpaceMeteor[] smallerMeteors;

	public AudioClip meteorExplode;

	private Rigidbody2D meteorBody;
	void Awake () {
		meteorBody = GetComponent<Rigidbody2D>();
	}
	void Start(){
		
		Vector2 randomDir= new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
		meteorBody.AddForce(randomDir * speed);
		meteorBody.AddTorque(speed);
	}
	void FixedUpdate () {
		checkLimits();
	}

	private void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Shoot") {
			SpaceBullet bullet = coll.gameObject.GetComponent<SpaceBullet>();
			SpaceMeteor newMeteor;
			if(bullet.power > 1 && this.size > 2){
				for(int i = 1; i < fragments * 2; i++){
					newMeteor = Instantiate(smallerMeteors[1], transform.position, Quaternion.identity);
					newMeteor.transform.SetParent(this.transform.parent);
				}
				newMeteor = Instantiate(smallerMeteors[0], transform.position, Quaternion.identity);
				newMeteor.transform.SetParent(this.transform.parent);
			}
			else if (bullet.power == 1){
				for(int i = 0; i < fragments; i++){
					newMeteor = Instantiate(smallerMeteors[0], transform.position, Quaternion.identity);
					newMeteor.transform.SetParent(this.transform.parent);
				}				
			}
			SoundManager.instance.PlayMeteorOnce(meteorExplode);
			GameManager.instance.addScore(points);
			Destroy(gameObject);
		}
	}

}
