using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPController : MonoBehaviour {

	public static CharacterController CharacterController;
	public static TPController Instance;

	void Awake () {
		CharacterController = GetComponent ("CharacterController") as CharacterController;
		Instance = this;
		TPCamera.SpawnCamera ();
	}

	void Update () {
		if (Camera.main == null) {
			return;
		}

		GetLocomotionInput ();

		TPMotor.Instance.UpdateMotor ();
	}

	void GetLocomotionInput (){
		var deadZone = 0.1f;

		TPMotor.Instance.MoveVector = Vector3.zero;

		if (Input.GetAxis ("Vertical") > deadZone || Input.GetAxis("Vertical")  < -deadZone) {
			TPMotor.Instance.MoveVector += new Vector3 (0, 0, Input.GetAxis ("Vertical"));
		}

		if (Input.GetAxis ("Horizontal") > deadZone || Input.GetAxis("Horizontal")  < -deadZone) {
			TPMotor.Instance.MoveVector += new Vector3 (Input.GetAxis ("Horizontal"), 0, 0);
		}
	}
}
