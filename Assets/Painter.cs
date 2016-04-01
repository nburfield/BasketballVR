using UnityEngine;
using System.Collections.Generic;

public class Painter : MonoBehaviour
{
	//private List<List<Vector3>> points = new List<List<Vector3>>();
	private Material lrMat;

	//private bool mouseDown = false;
	private List<Vector3> currentList = null;
	private LineRenderer currentLr;

	private List<GameObject> painters = new List<GameObject>();

    private VRPNInputManager inputManager;

    private bool drawing = false;

	// Use this for initialization
	void Start () {
		lrMat = new Material(Shader.Find("Particles/Additive"));

		var cavr = FindObjectOfType<CaVR>();
		inputManager = cavr.InputManger;
	}
	
	// Update is called once per frame
	void Update () {

        if(inputManager == null)
        {
            var cavr = FindObjectOfType<CaVR>();
            inputManager = cavr.InputManger;
        }
         var sixdof = inputManager.GetSixdofValue("wand");



        //Debug.Log(sixdof);
        var pos = sixdof.Position;

        //pos.y -= 1;
        //pos.z += 2;

        //pos.Scale(new Vector3(0.1f, 0.1f, 0.1f));
        //pos.y = -pos.y;
        Debug.Log(pos);
        
        transform.localPosition = pos;//new Vector3(sixdof.Position.x * 6.5f, (sixdof.Position.y - 2.0f) * 6.5f, (sixdof.Position.z - 2.0f) * -6.5f);

        transform.localRotation = sixdof.Rotation;//new Quaternion(-sixdof.Rotation.x, sixdof.Rotation.z, sixdof.Rotation.y, sixdof.Rotation.w);//new Quaternion(sixdof.Rotation.z, sixdof.Rotation.y, sixdof.Rotation.x, sixdof.Rotation.w));

        //transform.localPosition.z += 10;

        var paintButton = inputManager.GetButtonValue("paint");

		if(Input.GetMouseButtonDown(0) || (paintButton && !drawing)) {
			var obj = new GameObject();
			obj.transform.localPosition = transform.localPosition;
			obj.transform.parent = transform.parent;
			obj.transform.localScale = transform.localScale;
			obj.transform.localRotation = transform.localRotation;

			currentLr = obj.AddComponent<LineRenderer>();
			currentLr.material = lrMat;
			currentLr.SetColors(Color.green, Color.green);
			currentLr.SetWidth(0.1f, 0.1f);
			currentList = new List<Vector3>();

			painters.Add(obj);

            drawing = true;
		}

		if(Input.GetMouseButtonUp(0) || (!paintButton && drawing)) {
			currentList = null;
            drawing = false;
		}

		if(currentList != null) {
			currentList.Add(transform.position);

			currentLr.SetVertexCount(currentList.Count);
			currentLr.SetPositions(currentList.ToArray());
		}

		if(Input.GetKeyDown(KeyCode.C) || inputManager.GetButtonValue("clear")) {
			foreach(var obj in painters) {
				Destroy(obj);
			}

			painters.Clear();
		}
	}
}

public static class Matrix4x4Extensions
{
    public static Quaternion ToRotationQuaternion(this Matrix4x4 mat) {
        return Quaternion.LookRotation(mat.GetColumn(2), mat.GetColumn(1));
    }
}
