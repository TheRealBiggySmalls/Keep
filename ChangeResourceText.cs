using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeResourceText : MonoBehaviour {

	//updates the UI resources
	public static void UpdateUIResources(int food, int water){
		GameObject foodResource = GameObject.FindGameObjectWithTag("Food");
		GameObject waterResource = GameObject.FindGameObjectWithTag("Water");
		if(foodResource==null||waterResource==null){
			return;
		}
		foodResource.GetComponentInChildren<Text>().text = food.ToString();
		waterResource.GetComponentInChildren<Text>().text = water.ToString();
	}

	//CURRENTLY WORKS: - initially "updates" resource numbers at end of game start in map.
	//				   - additionally, updates resource numbers each time a new hex is set for the player.
	//				   - this ONLY applies to food and water as honey is not yet implemented
}
