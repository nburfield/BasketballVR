using UnityEngine;
using System.Collections;

public class BasketballCamera : MonoBehaviour {

	public CaVR cavr;

	private bool playState;

	// Use this for initialization
	void Start () {
		playState = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!playState) {			
			if (cavr.InputManger.GetButtonValue ("A_button")) {
				RaycastHit hit;
				var ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.tag == "Start") {
						hit.collider.gameObject.SetActive (false);
						GetComponent<AudioSource> ().volume = 0.15f;
						GetComponent<Animator> ().enabled = false;
						transform.position = new Vector3 (0.0f, 17.8f, 0.0f);
						transform.localPosition = new Vector3 (0.0f, 17.8f, 0.0f);
						playState = true;
					}
				}
			}
		} 
		else 
		{
			
		}
	}
}
