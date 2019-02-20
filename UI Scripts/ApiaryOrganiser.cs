using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.EventSystems;
using System;

[Serializable]

public class ApiaryOrganiser : MonoBehaviour {

	public GameObject apiaryWindow;
	private RawImage beeSlotOne, beeSlotTwo, resultOne,resultTwo,resultThree, honeyResultText;
	private Button resultOneButton, resultTwoButton, resultThreeButton;
	private GameObject beeResultOne,beeResultTwo,beeResultThree,toolTip;
	public List<GameObject> bees;
	private int breedTimer;
	private Breeding breed;
	private Text breedText; private Text apiaryDoneText;
	public Dictionary<string, bool> beeTruth;

	private readonly float xScale = 0.33f,yScale=0.55f;

	//this file works functionally but could use A LOT of optimisation
	void Start(){
		//initApiary();
		initVars();
		initBees();
		hideBeesOnTiles();
		HideApiaryWindow();
	}

	public int doTurn(){

		if(breedTimer==1){
			string[] results = breed.beeResults;
			int honeyResults = breed.honeyNumberResult;
			if(results.Length<1){
				return 0;
			}

			//IF TIMER IS DONE WE SHOULD GIVE AN ALERT TO USER THAT APIARY IS DONE: MAKE ONE MORE SPRITE WITH LITTLE ! AND CHANGE SPRITE

			//values in breed should be the same so i dont need to restore them
			Debug.Log("Results: " + results[0] + " " + results[1] + " " + results[2]);
			Debug.Log("HoneyResults: " + honeyResults);

			foreach(GameObject bee in bees){
				if(bee.name==results[0]){
					//maybe store gameobject here?
					resultOne.enabled=true;
					resultOneButton.enabled=true;
					resultOne.texture=bee.GetComponentInChildren<RawImage>().texture;
					beeResultOne=bee;
				}
				if(bee.name==results[1]){
					resultTwo.enabled=true;
					resultTwoButton.enabled=true;
					resultTwo.texture=bee.GetComponentInChildren<RawImage>().texture;
					beeResultTwo=bee;
				}
				if(bee.name==results[2]){
					resultThree.enabled=true;
					resultThreeButton.enabled = true;
					resultThree.texture=bee.GetComponentInChildren<RawImage>().texture;
					beeResultThree=bee;
				}
			}
			
			removeBeeSlot("one",0);
			removeBeeSlot("two",0);
			beeSlotOne.GetComponentInChildren<Button>().enabled=true;
			beeSlotTwo.GetComponentInChildren<Button>().enabled=true;

			//changes honey value and makes it active
			enableHoney();
			honeyResultText.GetComponentInChildren<Text>().text = honeyResults.ToString();

			breedTimerText(-100); //hard reset

			apiaryDoneText.enabled=true;

			return honeyResults;
		}else{
			breedTimerText(-1);
		}
		return 0;
	}

	//helper function to update breed timer on button
	public void breedTimerText(int timer){
		if(breedTimer+timer<0){
			breedTimer=0;
			breedText.text = "Start";
		}else{
			breedTimer+=timer;
			breedText.text = "Days: " + breedTimer.ToString();
		}
	}
	public void ShowApiaryWindow(){
		if(apiaryDoneText.enabled){
			apiaryDoneText.enabled=false;
		}
		apiaryWindow.SetActive(true);
	}


	//this will always be called first
	public void HideApiaryWindow(){
		disableHoney();
		toolTip.SetActive(false);
		apiaryWindow.SetActive(false);
	}

	public void disableHoney(){
		honeyResultText.GetComponentInChildren<Text>().enabled=false;
		honeyResultText.enabled=false;
	}

	public void enableHoney(){
		honeyResultText.enabled=true;
		honeyResultText.GetComponentInChildren<Text>().enabled=true;
	}

