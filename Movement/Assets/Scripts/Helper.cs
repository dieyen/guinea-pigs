using System;
using UnityEngine;

public static class Helper{
	public static float ClampAngle(float angle, float min, float max){
		//remove unnecessary full rotations on mouse input
		//returns a clamped angle
		do {
			if (angle < -360) {
				angle += 360;

			}

			if (angle > 360) {
				angle -= 360;
			}
		} 
		while(angle < -360 || angle > 360);

		return Mathf.Clamp (angle, min, max);
	}
}