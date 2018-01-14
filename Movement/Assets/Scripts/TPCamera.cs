using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCamera : MonoBehaviour {
	//create global variables for instance of camera & transform 
	public static TPCamera Instance;
	public Transform TargetLookAt;
	public float Distance = 5f;
	public float DistanceMin = 3f;
	public float DistanceMax = 10f;
	public float DistanceSmooth = 0.05f;

	//sets mouse input sensitivity
	public float XMouseSensitivity = 5f;
	public float YMouseSensitivity = 5f;
	public float MouseWheelSensitivity = 5f;
	public float XSmooth = 0.05f;
	public float YSmooth = 0.1f;

	//sets limit of how low or high the mouse can hover along the Y-axis
	public float YMinLimit = -40f;
	public float YMaxLimit = 80f;

	//Sets the location of the mouse
	private float mouseX = 0f;
	private float mouseY = 0f;
	private float velX = 0f;
	private float velY = 0f;
	private float velZ = 0f;
	private float startDistance = 0f;
	private float desiredDistance = 0f;
	private float velDistance = 0f;
	private Vector3 desiredPosition = Vector3.zero;
	private Vector3 position = Vector3.zero;

	void Awake (){
		Instance = this;
	}

	void Start () {
		Distance = Mathf.Clamp (Distance, DistanceMin, DistanceMax);
		startDistance = Distance;
		Reset ();
	}

	void LateUpdate () {
		if (TargetLookAt == null) {
			return;
		}

		HandlePlayerInput ();
		CalculateDesiredPosition ();
		UpdatePosition ();
	}

	void HandlePlayerInput (){
		var deadZone = 0.01f;

		if (Input.GetMouseButton (1)) {
			//RMB is on hold, get mouse axis input
			mouseX += Input.GetAxis ("Mouse X") * XMouseSensitivity;
			mouseY -= Input.GetAxis ("Mouse Y") * YMouseSensitivity;
		}
		//Clamps mouse's Y-axis to set limits
		mouseY = Helper.ClampAngle (mouseY, YMinLimit, YMaxLimit);

		//Limit mouse rotation along Y-axis
		if (Input.GetAxis ("Mouse ScrollWheel") < -deadZone || Input.GetAxis ("Mouse ScrollWheel") > deadZone) {
			desiredDistance = Mathf.Clamp (Distance - Input.GetAxis ("Mouse ScrollWheel") * MouseWheelSensitivity, DistanceMin, DistanceMax);
		}

	}

	void CalculateDesiredPosition (){
		//Evaluate distance
		Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velDistance, DistanceSmooth);

		//Calculate desired position
		desiredPosition = CalculatePosition(mouseY, mouseX, Distance);
	}

	Vector3 CalculatePosition(float rotationX, float rotationY, float distance){
		Vector3 direction = new Vector3 (0, 0, -distance);
		Quaternion rotation = Quaternion.Euler (rotationX, rotationY, 0);

		return TargetLookAt.position + rotation * direction;
	}

	void UpdatePosition (){
		//Updates camera position when moving
		var positionX = Mathf.SmoothDamp (position.x, desiredPosition.x, ref velX, XSmooth);
		var positionY = Mathf.SmoothDamp (position.y, desiredPosition.y, ref velY, YSmooth);
		var positionZ = Mathf.SmoothDamp (position.z, desiredPosition.z, ref velZ, XSmooth);

		position = new Vector3 (positionX, positionY, positionZ);

		transform.position = position;

		transform.LookAt (TargetLookAt);
	}

	public void Reset (){
		//Resets the position of mouse to a set distance
		mouseX = 0;
		mouseY = 10;
		Distance = startDistance;
		desiredDistance = Distance;
	}
		
	public static void SpawnCamera (){
		//Creates a new or uses existing camera

		//Create game object variables for cameras
		GameObject tempCamera;
		GameObject targetLookAt;
		TPCamera myCamera;

		//Checks if the main camera exists
		if (Camera.main != null) {
			tempCamera = Camera.main.gameObject;
		} 

		else {
			tempCamera = new GameObject ("MainCamera");
			tempCamera.AddComponent<Camera> ();
			tempCamera.tag = "MainCamera";
		}

		//Make tempCamera to the main camera
		tempCamera.AddComponent<TPCamera>();
		myCamera = tempCamera.GetComponent ("TPCamera") as TPCamera;

		//Assigns targetLookAt to a game object with a targetLookAt tag
		targetLookAt = GameObject.Find ("targetLookAt") as GameObject;

		//Checks if targetLookAt's value is null and repositions it to the world origin
		if (targetLookAt == null) {
			targetLookAt = new GameObject ("targetLookAt");
			targetLookAt.transform.position = Vector3.zero;
		}

		//Repositions the Main Camera to the transform value of targetLookAt
		myCamera.TargetLookAt = targetLookAt.transform;
	}
}
