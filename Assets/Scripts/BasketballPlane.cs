using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasketballPlane : MonoBehaviour {

    public CaVR cavr;
    public List<Rigidbody> basketballs;
    private List<Vector3> initPositions;

    // Use this for initialization
    void Start ()
    {
        initPositions = new List<Vector3>();

        foreach (var ball in basketballs)
        {
            initPositions.Add(new Vector3(ball.position.x, ball.position.y, ball.position.z));
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (cavr.InputManger.GetButtonValue("reset"))
        {
            for (int i = 0; i < basketballs.Count; i++)
            {
                Rigidbody ball = basketballs[i];
                ball.MovePosition(initPositions[i]);
                ball.velocity = Vector3.zero;
                ball.angularVelocity = Vector3.zero;
                ball.rotation = Quaternion.identity;
                ball.useGravity = true;
            }
        }
    }
}
