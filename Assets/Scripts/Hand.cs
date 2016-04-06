using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

    public CaVR cavr;
    public Basket basket;
    public float pushForce;
    public int forceValue;    

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
        //forceValue = -30000;
        isHold = false;
        deltaTime = 0;
    }

    void Update()
    {
        //FindObjectOfType<CaVR>();
        /*
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

        pos.y = 22 * ((pos.y-0.75f) / 1.01f);
        

        pos.x += -2.11f;
        //pos.y += 16.57f;
        pos.z += 9.25f;

        Debug.Log(pos);
        transform.localPosition = pos;
        transform.localRotation = sixdof.Rotation;
        */

        if (cavr.InputManger.GetButtonValue("B_button"))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Basketball")
                {
                    current_ball = hit.collider.GetComponent<Rigidbody>();
                    //hit.collider.GetComponent<Transform>().Translate(Camera.main.transform.position + (Camera.main.transform.forward * 5));
                    current_ball.MovePosition(Camera.main.transform.position + (Camera.main.transform.forward * 5));
                    current_ball.velocity = Vector3.zero;
                    current_ball.angularVelocity = Vector3.zero;
                    current_ball.rotation = Quaternion.identity;
                    current_ball.useGravity = false;
                }
            }
        }

        //if (Input.GetKeyDown(KeyCode.C) || inputManager.GetButtonValue("B_button"))
        if (Input.GetKeyDown(KeyCode.C) || isHold)
        {
            isHold = true;
            GetComponent<BoxCollider>().enabled = false;
            if(current_ball != null)
            {
                current_ball.MovePosition(transform.parent.GetComponent<Transform>().position);
                current_ball.velocity = Vector3.zero;
                current_ball.angularVelocity = Vector3.zero;
                current_ball.rotation = Quaternion.identity;
                current_ball.useGravity = false;
            }
            else
            {

            }            
        }


        if(cavr.InputManger.GetButtonValue("A_button"))
        {            
            Vector3 angles = Camera.main.transform.forward;
            Vector3 force = new Vector3(2.24f * angles.x, 10.24f * angles.y, 2.24f * angles.z);
            Debug.LogError("angle is: " + angles);
            current_ball.AddForce(force * pushForce, ForceMode.Impulse);
            current_ball.AddTorque(-Camera.main.transform.right * 100000000, ForceMode.Impulse);
            current_ball.useGravity = true;
            current_ball = null;
            //GetComponent<BoxCollider>().enabled = true;
            isHold = false;
            deltaTime += Time.deltaTime;
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
            force = new Vector3(0, forceValue, 0);
            /*
            if ((Mathf.Abs(currentEulerAngle.z % 360) < 90 || Mathf.Abs(currentEulerAngle.z % 360) > 270) && (Mathf.Abs(currentEulerAngle.x % 360) < 90 || Mathf.Abs(currentEulerAngle.x % 360) > 270))
            {
                force = new Vector3(0, forceValue, 0);
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
                force = new Vector3(forceX * forceValue * currentEulerAngle.z / 360, forceValue, forceZ * forceValue * currentEulerAngle.x / 360);
                Debug.Log(force);
            }
            */
            current_ball = other.GetComponent<Collider>().GetComponent<Rigidbody>();
            other.GetComponent<Collider>().GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }
}