	public void StartBreeding(){
		if(breedTimer>0){
			return;
		}
		GameObject beeOne=null, beeTwo=null;
		foreach(GameObject bee in bees){
			if(bee.GetComponentInChildren<Bee>().name==beeSlotOne.texture.name){
				beeOne=bee;
			}
			if(bee.GetComponentInChildren<Bee>().name==beeSlotTwo.texture.name){
				beeTwo=bee;
			}
		}

		if(beeOne!=null && beeTwo !=null){
			breedTimer = breed.startBreeding(beeOne,beeTwo);
		}

		if(breedTimer==-100){ //for if the bees cant breed... return a mystery ???
			breedText.text="???";
			return;
		}
		breedTimerText(0);
		
		//so bees cannot be removed from apiary mid breeding
		beeSlotOne.GetComponentInChildren<Button>().enabled=false;
		beeSlotTwo.GetComponentInChildren<Button>().enabled=false;
	}

	public void AddBeeToBackpack(string clicked){
		//now remove graphic and unenable them again
		//add to quantity of bee with same texture
		if(clicked=="one"){
			Bee comp = beeResultOne.GetComponentInChildren<Bee>();
			Texture text = beeResultOne.GetComponentInChildren<RawImage>().texture;
			addToBeeQuantity(text,1);
			resultOne.enabled=false;
			resultOneButton.enabled=false;
			//HERE TURN OFF BUTTON
			//TODO: for infinibee bug - maybe enabled is not enough, might have to unset onclick listener
		}else if(clicked=="two"){
			Bee comp = beeResultTwo.GetComponentInChildren<Bee>();
			Texture text = beeResultTwo.GetComponentInChildren<RawImage>().texture;
			addToBeeQuantity(text,1);
			resultTwo.enabled=false;
			resultTwoButton.enabled=false;
		}else if(clicked=="three"){
			Bee comp = beeResultThree.GetComponentInChildren<Bee>();
			Texture text = beeResultThree.GetComponentInChildren<RawImage>().texture;
			addToBeeQuantity(text,1);
			resultThree.enabled=false;
			resultThreeButton.enabled=false;
		}
	}

	/*public void initApiary(){
		Image[] images = GameObject.Find("Canvas").GetComponentsInChildren<Image>();
		Debug.Log(images[16].name);
		apiaryWindow = images[16].gameObject;
	}*/
	
	//this is ugly as but the logic all chacks out
	public void initVars(){
		breedTimer=0;
		breed = apiaryWindow.GetComponentInChildren<Breeding>();
		Button[] temp = apiaryWindow.GetComponentsInChildren<Button>();
		breedText = temp[temp.Length-2].GetComponentInChildren<Text>();
		RawImage[] tiles = apiaryWindow.GetComponentsInChildren<RawImage>();

		int offset = bees.Count;
		beeSlotOne = tiles[offset + 1];
		beeSlotTwo = tiles[offset +3];
		resultOne= tiles[offset +4];
		resultOneButton = resultOne.GetComponentInChildren<Button>();
		resultTwo=tiles[offset +5];
		resultTwoButton = resultTwo.GetComponentInChildren<Button>();
		resultThree=tiles[offset +6];
		resultThreeButton = resultThree.GetComponentInChildren<Button>();
		honeyResultText = tiles[tiles.Length-1];
		Image[] gm = apiaryWindow.GetComponentsInChildren<Image>();
		toolTip = gm[gm.Length-1].gameObject;

		apiaryDoneText = GameObject.Find("Canvas").GetComponentInChildren<Button>().GetComponentInChildren<Text>();

		beeTruth = new Dictionary<string, bool>();
	}
	public void initBees(){
		foreach(GameObject bee in bees) { 
			bee.GetComponentInChildren<Button>().onClick.AddListener(delegate{addBeeToApaiary(bee);});
			
			Bee component = bee.GetComponentInChildren<Bee>();
			component.beeObject=bee;

			string name = bee.GetComponentInChildren<RawImage>().texture.name;
			if(name=="bland"){
				component.quantity=5;
				beeTruth.Add("bland",true);
			}else if(name=="common"){
				component.quantity=2;
				beeTruth.Add("common",true);
			}else{
				component.quantity=-1; //default value for bees that have not been found yet
				beeTruth.Add(name,false);
			}
			//component.quantity=5;
			//beeTruth.Add(name,true);

			component.initText();
			component.type=name;
			bee.name = name;

			component.setToolTip(toolTip); //all bees share the same object. 

			if(component.updateVisuals()){
				DisplayBee(bee, component);
			}else{
				bee.SetActive(false);
			}
		}
	}

