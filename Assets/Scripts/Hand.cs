using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

    private Rigidbody rb;

    private Vector3 force;

    private Vector3 currentEulerAngle;

    public int forceValue;
    private VRPNInputManager inputManager;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        force = new Vector3(0, 0, 0);
        //forceValue = -30000;
    }

    void Update()
    {
        if (inputManager == null)
        {
            var cavr = FindObjectOfType<CaVR>();
            inputManager = cavr.InputManger;
        }
        var sixdof = inputManager.GetSixdofValue("wand");
        var pos = sixdof.Position;

        Debug.LogError("SixDOF Position: " + pos);

        pos.x += -2.11f;
        pos.y += 16.57f;
        pos.z += 9.25f;

        //Debug.Log(pos);

        transform.localPosition = pos;//new Vector3(sixdof.Position.x * 6.5f, (sixdof.Position.y - 2.0f) * 6.5f, (sixdof.Position.z - 2.0f) * -6.5f);

        transform.localRotation = sixdof.Rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO decide force value and angle
        // This part may cause angle wield bug
        if (other.gameObject.CompareTag("Basketball"))
        {
            currentEulerAngle = transform.rotation.eulerAngles;
            if ((Mathf.Abs(currentEulerAngle.z % 360) < 20 || Mathf.Abs(currentEulerAngle.z % 360) > 340) && (Mathf.Abs(currentEulerAngle.x % 360) < 20 || Mathf.Abs(currentEulerAngle.x % 360) > 340))
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

            other.GetComponent<Collider>().GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }
}
