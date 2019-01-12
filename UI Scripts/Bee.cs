using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bee : MonoBehaviour {

	public string type;
	private bool activated=false;
	public int quantity;
	public GameObject beeObject;

	public bool updateVisuals(){
		if(quantity>=0){
			activated=true;
		}
		return activated;
	}

	//in here have it pass its texture
}
