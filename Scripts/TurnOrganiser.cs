using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrganiser : MonoBehaviour {

	public Map map;
	public Character player;
	public GameObject boy;
	public GameObject turnCount;
	public GameObject apiary;
	public int turnNumber;

	void Start(){
		turnNumber=1;
	}
	
	// Update is called once per frame
	public void NextTurn(){
		//it's in here as a safety net
		if(map==null||boy==null||player==null||turnCount==null||apiary==null){
			map = GameObject.Find("HEX: 0, 0").GetComponentInChildren<HexComponent>().hexMap;
			boy = GameObject.FindGameObjectWithTag("Player");
			player = map.player;
			turnCount = GameObject.FindGameObjectWithTag("Turn");
			//LOGIC CHECKS OUT ITS JUST HIDDEN so manually assigning in editor
		}

		//have each of these things store their turn which is then called in here
		map.doTurn(); //at the moment map.doTurn() does nothing. Probably update fog of war or something in future
		player.doTurn();
		apiary.GetComponentInChildren<ApiaryOrganiser>().doTurn();

		//reupdates visuals: DOESNT WORK BECAUSE TURN IS QUEUED (the thingo isnt triggered until next turn)
		//ChangeResourceText.UpdateUIResources(player.Food,player.Water,player.Honey);

		turnNumber += 1;
		turnCount.GetComponentInChildren<Text>().text = "Day: " + turnNumber;
		//what else do i need to do?
	}
}
