using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour {

	protected float vertExtent;
	protected	float HorzExtent;

	private const float threshold = 0.2f;

	void Start () {
		
	}
	
	protected void checkLimits(){
		//Camera viewport could be resized
        vertExtent = Camera.main.orthographicSize;
		HorzExtent = (vertExtent * Screen.width / Screen.height);
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
