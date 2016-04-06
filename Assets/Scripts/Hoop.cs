using UnityEngine;
using System.Collections;

public class Hoop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    float Normalize(float value)
    {
        return value / 30.0f;
    }

    void OnCollisionEnter(Collision name)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = Normalize(name.relativeVelocity.magnitude);
        audio.Play();
    }
}
