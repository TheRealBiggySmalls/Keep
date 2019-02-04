using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.EventSystems;

public class ApiaryOrganiser : MonoBehaviour {

	public GameObject apiaryWindow;
	private RawImage beeSlotOne, beeSlotTwo, resultOne,resultTwo,resultThree, honeyResultText;
	private Button resultOneButton, resultTwoButton, resultThreeButton;
	private GameObject beeResultOne,beeResultTwo,beeResultThree,toolTip;
	public List<GameObject> bees;
	private int breedTimer;
	private Breeding breed;
	private Text breedText;

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
		breedTimerText(0);
		
		//so bees cannot be removed from apiary mid breeding
		beeSlotOne.GetComponentInChildren<Button>().enabled=false;
		beeSlotTwo.GetComponentInChildren<Button>().enabled=false;
	}

	public void AddBeeToBackpack(string clicked){
		//now remove graphic and unenable them again
		//add to quantity of bee with same texture
		if(clicked=="one"){
			if(beeResultOne.GetComponentInChildren<Bee>().quantity<0){
				beeResultOne.GetComponentInChildren<Bee>().quantity=1;
			}else{
				beeResultOne.GetComponentInChildren<Bee>().quantity+=1;
			}
			DisplayBee(beeResultOne, beeResultOne.GetComponentInChildren<Bee>());
			resultOne.enabled=false;
			resultOneButton.enabled=false;
			//HERE TURN OFF BUTTON
			//TODO: for infinibee bug - maybe enabled is not enough, might have to unset onclick listener
		}else if(clicked=="two"){
			if(beeResultTwo.GetComponentInChildren<Bee>().quantity<0){
				beeResultTwo.GetComponentInChildren<Bee>().quantity=1;
			}else{
				beeResultTwo.GetComponentInChildren<Bee>().quantity+=1;
			}
			DisplayBee(beeResultTwo, beeResultTwo.GetComponentInChildren<Bee>());
			resultTwo.enabled=false;
			resultTwoButton.enabled=false;
		}else if(clicked=="three"){
			if(beeResultThree.GetComponentInChildren<Bee>().quantity<0){
				beeResultThree.GetComponentInChildren<Bee>().quantity=1;
			}else{
				beeResultThree.GetComponentInChildren<Bee>().quantity+=1;
			}
			DisplayBee(beeResultThree, beeResultThree.GetComponentInChildren<Bee>());
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
	}
	public void initBees(){
		foreach(GameObject bee in bees) { 
			bee.GetComponentInChildren<Button>().onClick.AddListener(delegate{addBeeToApaiary(bee);});
			
			Bee component = bee.GetComponentInChildren<Bee>();
			component.beeObject=bee;

			string name = bee.GetComponentInChildren<RawImage>().texture.name;
			if(name=="bland"){
				component.quantity=5;
			}else if(name=="common"){
				component.quantity=2;
			}else{
				component.quantity=-1; //default value for bees that have not been found yet
			}
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

	//code to update bee visuals. Can make this more efficient by passing in args
	public void DisplayBee(GameObject bee, Bee component){
		bee.GetComponentInChildren<Text>().text = component.quantity.ToString();
		bee.SetActive(true);
	}

	//only used for initialisation (???) might be an obsolete function
	public void hideBeesOnTiles(){
		beeSlotOne.enabled =false;
		beeSlotTwo.enabled =false;
		//LATER: when we enable this we just set the rawimage to whatever was clicked and reenable it
	}

	public void addBeeToApaiary(GameObject obj){
		//NEED TO FIND WHICH BEE WAS CLICKED
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
			currentBee.quantity -= 1; //just hardcoded here. Could use function below
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
				if((temp+=amount)<0){
					comp.quantity=0;
				}else{
					comp.quantity += amount;
				}
				DisplayBee(bee, comp);
			}
		}
	}
}
