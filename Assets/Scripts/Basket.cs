using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Basket : MonoBehaviour {

    public GameObject score;
    public AudioClip basket;
    private bool firstHit;
    private Rigidbody selectedBall;
    public List<Rigidbody> basketballs;
    private List<Vector3> initPositions;

	public Material lineMaterial;
	int maxPoints = 500;
	Rigidbody ballPrefab;
	float force = 16.0f;
	
	private int pathIndex = 0;
	private Vector3[] pathPoints;
	private bool running = false;
	private Transform thisTransform;

	// Use this for initialization
	void Start () {
        firstHit = false;
        selectedBall = null;
        initPositions = new List<Vector3>();

        foreach(var ball in basketballs)
        {
            initPositions.Add(new Vector3(ball.position.x, ball.position.y, ball.position.z));
        }

		pathPoints = new Vector3[maxPoints];
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedBall == null)
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Basketball")
                    {
                        selectedBall = hit.collider.GetComponent<Rigidbody>();
                        //hit.collider.GetComponent<Transform>().Translate(Camera.main.transform.position + (Camera.main.transform.forward * 5));
                        selectedBall.MovePosition(Camera.main.transform.position + (Camera.main.transform.forward * 5));
                        selectedBall.velocity = Vector3.zero;
                        selectedBall.angularVelocity = Vector3.zero;
                        selectedBall.rotation = Quaternion.identity;
                        selectedBall.useGravity = false;
                    }
                }
            }
            else
            {
                Vector3 angles = Camera.main.transform.forward;
                Vector3 force = new Vector3(2.24f * angles.x, 8.24f * angles.y, 2.24f * angles.z);
                
                selectedBall.AddForce(force*6000, ForceMode.Impulse);
                selectedBall.AddTorque(-Camera.main.transform.right * 100000000, ForceMode.Impulse);
                selectedBall.useGravity = true;
				thisTransform = Transform.Instantiate(selectedBall.transform);
				running = true;
                selectedBall = null;
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            for(int i = 0; i < basketballs.Count; i++)
            {
                Rigidbody ball = basketballs[i];
                ball.MovePosition(initPositions[i]);
                ball.velocity = Vector3.zero;
                ball.angularVelocity = Vector3.zero;
                ball.rotation = Quaternion.identity;
                ball.useGravity = true;
            }
        }

        if(selectedBall != null)
        {
            selectedBall.MovePosition(Camera.main.transform.position + (Camera.main.transform.forward * 5));
        }

		if(running)
		{
				pathPoints [pathIndex] = thisTransform.position;
				if (++pathIndex == maxPoints) {
					running = false;
				}
				// yield return new WaitForSeconds (0.05);
				Debug.LogError(thisTransform.position);
				UnityEditor.Handles.DrawBezier (thisTransform.position, 
				                                Vector3.zero, 
				                                Vector3.up, 
				                                -Vector3.up,
				                                Color.red, 
				                                null,
				                                10);

		}
    }

	void SamplePoints (Transform thisTransform) {

	}

    void OnTriggerEnter(Collider name)
    {
        Debug.LogError(name.transform.position);
        if(name.transform.position.y > 30.5 && name.transform.position.y < 32.5)
        {
            firstHit = true;
        }
        if(name.transform.position.y > 28.5 && name.transform.position.y < 30.5 && firstHit)
        {
            firstHit = false;
            int cs = int.Parse(score.GetComponent<UnityEngine.UI.Text>().text) + 1;
            score.GetComponent<UnityEngine.UI.Text>().text = cs.ToString();
        }
    }
}
