using UnityEngine;
using System.Collections;

using UnityStandardAssets.Vehicles.Car;

[RequireComponent(typeof(CarController))]
public class CarInput : MonoBehaviour {

    private CarController car;
    private CaVR cavr;
    // Use this for initialization
    void Start () {
        car = GetComponent<CarController>();
        cavr = FindObjectOfType<CaVR>();
	}
	

    void FixedUpdate() {
        var wheel = cavr.InputManger.GetSixdofValue("wheel");
        Debug.Log(wheel.Forward);
        var turn = -wheel.Forward.y;

        var accelerator = Input.GetKey(KeyCode.W) ? 1f : 0f;//cavr.InputManger.GetAnalogValue("accelerator");
        var decelerator = Input.GetKey(KeyCode.S) ? 1f : 0f;//Mathf.Max(-accelerator, 0f);

        car.Move(turn, accelerator, decelerator, 0f);
    }


}
