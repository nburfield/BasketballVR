using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private VRPNInputManager inputManager;
    private Camera cam;
	// Use this for initialization
	void Start () {
		var cavr = FindObjectOfType<CaVR>();
		inputManager = cavr.InputManger;//FindObjectOfType<VRPNInputManager>();
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        var sixdof = inputManager.GetSixdofValue("view");

        transform.localPosition = sixdof.Position;
        transform.localRotation = sixdof.Rotation;

	}
}