	public void reInitBeeDict(){
		beeTruth.Clear();
		foreach(GameObject bee in bees){
			Bee component = bee.GetComponentInChildren<Bee>();
			if(component.quantity>0){ //IS TRUE IF QUANTITY IS > 0
				beeTruth.Add(component.name,true);
			}else{
				beeTruth.Add(component.name,false);
			}
		}
	}

	//code to update bee visuals. Can make this more efficient by passing in args
	public void DisplayBee(GameObject bee, Bee component){
		bee.GetComponentInChildren<Text>().text = component.quantity.ToString();
		bee.SetActive(true);
	}

	//only used for initialisation (???) might be an obsolete function
	public void hideBeesOnTiles(){
		apiaryDoneText.enabled=false;
		beeSlotOne.enabled =false;
		beeSlotTwo.enabled =false;
	}

	public void addBeeToApaiary(GameObject obj){

		Bee currentBee = obj.GetComponentInChildren<Bee>();

		if(currentBee.quantity>0){

			Texture texture = obj.GetComponentInChildren<RawImage>().texture;
			if(beeSlotOne.texture.name=="flowerBlue"){				
				beeSlotOne.GetComponentInChildren<BeeSlotScript>().addBee(beeSlotOne, texture);

			}else if(beeSlotTwo.texture.name=="flowerBlue"){			
				beeSlotTwo.GetComponentInChildren<BeeSlotScript>().addBee(beeSlotTwo, texture);

			}else{
				Debug.Log("Both slots full!");
				return;
			}

			//remove one from quantity and update the text
			addToBeeQuantity(obj.GetComponentInChildren<RawImage>().texture, -1);
			obj.GetComponentInChildren<Text>().text = currentBee.quantity.ToString(); //can probably use DisplayBee() here but its not super important

		}else{
			Debug.Log("insufficent bees of this type");
			return;
		}
	}

	//pretty sure these functions are some VERY bad/dubious programming but thata is okay it is your first project :^)
	public void removeBeeSlot(string slot){
		if(slot=="one"){
			BeeSlotScript script = beeSlotOne.GetComponentInChildren<BeeSlotScript>();
			script.removeBee(beeSlotOne);
			addToBeeQuantity(script.oldTexture, 1);
		}else if(slot=="two"){
			BeeSlotScript script = beeSlotTwo.GetComponentInChildren<BeeSlotScript>();
			script.removeBee(beeSlotTwo);
			addToBeeQuantity(script.oldTexture, 1);
		}
	}

	public void removeBeeSlot(string slot,int amount){
		if(slot=="one"){
			BeeSlotScript script = beeSlotOne.GetComponentInChildren<BeeSlotScript>();
			script.removeBee(beeSlotOne);
			addToBeeQuantity(script.oldTexture, amount);
		}else if(slot=="two"){
			BeeSlotScript script = beeSlotTwo.GetComponentInChildren<BeeSlotScript>();
			script.removeBee(beeSlotTwo);
			addToBeeQuantity(script.oldTexture, amount);
		}
	}

	//pass a negative to take away from quantity
	public void addToBeeQuantity(Texture texture, int amount){
		
		foreach(GameObject bee in bees){
			if(bee.GetComponentInChildren<RawImage>().texture==texture){
				Bee comp = bee.GetComponentInChildren<Bee>();
				int temp = comp.quantity;
				if(texture.name=="diligentWorker"||texture.name=="exoticWorker"){ //infinity exceptionss
					comp.quantity=1;
				}else if((temp+=amount)<0){ //if the amount will be less than 0
					comp.quantity=0;
				}else if(comp.quantity==-1&&amount>1){ //for if >1 bees are added the first time a bee is discovered
					comp.quantity+=(amount+1);
				}else if(comp.quantity==-1){ //if 1 is added the first time a bee is discovered
					comp.quantity=1;
				}else{ //otherwise just regularly add
					comp.quantity += amount;
				}
				DisplayBee(bee, comp);
			}
		}
	}
}
