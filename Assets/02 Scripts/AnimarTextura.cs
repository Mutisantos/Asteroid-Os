using UnityEngine;
using System.Collections;

/**Script for animating background with a tiled-like texture 
* Esteban.Hernandez
*/
public class AnimarTextura : MonoBehaviour {

	public Renderer rend;
	public Vector2 uvAnimationRate = new Vector2( 1.0f, 1.0f );
	public string textureName = "_MainTex";//Textura principal
	private Vector2 uvOffset = Vector2.zero;//Vector del offset

	void Start () {
		rend = GetComponent<Renderer>();
		rend.enabled = true;

	}
				
	void Update() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );//Move the texture offset while time advances
		if( rend.enabled )
		{
			rend.material.SetTextureOffset(textureName,uvOffset);//Change texture's offset every frame to simulate movement
		}
	}
}
