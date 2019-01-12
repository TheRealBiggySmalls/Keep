﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPath;
using UnityEngine.UI;

public class Character {

	//RESOURCES. Honey should not be a flat resource, instead individual combs with different stats
	//Same as bees should be an array resouce, same as honey
	//Maybe trade for coins which can be used to buy things off merchant OR specific items
	//require specific rare honeycombs
	public int Food=50, Water=50, Honey=0;
	public string Name = "Character";
	public int movement = 1;
	public int movementRemaining = 1;

	public List<Bee> bees;


	public delegate void CharacterMovedDelegate (Hex oldHex, Hex newHex);
	public CharacterMovedDelegate OnCharacterMoved;

	/*
	//HAVE A VARIABLE THAT INCREASES YEILD CHANCE FROM TILES???
	public int hitPoints = 100;
	public int strength = 8;
	*/
	public Hex currentHex{get; protected set;}
	public Hex nextHex;
	
	public void initBees(){
		bees = new List<Bee>();
	}
	//store the move for the character and then do the things in here
	public void doTurn(){
		UpdateDayResources();

		if(nextHex==currentHex||nextHex==null){
			ChangeResourceText.UpdateUIResources(Food,Water,Honey);
			return;
		}
		//set all the variables and stuff when the click happens, then call all the actual moving and updating of stuff here!
		//TODO: make it more clear when the player has selected a tile
		//have to pass as negative as we are taking these away as movement cost
		UpdateResources(-nextHex.foodCost,-nextHex.waterCost);

		//updates and resets event costs
		UpdateResources(ScenarioManager.foodResult,ScenarioManager.waterResult,ScenarioManager.honeyResult);
		ScenarioManager.honeyResult=0;ScenarioManager.waterResult=0;ScenarioManager.foodResult=0;

		float rand = Random.Range(0f,10f);
		if(rand<3.33f){
			//happens a turn late but at least it happens
			ScenarioManager.ChooseEvent();
			//should update UI values
			//update the costs here -- DISPLAY EVENT???
		}

		ChangeResourceText.UpdateUIResources(Food,Water,Honey);
		//TODO: play walking animation
		SetHex(nextHex);
		unHighlightTile(nextHex.hexMap.hexToGameObjectMap[nextHex]);

	}

	public void UpdateDayResources(){
		UpdateResources(-5,-5);
	}

	public void SetHex(Hex newHex){
		//check for water tiles etc here for movement
		//can add whatever code you want in here for movement restriction
		Hex oldHex = currentHex;
		currentHex = newHex;

		//NOW: factor in resource costs. Should put somewhere AFTER move so that moving to a 
		//tile can happen if you wont have enough resources to move after move

		if(OnCharacterMoved != null){
			OnCharacterMoved(oldHex, newHex);
		}
	}

	private Color currentSelectionColour;
	private GameObject oldTile;
	public void moveToHex(Hex destinationTile){
		//this is pretty messy, ideally character would not have information of these things
		Map map = destinationTile.hexMap;
		GameObject tile = map.hexToGameObjectMap[destinationTile];

		//in each of these create a new alert if tile violates one of their rules
		if(!checkIllegalTiles(destinationTile)){
			AlertSection.NewAlert("Illegal Tile!","default",tile);
			return;
		}

		if(!checkTileInNeighbours(destinationTile, map)){
			AlertSection.NewAlert("Tile not in neighbours!","default",map.hexToGameObjectMap[destinationTile]);
			return;
		}

		if(!checkResourcesSufficient(destinationTile)){
			AlertSection.NewAlert("Resources insufficient for travel!","default",map.hexToGameObjectMap[destinationTile]);
			return;
		}

		if(oldTile==null){
			updateTileSelections(tile);
		}else{
			updateTileSelections(oldTile, tile);
		}

		nextHex = destinationTile;
	}

	public void updateTileSelections(GameObject oldie, GameObject newTile){

		//unset old tile to its original colour
		//WE ARE CHANGING THE COLOUR OF THE ORIGINAL MATERIAL. NEED TO CHANGE MATERIAL OR SOMETHING
		unHighlightTile(oldie);

		//set new tile to the selection colour
		updateTileSelections(newTile);
	}

	public void updateTileSelections(GameObject tile){
		MeshRenderer mf = tile.GetComponentInChildren<MeshRenderer>();
		currentSelectionColour = mf.material.color;
		Debug.Log("Colour " + currentSelectionColour);
		mf.material.color = Color.magenta;
		oldTile=tile;
	}

	public void unHighlightTile(GameObject tile){
		MeshRenderer mfOld = tile.GetComponentInChildren<MeshRenderer>();
		mfOld.material.color = currentSelectionColour;
		oldTile=null;
	}

	//checks tile is not illegal in any way
	public bool checkIllegalTiles(Hex hex){
		if(hex.Elevation<=0f){
			return false;
		}
		return true;
	}
	public bool checkTileInNeighbours(Hex hex, Map map){
		//need to write a new checkNeighbours function
		Hex[] hexes = map.getNeighbours(currentHex);
		foreach(Hex a in hexes){
			Debug.Log("a: " + map.hexToGameObjectMap[a]);
			Debug.Log("Hex: " + map.hexToGameObjectMap[hex]);
			//checks if positions match
			if(a.Position()==hex.Position()){
				return true;
			}
		}
		return false;
	}

	public bool checkResourcesSufficient(Hex hex){
		//TODO: write scripts to assign varying costs of food and water based on hex mesh and top mesh
		if(hex.foodCost<=Food&&hex.waterCost<=Water){
			return true;
		}
		return false;
	}

	public void UpdateResources(int food, int water, int honey=0){
		if(Food+food<0){
			Food=0;
		}else{
			Food+=food;
		}

		if(Water+water<0){
			Water = 0;
		}else{
			Water +=water;
		}

		if(Honey+honey<0){
			Honey=0;
		}else{
			Honey +=honey;
		}
	}

}
