using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Basket : MonoBehaviour {

    public GameObject score;
    public AudioSource audio;
    private bool firstHit;

	// Use this for initialization
	void Start ()
    {
        firstHit = false;        
    }
	
	// Update is called once per frame
	void Update ()
    {

    }


    void OnTriggerEnter(Collider name)
    {
        if(name.transform.position.y > 30.5 && name.transform.position.y < 32.5)
        {
            firstHit = true;
        }
        if(name.transform.position.y > 28.5 && name.transform.position.y < 30.5 && firstHit)
        {
            firstHit = false;
            int cs = int.Parse(score.GetComponent<UnityEngine.UI.Text>().text) + 1;
            score.GetComponent<UnityEngine.UI.Text>().text = cs.ToString();
            audio.Play();
        }
    }
}
