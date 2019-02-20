using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]

public class TurnOrganiser : MonoBehaviour {

	public Map map;
	public Character player;
	public GameObject boy;
	public GameObject turnCount;
	public GameObject apiary;
	private UniquesBackpack backpack;
	private npcController npc;
	public bool firstEncounter;

	public int turnNumber;

	void Start(){
		turnNumber=1;
		firstEncounter=true;
	}
	
	// Update is called once per frame
	public void NextTurn(){
		//it's in here as a safety net
		if(map==null||boy==null||player==null||turnCount==null||apiary==null||npc==null){
			map = GameObject.Find("HEX: 0, 0").GetComponentInChildren<HexComponent>().hexMap;
			boy = GameObject.FindGameObjectWithTag("Player");
			player = map.player;
			turnCount = GameObject.FindGameObjectWithTag("Turn");
			npc = GameObject.Find("Canvas").GetComponentInChildren<npcController>();

			apiary = npc.apiary;
			backpack =  GameObject.Find("Canvas").GetComponentInChildren<UniquesBackpack>();
		}
		apiary.GetComponentInChildren<ApiaryOrganiser>().reInitBeeDict();
		backpack.createDict();
		
		//have each of these things store their turn which is then called in here
		map.doTurn(); //at the moment map.doTurn() does nothing. Probably update fog of war or something in future
		int honeyUpdate = apiary.GetComponentInChildren<ApiaryOrganiser>().doTurn();
		apiary.GetComponentInChildren<ApiaryOrganiser>().reInitBeeDict(); //need before as may apply to character with scenarios

		player.UpdateResources(0,0,honeyUpdate);//put this into where the apiary is opened to make game smoother
		player.doTurn(turnNumber);

		//every third day the man comes. FOR NOW we treat it as a UI screen that pops up
		//happens after other updates so npc comes in front of events
		if(turnNumber%7==4){
			if(firstEncounter){
				npc.initNpc(firstEncounter);
				firstEncounter=false;
			}else{
				npc.openNpcScreen(false);
			}
		}

		apiary.GetComponentInChildren<ApiaryOrganiser>().reInitBeeDict();

		turnNumber += 1;
		turnCount.GetComponentInChildren<Text>().text = "Day: " + turnNumber;
		//what else do i need to do?
	}
}
