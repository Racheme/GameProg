using UnityEngine;

public class Rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("left"))
			RotateLeft();
	}

	void RotateLeft () {
            transform.Rotate (Vector3.forward * -90);        

	}
}