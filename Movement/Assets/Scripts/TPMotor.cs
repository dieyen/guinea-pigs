using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPMotor : MonoBehaviour {

	public static TPMotor Instance;

	public float MoveSpeed = 10f;
	public float JumpSpeed = 6f;
	public float Gravity = 50f;
	public float TerminalVelocity = 60f;
	public float SlideThreshold = 0.6f;
	public float MaxControllableSlideMagnitude = 0.4f;

	private Vector3 slideDirection;


	public Vector3 MoveVector { get; set; }
	public float VerticalVelocity { get; set; }

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

		//Apply sliding if applicable
		ApplySlide();

		//Multiply MoveVector by MoveSpeed
		MoveVector *= MoveSpeed;

		//Reapply VerticalVelocity to MoveVector.y
		MoveVector = new Vector3 (MoveVector.x, VerticalVelocity, MoveVector.z);

		//Apply gravity
		ApplyGravity();

		//Move Character in world space
		TPController.CharacterController.Move (MoveVector * Time.deltaTime);
	}

	void ApplyGravity(){
		//Applies gravity to model

		if (MoveVector.y > -TerminalVelocity) {
			MoveVector = new Vector3 (MoveVector.x, MoveVector.y - Gravity * Time.deltaTime, MoveVector.z);
		}

		if (TPController.CharacterController.isGrounded && MoveVector.y < -1) {
			MoveVector = new Vector3 (MoveVector.x, -1, MoveVector.z);
		}
	}

	void ApplySlide(){
		//Applies slide movement to model

		if (!TPController.CharacterController.isGrounded) {
			return;
		}

		slideDirection = Vector3.zero;

		RaycastHit hitInfo;

		if (Physics.Raycast (transform.position + Vector3.up, Vector3.down, out hitInfo)) {
			if (hitInfo.normal.y < SlideThreshold) {
				slideDirection = new Vector3 (hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
			}
		}

		if (slideDirection.magnitude < MaxControllableSlideMagnitude) {
			MoveVector += slideDirection;
		} 

		else {
			MoveVector = slideDirection;
		}
	}

	public void Jump(){
		if (TPController.CharacterController.isGrounded) {
			VerticalVelocity = JumpSpeed;
		}
	}

	void SnapAlignCharacterWithCamera (){
		//This snaps the camer ato the character just behind it.
		if (MoveVector.x != 0 || MoveVector.z != 0) {
			transform.rotation = Quaternion.Euler (transform.eulerAngles.x, 
				Camera.main.transform.eulerAngles.y, 
				transform.eulerAngles.z);
		}
	}
}
