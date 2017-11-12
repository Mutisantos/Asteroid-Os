using UnityEngine;

/** Abstract class that controls the movement and boundaries of the game objects (ships, shots, ufos, asteroids,etc) 
* Esteban.Hernandez
*/
public abstract class SpaceObject : MonoBehaviour {

	private const float threshold = 0.2f;

	void Start () {
		
	}
	
	protected void checkLimits(){
		//Camera viewport could be resized
        float vertExtent = BoundaryChecker.instance.VertExtent;
		float HorzExtent = BoundaryChecker.instance.HorzExtent;
		Vector2 newPosition = transform.position;
		if(transform.position.x > HorzExtent + threshold)//Right->Left
			newPosition.x = -HorzExtent;
		else if (transform.position.x < -HorzExtent - threshold)//Left->Right
			newPosition.x = HorzExtent;

		if(transform.position.y > vertExtent + threshold)//Up->Down
			newPosition.y = -vertExtent;
		else if (transform.position.y < -vertExtent - threshold)//Down->Up
			newPosition.y = vertExtent;
		
		if(newPosition.x != transform.position.x || newPosition.y != transform.position.y)
			transform.position = (newPosition);
		
			
	}
}
