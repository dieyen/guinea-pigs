using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequiredItem : MonoBehaviour {
	
	// Use this for initialization
	void OnTriggerEnter (Collider other) {
		if (other.CompareTag("Player")){
			public bool QuestCompleted = true
			Destroy(RequiredItem)
		}	
	}


}
