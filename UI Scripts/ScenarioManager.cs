using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioManager : MonoBehaviour {

	public static int foodResult, waterResult, honeyResult;

	private Character boi;
	public GameObject preFab;
	public GameObject closePrefab;
	private static string HeaderText, EventText, OptionOne, OptionTwo, CloseText;

	private static ScenarioManager instance;
	public int ScenarioId;

	//store a delegate function that stores the current outcome
	//OR can just have an if statement that stores the fun bois in there with a million elifs

	private static Vector3 defaultPos = Vector3.zero;

	void Start(){
		instance = this;
		HeaderText = "DEFAULT HEADER";
		EventText = "DEFAULT EVENT";
		OptionOne = "FIRST OPTION";
		OptionTwo = "SECOND OPTION";
		ScenarioId = 999;
	}

	public static void ChooseEvent(int id = 0){
		int rand;
		if(id==0){
			//Pick a random event!
			//change this to 30 later. Just using now so it doesnt break
			rand = (int) Random.Range(1,2.99f);
		}else{
			rand = id;
		}

		instance.yuckyIf(rand);
		instance.CreateEvent();
	}

	public void CreateEvent(){
		GameObject tileEvent  = (GameObject) Instantiate(instance.preFab);
		Text[] texts = tileEvent.GetComponentsInChildren<Text>();
		
		//sets header and event text
		if(texts[0].name=="HeaderText"){
			texts[0].text = HeaderText;
			texts[1].text = EventText;
		}else{
			texts[0].text = EventText;
			texts[1].text = HeaderText;
		}

		Button[] buttons = tileEvent.GetComponentsInChildren<Button>();
		if(buttons[0].name=="OptionOne"){
			buttons[0].GetComponentInChildren<Text>().text = OptionOne;
			buttons[1].GetComponentInChildren<Text>().text = OptionTwo;
		}else{
			buttons[0].GetComponentInChildren<Text>().text = OptionTwo;
			buttons[1].GetComponentInChildren<Text>().text = OptionOne;
		}
		tileEvent.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
		tileEvent.transform.localScale=Vector3.one;

		//Setting position to be correct
		defaultPos.y = Screen.height/2;
		defaultPos.x = Screen.width/2;
		tileEvent.transform.position = defaultPos;
	}

	public static void DisplayResults(int option){
		//TODO: fill out the rest of the scenarios
		//TODO: add in chance to the outcomes for FUN
		//TODO: add in results that update the users posessions
		instance.yuckyOutcomeIf(option);

		//creates the actual UI outcome
		GameObject outcome = (GameObject) Instantiate(instance.closePrefab);
		Text[] texts = outcome.GetComponentsInChildren<Text>();

		//sets header and event text
		if(texts[0].name=="HeaderText"){
			texts[0].text = HeaderText;
			texts[1].text = EventText;
		}else{
			texts[0].text = EventText;
			texts[1].text = HeaderText;
		}

		//sets resources gained text
		texts[2].text = waterResult.ToString();
		texts[3].text = foodResult.ToString();
		texts[4].text = honeyResult.ToString();
		
		//sets button text
		outcome.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = CloseText;


		outcome.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
		outcome.transform.localScale=Vector3.one;

		defaultPos.y = Screen.height/2;
		defaultPos.x = Screen.width/2;
		outcome.transform.position = defaultPos;

		//need to call update resource text here to make sure all is good
		//WILL BE FIXED WITH TURNS
	}





	//Here create a bunch of scenarios. Or maybe store them in a different file called Events

	//STORE ALL SCENARIOS IN HERE AS FUNCTIONS
	//FUNCTIONS SET IDS ETC
	//FUNCTION FOR SETTING TEXT AND SEPERATE ONE FOR OUTCOMES

	public void ScenarioOne(){
		ScenarioId=1;
		HeaderText = "What's that sound?";
		EventText = "You hear a strange buzzing in the distance, unlike that you've ever heard before...";
		OptionOne = "Investigate!";
		OptionTwo = "It's probably nothing.";
	}
	public void ScenarioOneOutcome(int outcome){
		if(outcome==1){
			honeyResult = Random.Range(3,13);
			//HERE: add in some randomness
			//TODO: add in icons for things gained etc. MAKE THIS VERY PRETTY IN THE FUTURE
			EventText = "You stumble across a wild hive filled with irregular bees and are able to harvest some of them!";
			CloseText = "Huzzah!";
			/*
			EventText = "You stumble across a wild hive filled with irregular bees but soon realize they are hyper aggressive! You are chased away and drop some of your supplies in the escape!";
			CloseText = "Oh no!"; 
			*/
		}else if(outcome==2){
			EventText = "You think it's best to leave whatever it is alone. Who knows what's in these woods!";
			CloseText = "Carry on.";
		}
	}

	public void ScenarioTwo(){
		ScenarioId=2;
		HeaderText = "Abandoned cottage ahead!";
		EventText = "Coming over a small hill you are confronted by a small, abandoned cottage that lies in your path. It looks to be in relatively good condition for how old it must be.";
		OptionOne = "Go inside!";
		OptionTwo = "Not worth exploring.";
	}
	public void ScenarioTwoOutcome(int outcome){
		if(outcome==1){
			waterResult = Random.Range(7,14);
			foodResult = Random.Range(2,6);
			EventText = "You enter the cottage and find a small number of well-preserved supplies!";
			CloseText = "Hooray!";
			/*
			EventText = "You enter the cottage but find the insides in ruins. Whatever wealth was once here is long gone.";
			CloseText = "Move on."; 
			*/
		}else if(outcome==2){
			EventText = "Whatever goods it once held will be long gone, no use checking inside.";
			CloseText = "Okay.";
		}
	}

	public void ScenarioThree(){

	}
	public void ScenarioThreeOutcome(int outcome){

	}

	public void ScenarioFour(){

	}
	public void ScenarioFourOutcome(int outcome){

	}

	public void ScenarioFive(){

	}
	public void ScenarioFiveOutcome(int outcome){

	}



	public void yuckyIf(int rand){
		if(rand==1){
			ScenarioOne();
		}else if(rand==2){
			ScenarioTwo();
		}else if(rand==3){
			ScenarioThree();
		}else if(rand==4){
			ScenarioFour();
		}else if(rand==5){
			ScenarioFive();
		}else if(rand==6){
			//ScenarioSix();
		}else if(rand==7){
			//ScenarioSeven();
		}else if(rand==8){
			//ScenarioEight();
		}else if(rand==9){
			//ScenarioNine();
		}else if(rand==10){
			//ScenarioTen();
		}else if(rand==11){
			//ScenarioEleven();
		}else if(rand==12){
			//ScenarioTwelve();
		}else if(rand==13){
			//ScenarioThirteen();
		}else if(rand==14){
			//ScenarioFourteen();
		}else if(rand==15){
			//ScenarioFifteen();
		}else if(rand==16){
			//ScenarioSixteen();
		}else if(rand==17){
			//ScenarioSeventeen();
		}else if(rand==18){
			//ScenarioEighteen();
		}else if(rand==19){
			//ScenarioNineteen();		
		}else if(rand==20){
			//ScenarioTwenty();
		}else if(rand==21){
			//ScenarioTwentyOne();
		}else if(rand==22){
			//ScenarioTwentyTwo();
		}else if(rand==23){
			//ScenarioTwentyThree();
		}else if(rand==24){
			//ScenarioTwentyFour();
		}else if(rand==25){
			//ScenarioTwentyFive();
		}else if(rand==26){
			//ScenarioTwentySix();
		}else if(rand==27){
			//ScenarioTwentySeven();
		}else if(rand==28){
			//ScenarioTwentyEight();
		}else if(rand==29){
			//ScenarioTwentyNine();		
		}else if(rand==30){
			//ScenarioThirty();
		}
	}

	public void yuckyOutcomeIf(int rand){
		if(ScenarioId==1){
			ScenarioOneOutcome(rand);
		}else if(ScenarioId==2){
			ScenarioTwoOutcome(rand);
		}else if(ScenarioId==3){
			ScenarioThreeOutcome(rand);
		}else if(ScenarioId==4){
			ScenarioFourOutcome(rand);
		}else if(ScenarioId==5){
			ScenarioFiveOutcome(rand);
		}else if(ScenarioId==6){
			//ScenarioSixOutcome(rand);
		}else if(ScenarioId==7){
			//ScenarioSevenOutcome(rand);
		}else if(ScenarioId==8){
			//ScenarioEightOutcome(rand);
		}else if(ScenarioId==9){
			//ScenarioNineOutcome(rand);
		}else if(ScenarioId==10){
			//ScenarioTenOutcome(rand);
		}else if(ScenarioId==11){
			//ScenarioElevenOutcome(rand);
		}else if(ScenarioId==12){
			//ScenarioTwelveOutcome(rand);
		}else if(ScenarioId==13){
			//ScenarioThirteenOutcome(rand);
		}else if(ScenarioId==14){
			//ScenarioFourteenOutcome(rand);
		}else if(ScenarioId==15){
			//ScenarioFifteenOutcome(rand);
		}else if(ScenarioId==16){
			//ScenarioSixteenOutcome(rand);
		}else if(ScenarioId==17){
			//ScenarioSeventeenOutcome(rand);
		}else if(ScenarioId==18){
			//ScenarioEighteenOutcome(rand);
		}else if(ScenarioId==19){
			//ScenarioNineteenOutcome(rand);		
		}else if(ScenarioId==20){
			//ScenarioTwentyOutcome(rand);
		}else if(ScenarioId==21){
			//ScenarioTwentyOneOutcome(rand);
		}else if(ScenarioId==22){
			//ScenarioTwentyTwoOutcome(rand);
		}else if(ScenarioId==23){
			//ScenarioTwentyThreeOutcome(rand);
		}else if(ScenarioId==24){
			//ScenarioTwentyFourOutcome(rand);
		}else if(ScenarioId==25){
			//ScenarioTwentyFiveOutcome(rand);
		}else if(ScenarioId==26){
			//ScenarioTwentySixOutcome(rand);
		}else if(ScenarioId==27){
			//ScenarioTwentySevenOutcome(rand);
		}else if(ScenarioId==28){
			//ScenarioTwentyEightOutcome(rand);
		}else if(ScenarioId==29){
			//ScenarioTwentyNineOutcome(rand);		
		}else if(ScenarioId==30){
			//ScenarioThirtyOutcome(rand);
		}
	}
}
