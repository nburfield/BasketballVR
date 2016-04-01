using UnityEngine;
using System.Collections;

public class Rim : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter() //if ball hits board
	{
		GetComponent<AudioSource>().Play(); //plays the hit board sound
	}
}
