using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPMotor : MonoBehaviour {

	public static TPMotor Instance;

	public float MoveSpeed = 10f;

	public Vector3 MoveVector { get; set; }

	void Awake () {
		Instance = this;
	}

	public void UpdateMotor () {
		SnapAlignCharacterWithCamera();
		ProcessMotion();

	}

	void ProcessMotion (){
		//Transform MoveVector to world space
		MoveVector = transform.TransformDirection(MoveVector);

		//Normalize MoveVector if Magnitude > 1
		if (MoveVector.magnitude > 1) {
			MoveVector = Vector3.Normalize (MoveVector);
		}

		//Multiply MoveVector by MoveSpeed
		MoveVector *= MoveSpeed;

		//Multiply new MoveVector by DeltaTime
		MoveVector *= Time.deltaTime;

		//Move Character in world space
		TPController.CharacterController.Move (MoveVector);
	}

	void SnapAlignCharacterWithCamera (){
		if (MoveVector.x != 0 || MoveVector.z != 0) {
			transform.rotation = Quaternion.Euler (transform.eulerAngles.x, 
				Camera.main.transform.eulerAngles.y, 
				transform.eulerAngles.z);
		}
	}
}
