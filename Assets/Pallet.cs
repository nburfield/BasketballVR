using UnityEngine;
using System.Collections;

public class Pallet : MonoBehaviour {

    private static readonly Vector3 UnitVector = new Vector3(1, 1, 1);
    private static readonly Vector3 ScaleVector = new Vector3(1.2f, 1.2f, 1.2f);

    private Painter painter;

	// Use this for initialization
	void Start() {
        painter = FindObjectOfType<Painter>();
	}
	
	// Update is called once per frame
	void Update() {
        var painterPos = painter.transform.position;

        if(Vector3.Distance(transform.position, painterPos) < 2.0f) {
            Transform closest = null;
            var distance = float.MaxValue;
            foreach(Transform colorSphere in transform) {
                var currentDistance = Vector3.Distance(colorSphere.position, painterPos);
                if(currentDistance < distance) {
                    closest = colorSphere;
                    distance = currentDistance;
                }
                else {
                    colorSphere.localScale = UnitVector;
                }
            }

            closest.localScale = ScaleVector;
        }

        else {
            foreach(Transform colorSphere in transform) {
                colorSphere.localScale = UnitVector;
            }
        }
	}
}
