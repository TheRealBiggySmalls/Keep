using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour {

	Vector3 oldPosition;
	Vector3 newPosition;
	//Vector3 currentVelocity;
	float smoothTime = 1f;

	void Start(){
		oldPosition = newPosition = this.transform.position;
	}
	public void OnCharacterMoved(Hex oldHex, Hex newHex) {
		//animate moving from one hex to the other
		oldPosition = oldHex.PositionFromCamera();
		newPosition = newHex.PositionFromCamera();

		if(Vector3.Distance(oldPosition, newPosition)>1.1f){
			//this is quite the hop. Illegal move
			//can also check neighbours to verify this
			Debug.Log("Can only move a single tile");
		}
	}

	void Update(){
		this.transform.position = Vector3.Lerp(this.transform.position, newPosition, smoothTime);
	}
	
}

