using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

    public CaVR cavr;
    public float throwForce;
	public float torqueForce;
    public int bounceForce;
	public GameObject forcetext;

    private Rigidbody rb;
    private Vector3 force;
    private bool isHold;
    private float deltaTime;
    private Vector3 currentEulerAngle;
    private VRPNInputManager inputManager;
    private Rigidbody current_ball = null;

    // test
    private float min;
    private float max;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        force = new Vector3(0, 0, 0);

        min = 1000f;
        max = -1000f;
        //bounceForce = -30000;
        isHold = false;
        deltaTime = 0;
		forcetext.GetComponent<UnityEngine.UI.Text>().text = throwForce.ToString();
    }

    void Update()
    {
        //FindObjectOfType<CaVR>();
        var sixdof = cavr.InputManger.GetSixdofValue("wand");
        var pos = sixdof.Position;

        //Debug.LogError("SixDOF Position: " + pos);

        min = Mathf.Min(min,pos.y);
        max = Mathf.Max(max, pos.y);

        if (Input.GetKey(KeyCode.M))
        {
            Debug.LogError("min is: " + min);
            Debug.LogError("max is: " + max);
        }

        pos.y = 15 * ((pos.y-0.75f) / 1.01f);
        

        //pos.x += -2.11f;
        pos.y += 10.45f;
        //pos.z += 9.25f;

        //Debug.LogError(pos);
		sixdof.Rotation.SetLookRotation (new Vector3(sixdof.Rotation.x + 90.0f, transform.localRotation.y, transform.localRotation.z));
		transform.localPosition = new Vector3(transform.localPosition.x, pos.y, transform.localPosition.z);
		//transform.localRotation = sixdof.Rotation;

		// Find a ball below
		if (cavr.InputManger.GetButtonValue ("B_button") && !isHold) {
			RaycastHit hit;

			GameObject lefthand = GameObject.Find ("RightCube");
			Vector3 positio = lefthand.transform.position;
			Vector3 dir = lefthand.transform.forward;

			if (Physics.Raycast (positio, dir, out hit, 20)) {
				if (hit.collider.tag == "Basketball") {
					current_ball = hit.collider.GetComponent<Rigidbody> ();

					current_ball.velocity = Vector3.zero;
					current_ball.angularVelocity = Vector3.zero;
					current_ball.rotation = Quaternion.identity;
					current_ball.useGravity = true;

					force = new Vector3 (0, bounceForce, 0);
					current_ball.AddForce (force, ForceMode.Impulse);
				}
			}
		} 
		else 
		{
			deltaTime += Time.deltaTime;
		}


        //if (Input.GetKeyDown(KeyCode.C) || inputManager.GetButtonValue("B_button"))
		if(cavr.InputManger.GetButtonValue("B_button") && isHold)
		{  
			Vector3 angles = transform.forward;
			Vector3 force = new Vector3(2.24f * angles.x, 10.24f * angles.y, 2.24f * angles.z);
			current_ball.AddForce(force * throwForce, ForceMode.Impulse);
			current_ball.AddTorque(-transform.right * torqueForce, ForceMode.Impulse);
			current_ball.useGravity = true;
			current_ball = null;
			//GetComponent<BoxCollider>().enabled = true;
			isHold = false;
			transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
		}

		if (cavr.InputManger.GetButtonValue("A_button") || isHold)
        {            
            if(current_ball != null)
            {
				isHold = true;
				GetComponent<BoxCollider>().enabled = false;
				GameObject righthand = GameObject.Find("RightCube");
				current_ball.MovePosition(righthand.transform.position + (righthand.transform.forward * 1.5f));
                current_ball.velocity = Vector3.zero;
                current_ball.angularVelocity = Vector3.zero;
                current_ball.rotation = Quaternion.identity;
                current_ball.useGravity = false;
				transform.localEulerAngles = new Vector3(-45.0f, 0.0f, 0.0f);
            }
            else
            {

            }            
        }

		if(cavr.InputManger.GetButtonValue("increase"))
		{
			throwForce += 1000;
			forcetext.GetComponent<UnityEngine.UI.Text>().text = throwForce.ToString();
		}

		if(cavr.InputManger.GetButtonValue("decrease"))
		{
			throwForce -= 1000;
			forcetext.GetComponent<UnityEngine.UI.Text>().text = throwForce.ToString();
		}

        if (deltaTime >= 1)
        {
            GetComponent<BoxCollider>().enabled = true;
            deltaTime = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO decide force value and angle
        // This part may cause angle wield bug
        if (other.gameObject.CompareTag("Basketball"))
        {
            currentEulerAngle = transform.rotation.eulerAngles;
			force = new Vector3(0, bounceForce, 0);

            /*
            if ((Mathf.Abs(currentEulerAngle.z % 360) < 90 || Mathf.Abs(currentEulerAngle.z % 360) > 270) && (Mathf.Abs(currentEulerAngle.x % 360) < 90 || Mathf.Abs(currentEulerAngle.x % 360) > 270))
            {
                force = new Vector3(0, bounceForce, 0);
                Debug.Log(force);
            }
            else
            {
                int forceZ;
                int forceX;
                if (currentEulerAngle.x > 180)
                {
                    forceZ = -1;
                }
                else
                {
                    forceZ = 1;
                }
                if (currentEulerAngle.z > 180)
                {
                    forceX = 1;
                }
                else
                {
                    forceX = -1;
                }
                force = new Vector3(forceX * bounceForce * currentEulerAngle.z / 360, bounceForce, forceZ * bounceForce * currentEulerAngle.x / 360);
                Debug.Log(force);
            }
            */
            current_ball = other.GetComponent<Collider>().GetComponent<Rigidbody>();
			current_ball.velocity = Vector3.zero;
			current_ball.angularVelocity = Vector3.zero;
			current_ball.rotation = Quaternion.identity;
			current_ball.useGravity = true;
            other.GetComponent<Collider>().GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }
}
