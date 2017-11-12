using UnityEngine;

/** Singleton that check for game boundaries that could change or resize 
 * Esteban.Hernandez
 */
public class BoundaryChecker : MonoBehaviour {

	public static BoundaryChecker instance;
	public float VertExtent;
	public float HorzExtent;

	void Awake() {
		MakeSingleton ();
	}

	private void MakeSingleton() {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}
	void Start () {
		calculateBounds();
	}
	
	void Update () {
		calculateBounds();		
	}

	void calculateBounds(){
		VertExtent = Camera.main.orthographicSize;
		HorzExtent = (VertExtent * Screen.width / Screen.height);
	}
}
