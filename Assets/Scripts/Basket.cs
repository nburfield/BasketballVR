using UnityEngine;
using System.Collections;

public class Basket : MonoBehaviour {

    public GameObject score;
    public AudioClip basket;
    private bool firstHit;
    private Rigidbody selectedBall;

	// Use this for initialization
	void Start () {
        firstHit = false;
        selectedBall = null;
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
                        selectedBall.ResetCenterOfMass();
                        selectedBall.AddRelativeTorque(Vector3.zero);
                        selectedBall.useGravity = false;
                    }
                }
            }
            else
            {
                selectedBall.AddForce((Camera.main.transform.forward * 1000));
                selectedBall.useGravity = true;
                selectedBall = null;
            }
        }

        if(selectedBall != null)
        {
            selectedBall.MovePosition(Camera.main.transform.position + (Camera.main.transform.forward * 5));
        }
    }

    void OnTriggerEnter(Collider name)
    {
        if(name.transform.position.y > 29.5 && name.transform.position.y < 31.5)
        {
            firstHit = true;
        }
        if(name.transform.position.y > 26.5 && name.transform.position.y < 28.5 && firstHit)
        {
            firstHit = false;
            int cs = int.Parse(score.GetComponent<UnityEngine.UI.Text>().text) + 1;
            score.GetComponent<UnityEngine.UI.Text>().text = cs.ToString();
        }
    }
}
