using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BasicColor : MonoBehaviour {

	public Color color;

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer>().sharedMaterial.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
