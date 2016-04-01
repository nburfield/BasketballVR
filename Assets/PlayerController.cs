using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private VRPNInputManager inputManager;
	// Use this for initialization
	void Start () {
		var cavr = FindObjectOfType<CaVR>();
		inputManager = cavr.InputManger;//FindObjectOfType<VRPNInputManager>();
	}
	
	// Update is called once per frame
	void Update () {
        var x = inputManager.GetAnalogValue("x");
        var z = inputManager.GetAnalogValue("z");

        if(x < 0.1f && x > -0.1f) {
            x = 0.0f;
        }

        if(z < 0.1f && z > -0.1f) {
            z = 0.0f;
        }

        var zButton = inputManager.GetButtonValue("rotate");
        var cButton = inputManager.GetButtonValue("boost");

        //Debug.Log(string.Format("C: {0}  Z: {1}", cButton, zButton));

        var boost = cButton ? 1.0f : 0.1f;
        var rotateBoost = cButton ? 3.0f : 1.0f;

        if(zButton) {
            //var rot = transform.rotation;

            transform.Rotate(0, x * rotateBoost, 0);

            //Debug.Log(rotY);
            //Debug.Log(rot.y + (float)x);
        }

        else {
            transform.Translate(Vector3.forward * (z * boost));
            transform.Translate(Vector3.right * (x * boost));
        }
        //Debug.Log(string.Format("X: {0}  Z: {1}", x, z));
    }
}
