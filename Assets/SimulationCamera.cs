using UnityEngine;
using System.Collections;
using System.Linq;

public class SimulationCamera : MonoBehaviour {

	public Rect windowRect; //= new Rect(0, 0, Display.main.systemWidth / 2, Display.main.systemHeight);
	public Camera simCamera;

	void OnGUI() {
		windowRect = GUI.Window(0, windowRect, UpdateWindow, "Simulation View");
	}

	void UpdateWindow(int windowID) {
		GUI.DrawTexture(windowRect, simCamera.targetTexture);
	}
	// Use this for initialization
	void Start () {
		//simCamera = Camera.allCameras.Where(c => c.name == "Simulation Camera").First();
		windowRect = new Rect(0, 0, Display.main.systemWidth / 2, Display.main.systemHeight);
		simCamera.targetTexture = new RenderTexture(Display.main.systemWidth / 2, Display.main.systemHeight, (int) simCamera.depth);
	}
	
	// Update is called once per frame
	void Update () {
		simCamera.Render();
	}
}
