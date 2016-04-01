using UnityEngine;
using System.Collections;

public class Basketball : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision name)
	{
		if(name.collider.tag == "Plane")
		{
			GetComponent<AudioSource>().Play(); //plays the hit board sound
		}
	}
}
