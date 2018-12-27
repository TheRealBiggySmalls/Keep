using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPath;

public class Character {

	//RESOURCES. Honey should not be a flat resource, instead individual combs with different stats
	//Same as bees should be an array resouce, same as honey
	//Maybe trade for coins which can be used to buy things off merchant OR specific items
	//require specific rare honeycombs
	public int Food=50, Water=50, Honey=0;
	public string Name = "Character";
	public int movement = 1;
	public int movementRemaining = 1;

	public delegate void CharacterMovedDelegate (Hex oldHex, Hex newHex);
	public CharacterMovedDelegate OnCharacterMoved;

	//We are implementing path finding even though we dont really need it
	Queue<Hex> hexPath;

	//TODO: this should probably be moved to some kind of central config
	const bool MOVEMENT_RULES_LIKE_CIV6 = false;

	/*
	//HAVE A VARIABLE THAT INCREASES YEILD CHANCE FROM TILES???
	public int hitPoints = 100;
	public int strength = 8;
	*/
	public Hex currentHex{get; protected set;}
	
	public void SetHex(Hex newHex){
		//check for water tiles etc here for movement
		//can add whatever code you want in here for movement restriction
		Hex oldHex = currentHex;
		currentHex = newHex;

		if(OnCharacterMoved != null){
			OnCharacterMoved(oldHex, newHex);
		}
	}

	public void SetHexPath(Hex[] path){
		this.hexPath = new Queue<Hex>(hexPath);
	}

	public void doTurn(){
		
		//movement doesnt currently work as hex queue is empty
		if(hexPath==null || hexPath.Count == 0){
			return;
		}	

		Hex newHex = hexPath.Dequeue();
		//Test moving one to the right.
		//TODO: write in rules for not allowing travel to water tiles (check elevation)
		//TODO: highlight tiles that can be moved to
		//TODO: link to mouse click

		//FIRST: check for illegal tiles (eg. water)
		//SECOND: check resources are sufficient for travel
		SetHex(newHex);
	}

	public void moveToHex(Hex destinationTile){
		//WE HAVE CURRENT HEX FOR sourceTile
		Debug.Log("ATTEMPT TO MOVE TO HEX:" + destinationTile);
		//in each of these create a new alert if tile violates one of their rules
		checkIllegalTiles(destinationTile);
		checkTileInNeighbours();
		checkResourcesSufficient();
	}

	//checks tile is not illegal in any way
	public bool checkIllegalTiles(Hex hex){
		if(hex.Elevation<=0f){
			AlertSection.NewAlert("Illegal Tile!","default",hex.hexMap.hexToGameObjectMap[hex]);
			return false;
		}
		return true;
	}
	public void checkTileInNeighbours(){

	}
	public void checkResourcesSufficient(){

	}
	public int MovementCostToEnterHex(Hex hex){
		if(hex.Elevation<0.01f){
			//0 is impossible, unable to enter
			return 0;
		}
		//TODO: Overrise base movement cost
		return hex.BaseMovementCost();
	}

	//a lot of this is for if we choose to allow pathfinding. At the moment
	//we only care about single neighbour tile based movement

	public float AggregateCostToEnterHex(Hex hex, float turnsToDate){
		//all these complex floats are probably unnecessary for out uses:
		//all we need to check is cost to enter based on our FOOD and WATER
		//numbers. Considering max movement is 1 we won't allow pathfinding
		//and so we can do single direct checks each time instead of this
		//complex aggregation work. 

		//TODO: Change logic to work off resources
		/*
			-get resource costs of travel
			-if current resource<cost{
				return false;
			}else{
				return true;
			}
		 */
		
		//we should return the number of turns the total move is going
		//to take

		/*if(Water<hex.waterCost){
			Debug.Log("Insufficient water for exploration!");
			return false;
		}else if(Food<hex.foodCost){
			Debug.Log("Insufficient water for exploration!");
			return false
		}else{
			return true;
		}*/





		float baseTurnsToEnterHex = MovementCostToEnterHex(hex)/movement;
		float turnsRemainging = movementRemaining/movement; //always 1 for us

		float turnsToDateWhole = Mathf.Floor(turnsToDate);
		float turnsToDateFraction = turnsToDate - turnsToDateWhole;

		if(turnsToDateFraction < 0.01f || turnsToDateFraction > 0.99f){
			Debug.Log("We have some floating point drift");
			//TODO: round?
			if(turnsToDateFraction<0.01f){
				turnsToDateFraction=0;
			}else if(turnsToDateFraction>0.99f){
				turnsToDateWhole+=1;
				turnsToDateFraction=0;
			}
		}

		float turnsUsedAfterThisMove = turnsToDateFraction + baseTurnsToEnterHex;

		if(turnsUsedAfterThisMove>1){
			//we have a situation where we dont actually have enough movement to continue
			if(MOVEMENT_RULES_LIKE_CIV6){
				//cant enter tile and have to sit idle for this turn
				if(turnsToDateFraction==0){
					//we have full movement but still cannot enter tile (because of resources)
				}else{
					turnsToDateWhole+=1;
					turnsToDateFraction=1;
				}

				//So now we know for a fact we are starting to move into difficult
				//terrain on a fresh turn
				turnsUsedAfterThisMove = baseTurnsToEnterHex;
				if(turnsUsedAfterThisMove>1){
					turnsUsedAfterThisMove=1;
				}
			}else{
				turnsUsedAfterThisMove=1;
			}
		}
		return turnsToDateWhole + turnsUsedAfterThisMove;
	}

	//cost to enter a hex. This function should still be useful for us.
	public float CostToEnterHex(Hex sourceTile, Hex destinationTile){
		return 1;
	}
}
