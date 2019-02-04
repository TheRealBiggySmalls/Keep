using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioManager : MonoBehaviour {

	public static int foodResult, waterResult, honeyResult, beeQuantity;
	public Texture defaultTexture;
	private Character boi;
	public GameObject preFab;
	public GameObject closePrefab;
	public GameObject beeClosePrefab;
	private static string HeaderText, EventText, OptionOne, OptionTwo, CloseText;

	public GameObject apiary; private UniquesBackpack backpack;

	private static ScenarioManager instance;
	public int ScenarioId;

	//store a delegate function that stores the current outcome
	//OR can just have an if statement that stores the fun bois in there with a million elifs

	private static Vector3 defaultPos = Vector3.zero;

	void Start(){	//so we only need to for loop once to find each items truth instead of having to iterate every time
		instance = this;
		HeaderText = "DEFAULT HEADER";
		EventText = "DEFAULT EVENT";
		OptionOne = "FIRST OPTION";
		OptionTwo = "SECOND OPTION";
		ScenarioId = 999;
		backpack = GameObject.Find("Canvas").GetComponentInChildren<UniquesBackpack>();
	}

	public static void ChooseEvent(int id = 0){
		int rand;
		if(id==0){
			//Pick a random event!
			//change this to 30 later. Just using now so it doesnt break
			rand = (int) Random.Range(1,19.99f);
		}else{
			rand = id;
		}

		instance.yuckyIf(rand);
		instance.CreateEvent();
	}

	public void CreateEvent(){
		GameObject tileEvent  = (GameObject) Instantiate(instance.preFab);
		Text[] texts = tileEvent.GetComponentsInChildren<Text>();

		//reinit dict every time
		backpack.createDict();
		
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
		Texture texture = instance.yuckyOutcomeIf(option);
		//HAVE THIS RETURN NULL if no bee OR a STRING NAME IF BEE
		if(texture==null){
			//creates the regular outcome for an event if no texture is returned
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
		}else{
			//creates the regular outcome for an event if no texture is returned
			GameObject outcome = (GameObject) Instantiate(instance.beeClosePrefab);
			Text[] texts = outcome.GetComponentsInChildren<Text>();

			//sets header and event text
			if(texts[0].name=="HeaderText"){
				texts[0].text = HeaderText;
				texts[1].text = EventText;
			}else{
				texts[0].text = EventText;
				texts[1].text = HeaderText;
			}

			//TODO: hide bee pedestal in UI OR set it to a question mark and hide text

			//sets bee texture and quantity
			RawImage bee = outcome.GetComponentsInChildren<RawImage>()[1];
			if(texture.name=="questionMark"){
				texts[2].enabled=false; //disable text
			}else{
				texts[2].text = beeQuantity.ToString();
			}
			
			bee.texture=texture;

			//sets button text
			outcome.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = CloseText;

			outcome.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
			outcome.transform.localScale=Vector3.one;

			defaultPos.y = Screen.height/2;
			defaultPos.x = Screen.width/2;
			outcome.transform.position = defaultPos;

			//IT ALL WORKS FUCK YEAH
			instance.apiary.GetComponentInChildren<ApiaryOrganiser>().addToBeeQuantity(texture, beeQuantity+1);
		}

	}





	//Here create a bunch of scenarios. Or maybe store them in a different file called Events

	//STORE ALL SCENARIOS IN HERE AS FUNCTIONS
	//FUNCTIONS SET IDS ETC
	//FUNCTION FOR SETTING TEXT AND SEPERATE ONE FOR OUTCOMES
	//Having certain high level bees unlocks new events (and makes old events not occur any more)

	public void ScenarioOne(){
		ScenarioId=1;
		HeaderText = "What's that sound?";
		EventText = "You hear a strange buzzing in the distance...";
		OptionOne = "Investigate!";
		OptionTwo = "It's probably nothing.";
	}
	public Texture ScenarioOneOutcome(int outcome){
		if(outcome==1){
			honeyResult = Random.Range(3,13);
			//HERE: add in some randomness
			EventText = "You stumble across a wild hive filled with bland bees and are able to harvest some of their honey!";
			CloseText = "Huzzah!";
			/*
			EventText = "You stumble across a wild hive filled with irregular bees but soon realize they are hyper aggressive! You are chased away and drop some of your supplies in the escape!";
			CloseText = "Oh no!"; 
			*/
		}else if(outcome==2){
			EventText = "You think it's best to leave whatever it is alone. Who knows what's in these woods!";
			CloseText = "Carry on.";
		}
		return null;
	}


	public void ScenarioTwo(){
		ScenarioId=2;
		HeaderText = "Abandoned cottage ahead!";
		EventText = "Coming over a small hill you are confronted by a small, abandoned cottage that lies in your path. It looks to be in relatively good condition for how old it must be.";
		OptionOne = "Go inside!";
		OptionTwo = "Not worth exploring.";
	}
	public Texture ScenarioTwoOutcome(int outcome){
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
		return null;
	}


	//make most of these about "you see a strange new hive in the woods/swamp/mountains etc" and the options give different ways to approach the bees
	//each with different success rates and depending on the bee types they are more difficult to pass or require different tactics etc.
	public void ScenarioThree(){
		ScenarioId=3;
		HeaderText = "Bees!";
		EventText = "You stumble across a bright red hive, glowing with anger and activity. You could surely use some of these on your farm. How do you approach retreiving them?";
		OptionOne = "Sneak up to them...";
		if(backpack.itemTruth["sword"]){
			OptionTwo = "Draw your shiny sword!";
		}else{
			OptionTwo = "Charge the nest!";
		}
	}
	public Texture ScenarioThreeOutcome(int outcome){
		Texture texture=defaultTexture;
		if(outcome==1){
			EventText = "Sneaking was the wrong move. The bees spotted you and attacked before you could get close enough to their hive. You were forced to run away!";
			CloseText = "To hell with them!";
			//dont set texture here it will still be null
		}else if(outcome==2&&backpack.itemTruth["sword"]){

			beeQuantity = Random.Range(1,5);
			EventText = "You made the right move - your shiny sword stunned the bees and froze them with fear! You successfully nabbed some before the hive could react!";
			CloseText = "Hooray!";
			foreach(GameObject text in apiary.GetComponentInChildren<ApiaryOrganiser>().bees){
				//saves me having to store it twice?
				if(text.GetComponentInChildren<RawImage>().texture.name=="warrior"){
					texture=text.GetComponentInChildren<RawImage>().texture;
					break;
				}
			}
			
		}else if(outcome==2){
			beeQuantity = Random.Range(1,5);
			EventText = "The bees were caught off guard for a second but soon reacted and chased you away. Something tells you this approach could work...";
			CloseText = "Hmm...";
		}
		return texture;
	}

	public void ScenarioFour(){
		ScenarioId=4;
		HeaderText = "Bees!";
		EventText = "You approach a cold area and see some bees busy at work. However, as you approach they quickly retreat and hide inside their nest.";
		OptionOne = "Try to take some";
		if(backpack.itemTruth["robot"]){
			OptionTwo = "Boot up Compliment-o-bot";
		}else{
			OptionTwo = "Shower them with compliments";
		}
	}

	public Texture ScenarioFourOutcome(int outcome){
		Texture texture=defaultTexture;//SET IT TO BE SOME DEFAULT TEXTURE HERE
		if(outcome==1){
			EventText = "You try to get some out of the hive but it is covered in ice and unyeilding! You walk away unsuccessful.";
			CloseText = "Next time!";
			//dont set texture here it will still be null
		}else if(outcome==2&&backpack.itemTruth["robot"]){

			beeQuantity = Random.Range(1,5);
			EventText = "As your trusty companion starts praising the hive and how fashionable its' inhabitants look, you notice a few bees creeping out to listen to you. Before long a few settle into your backpack and decide to follow you back to the farm!";
			CloseText = "Nice moves buddy!";
			foreach(GameObject text in apiary.GetComponentInChildren<ApiaryOrganiser>().bees){
				//saves me having to store it twice?
				if(text.GetComponentInChildren<RawImage>().texture.name=="icy"){
					texture=text.GetComponentInChildren<RawImage>().texture;	
					break;
				}
			}
		//not today
		}else if(outcome==2){
			EventText = "You stand there shouting nice words at the hive for some time. Every now and then you think you spot some activity but no bees come out of the hive.";
			CloseText = "Not this time.";
		}
		return texture;
	}


	public void ScenarioFive(){ //ROSE IS FOR FOREST BEES
		ScenarioId=5;
		HeaderText = "Bees!";
		EventText = "The forest ascends on all sides of you. It must go high enough to touch the sky! A sudden wave of tranquility overcomes you and you realise you are frozen in your path. A small swarm of deep green bees wait in front of you, expectantly...";
		OptionTwo = "What do you want?";
		if(backpack.itemTruth["rose"]){
			OptionOne = "Offer them a rose";
		}else{
			OptionOne = "Show them your bee collection";
		}
	}
	public Texture ScenarioFiveOutcome(int outcome){
		Texture texture=defaultTexture;
		if(outcome==1&&backpack.itemTruth["rose"]){
			EventText = "The bees float away and you try to follow them with your head but cant. Your muscles eventually loosen and you find yourself able to move again. Turning to follow your assialants you see them slowly floating away with your rose, and a few that have stayed behind with you.";
			CloseText = "New friends!";
			beeQuantity = Random.Range(1,5);
			foreach(GameObject text in apiary.GetComponentInChildren<ApiaryOrganiser>().bees){
				//saves me having to store it twice?
				if(text.GetComponentInChildren<RawImage>().texture.name=="forest"){
					texture=text.GetComponentInChildren<RawImage>().texture;	
					break;
				}
			}
		}else if(outcome==1){
			EventText = "The bees are not impressed and leave.";
			CloseText = "Come back!";
		}else if(outcome==2){
			EventText = "You black out and find yourself on the outskirts of the forest.";
			CloseText = "What happened?";
		}
		return texture;
	}


	public void ScenarioSix(){ //PICKAXE IS FOR STONE BEES
		ScenarioId=6;
		HeaderText = "Bees!";
		EventText = "You enter a small chasm with a large rock cliff face to your left, long drained of all its precious ores. After a few steps you soon notice a flat tone emminating from the wall itself.";
		OptionTwo = "Touch the stone";
		if(backpack.itemTruth["pickaxe"]){
			OptionOne = "Chip at it with the pickaxe";
		}else{
			OptionOne = "Punch the rock face";
		}
	}
	public Texture ScenarioSixOutcome(int outcome){
		Texture texture=defaultTexture;
		if(outcome==1&&backpack.itemTruth["pickaxe"]){
			EventText = "Lightly knocking into the rock loosens a large plate of bassalt which falls to the ground. Behind it a large swarm is revealed, barely distinguishable from the rock itself, and moving at a snails pace. They do not object when you remove a few.";
			CloseText = "These are very heavy!";
			beeQuantity = Random.Range(1,5);
			foreach(GameObject text in apiary.GetComponentInChildren<ApiaryOrganiser>().bees){
				//saves me having to store it twice?
				if(text.GetComponentInChildren<RawImage>().texture.name=="stone"){
					texture=text.GetComponentInChildren<RawImage>().texture;	
					break;
				}
			}
		}else if(outcome==1){
			EventText = "What did you expect?";
			CloseText = "Ouch";
		}else if(outcome==2){
			EventText = "As your palm touches the stone you feel a wave of relief overcome you. Clear vibrations are emminating from it.";
			CloseText = "I'll be back, rock.";
		}
		return texture;
	}


	public void ScenarioSeven(){ //BOAT IS FOR OCEAN BEES
		ScenarioId=7;
		HeaderText = "Bees!";
		EventText = "Wandering around the island you spot strange shapes oscillating around the waves";
		OptionTwo = "Swim out";
		if(backpack.itemTruth["boat"]){
			OptionOne = "Investigate with your dingy";
		}else{
			OptionOne = "Try to make out the shapes";
		}
	}
	public Texture ScenarioSevenOutcome(int outcome){
		Texture texture=defaultTexture;
		if(outcome==1&&backpack.itemTruth["boat"]){
			EventText = "Your rickety old boat calmly moors the tides as you row out. Looking around you barely can make out a bustling hive - filled with dark blue bees, hard to see amongst the depth of the ocean. You calmly collect a few and they dont seem to object as you take them back to shore with you.";
			CloseText = "Yeehaw!";
			beeQuantity = Random.Range(1,5);
			foreach(GameObject text in apiary.GetComponentInChildren<ApiaryOrganiser>().bees){
				//saves me having to store it twice?
				if(text.GetComponentInChildren<RawImage>().texture.name=="ocean"){
					texture=text.GetComponentInChildren<RawImage>().texture;	
					break;
				}
			}
			//dont set texture here it will still be null
		}else if(outcome==1){
			EventText = "Squinting you notice the figures dip in and out of the tides. You're fairly sure it's not just your imagination...";
			CloseText = "Hmm...";
		//not today
		}else if(outcome==2){
			EventText = "Stripping down too your togs and diving into the ocean you manage to brave the first few sets of waves but before long the gentle tide has carried you straight back to shore.";
			CloseText = "Blast!";
		}
		return texture;
	}

	public void ScenarioEight(){ //SHOES IS FOR OCEAN BEES
		ScenarioId=8;
		HeaderText = "Bees!";
		EventText = "From a nearby beach you can see unusual bees busy at work";
		OptionOne = "Call out the them";
		if(backpack.itemTruth["shoe"]){
			OptionTwo = "Adorn your sneak proof shoes";
		}else{
			OptionTwo = "Walk up to them";
		}
	}
	public Texture ScenarioEightOutcome(int outcome){
		Texture texture=defaultTexture;
		if(outcome==2&&backpack.itemTruth["shoe"]){
			EventText = "Your special shoes cancel out all sounds from the squeaky sand and you are able to reach the bees without startling them. they happily come with you when gathered.";
			CloseText = "Yeet!";
			beeQuantity = Random.Range(1,5);
			foreach(GameObject text in apiary.GetComponentInChildren<ApiaryOrganiser>().bees){
				//saves me having to store it twice?
				if(text.GetComponentInChildren<RawImage>().texture.name=="shore"){
					texture=text.GetComponentInChildren<RawImage>().texture;	
					break;
				}
			}
			//dont set texture here it will still be null
		}else if(outcome==2){
			EventText = "You start to approach them before realising this is no regular sand... its SQUEAKY sand! The bees are startled by the sound and disperse.";
			CloseText = "Hmm...";
		//not today
		}else if(outcome==1){
			EventText = "On hearing you they disperse, and even after waiting for some time they show no signs of returning.";
			CloseText = "Hmm...";
		}
		return texture;
	}



	//BOOK IS FOR EXTENDED SCENARIOS
	public void ScenarioNine(){
		ScenarioId=9;
		HeaderText = "Studious Work";
		EventText = "Reading from your book you decide to consult a new passage, what do you choose to study?";
		OptionTwo = "The Lurker";
		OptionOne = "The Sky Holder";
	}
	public Texture ScenarioNineOutcome(int outcome){

		if(outcome==2){
			EventText = "Forgotten tales tell of a sunken God who lays in a magical kingdom beneath the tides in slumber. Apparently he will wake one day and reclaim his treasured island, wherever that may be.";
			CloseText = "The world grows a little darker...";
		//not today
		}else if(outcome==1){
			EventText = "Once there stood a mighty giant who held the sky from the Earth. Long ago he perished and the sky fell, collapsing on the Earth and warping it into a sphere, the shape it has been ever since. His bones can still be found as roots in the deepest mountains.";
			CloseText = "Was that always there?...";
		}
		return null;
	}

	public void ScenarioTen(){
		ScenarioId=10;
		HeaderText = "Studious Work";
		EventText = "Walking under some palms you find some nice shade and open up your trusted book, what do you choose to study?";
		OptionTwo = "Forest Dwellers";
		OptionOne = "Warrior Breeds";
	}
	public Texture ScenarioTenOutcome(int outcome){

		if(outcome==2){
			EventText = "There is a curious species of bee living deep within the most lush forests. These intelligent creatures spread life wherever they go and are known for their base understanding of the forces that connect all things. While rare, encounters with them have been brief and emphasized their love of beauty.";
			CloseText = "Interesting.";
		}else if(outcome==1){
			EventText = "This bee is not one to be trifled with. Most dangerous from its genetic roots being anchored in industrious breeds. Many a bee keeper has accidentally produced this species before and paid the price. Can be managed in small quantites but beware over populating your hives with them.";
			CloseText = "Interesting.";
		}
		return null;
	}


	public void ScenarioEleven(){
		ScenarioId=11;
		HeaderText = "Studious Work";
		EventText = "A gentle creek lays in front of you and you decide it is the perfect time for some reading. What do you choose to study?";
		OptionTwo = "Warrior Breeds";
		OptionOne = "Oceanic Bees";
	}
	public Texture ScenarioElevenOutcome(int outcome){

		if(outcome==2){
			EventText = "This bee is not one to be trifled with. Most dangerous from its genetic roots being anchored in industrious breeds. Many a bee keeper has accidentally produced this species before and paid the price. Can be managed in small quantites but beware over populating your hives with them.";
			CloseText = "Interesting.";
		}else if(outcome==1){
			EventText = "Amphibious bees with a preference for aquatic dwelling have been found on many occasions by unwary sailors. Just how they produce their honey is unknown, but they seem to have no trouble adapting from their coral hives to those of regular land bees. Strange mutations have been observed when such bees are confined to land for too long of a time.";
			CloseText = "Interesting.";
		}
		return null;
	}

	public void ScenarioTwelve(){
		ScenarioId=12;
		HeaderText = "Studious Work";
		EventText = "A dreamy canope of trees shields your current path from the morning sun. What better time to read? What do you choose to study?";
		OptionTwo = "Oceanic Bees";
		OptionOne = "Stone and Earth";
	}
	public Texture ScenarioTwelveOutcome(int outcome){

		if(outcome==2){
			EventText = "Amphibious bees with a preference for aquatic dwelling have been found on many occasions by unwary sailors. Just how they produce their honey is unknown, but they seem to have no trouble adapting from their coral hives to those of regular land bees. Strange mutations have been observed when such bees are confined to land for too long of a time.";
			CloseText = "Interesting";
		}else if(outcome==1){
			EventText = "Certain bees have been found residing inside of rocks and ore veins. Generally little is known of these bees save for their increibly slow speed and dense nature. Many a miner has been saved by this fact.";
			CloseText = "Interesting.";
		}
		return null;
	}


	public void ScenarioThirteen(){
		ScenarioId=13;
		HeaderText = "Studious Work";
		EventText = "The chilly morning breeze is too much to handle as you duck inside of a cave and whip out your trusty compendium. What do you choose to study?";
		OptionTwo = "Stone and Earth";
		OptionOne = "Beachs and Lakes";
	}
	public Texture ScenarioThirteenOutcome(int outcome){

		if(outcome==2){
			EventText = "Certain bees have been found residing inside of rocks and ore veins. Generally little is known of these bees save for their increibly slow speed and dense nature. Many a miner has been saved by this fact.";
			CloseText = "Interesting.";
		}else if(outcome==1){
			EventText = "A specific breed of bee has found its way to the lakes and beachs of the world. Common enough but easily startled, this bee is reknowned for the sheer solemnness of its hum. It is enough to bring any seasoned bee keeper to tears.";
			CloseText = "Interesting.";
		}
		return null;
	}

	public void ScenarioFourteen(){
		ScenarioId=14;
		HeaderText = "Studious Work";
		EventText = "As dawn cracks you decide there is no better time for learning than now. What do you choose to study?";
		OptionTwo = "Beaches and Lakes";
		OptionOne = "Frozen Tundras";
	}
	public Texture ScenarioFourteenOutcome(int outcome){

		if(outcome==2){
			EventText = "A specific breed of bee has found its way to the lakes and beachs of the world. Common enough but easily startled, this bee is reknowned for the sheer solemnness of its hum. It is enough to bring any seasoned bee keeper to tears.";
			CloseText = "Interesting.";
		}else if(outcome==1){
			EventText = "Rare and scattered encounters speak of remarkable bees which have the ability to survive in the coldest parts of the world. The arctic winds may have bleached the colour from their skin, but their temperament is legendary. If treated gently, these bees are bonified factory for honey.";
			CloseText = "Interesting.";
		}
		return null;
	}

	public void ScenarioFifteen(){
		ScenarioId=15;
		HeaderText = "Studious Work";
		EventText = "Your fire crackles in the silent night and you pull out your book before bed. What do you choose to study?";
		OptionTwo = "Frozen Tundras";
		OptionOne = "Forest Dwellers";
	}
	public Texture ScenarioFifteenOutcome(int outcome){

		if(outcome==2){
			EventText = "Rare and scattered encounters speak of remarkable bees which have the ability to survive in the coldest parts of the world. The arctic winds may have bleached the colour from their skin, but their temperament is legendary. If treated gently, these bees are bonified factory for honey.";
			CloseText = "Interesting.";
		}else if(outcome==1){
			EventText = "There is a curious species of bee living deep within the most lush forests. These intelligent creatures spread life wherever they go and are known for their base understanding of the forces that connect all things. While rare, encounters with them have been brief and emphasized their love of beauty.";
			CloseText = "Interesting.";
		}
		return null;
	}

	public void ScenarioSixteen(){
		ScenarioId=16;
		HeaderText = "Studious Work";
		EventText = "You decide to consult your book for information. What do you choose to study?";
		OptionTwo = "Mutations";
		OptionOne = "Protection";
	}
	public Texture ScenarioSixteenOutcome(int outcome){

		if(outcome==2){
			EventText = "Cross breeding is an interesting and crucial part of an bee keepers life. While it has the potential to create new and exciting variations of bees, it also has the potential for undesirable outcomes. Much thought should be put into cross breeding and which bees may have favourable genetic mutations.";
			CloseText = "Interesting.";
		}else if(outcome==1){
			EventText = "Ensuring the proper precations are taken before cross breeding is essential. Too many tales exist of farmers being melted, vaporized or even having their whole farm reduced to ruins by lack of preparation. Never underestimate nature, and the cost of messing with it.";
			CloseText = "Interesting.";
		}
		return null;
	}


	public void ScenarioSeventeen(){
		ScenarioId=17;
		HeaderText = "Studious Work";
		EventText = "A proper bee-keeper values knowledge above all else. What do you choose to study?";
		OptionTwo = "Protection";
		OptionOne = "Intelligence";
	}
	public Texture ScenarioSeventeenOutcome(int outcome){

		if(outcome==2){
			EventText = "Ensuring the proper precations are taken before cross breeding is essential. Too many tales exist of farmers being melted, vaporized or even having their whole farm reduced to ruins by lack of preparation. Never underestimate nature, and the cost of messing with it.";
			CloseText = "Interesting.";
		}else if(outcome==1){
			EventText = "Intelligence is an interesting trait for bees. On one hand it is incredibly desireable as it increases relations between the keeper and his hive, as well as increasing hive output. That being said, a surplus of intelligence can be very dangerous... beware the hive mind.";
			CloseText = "Interesting.";
		}
		return null;
	}

	public void ScenarioEighteen(){
		ScenarioId=18;
		HeaderText = "Studious Work";
		EventText = "The rain is pooring down outside and despite your efforts, you know nothing will get done today; and so you turn to your books. What do you choose to study?";
		OptionTwo = "Intelligence";
		OptionOne = "Family Trees";
	}
	public Texture ScenarioEighteenOutcome(int outcome){

		if(outcome==2){
			EventText = "Intelligence is an interesting trait for bees. On one hand it is incredibly desireable as it increases relations between the keeper and his hive, as well as increasing hive output. That being said, a surplus of intelligence can be very dangerous... beware the hive mind.";
			CloseText = "Interesting.";
		}else if(outcome==1){
			EventText = "Bee genetics can be loosely split into two categories: the Industrious/Natural tree and the Shy/Unnatural tree. Both have distinct genetic lines and safe mutations within them, though breeding between the trees is considered taboo and is an area lacking research.";
			CloseText = "Interesting.";
		}
		return null;
	}

	public void ScenarioNineteen(){
		ScenarioId=19;
		HeaderText = "Studious Work";
		EventText = "Book time! What do you choose to study?";
		OptionTwo = "Family Trees";
		OptionOne = "Mutations";
	}
	public Texture ScenarioNineteenOutcome(int outcome){

		if(outcome==2){
			EventText = "Bee genetics can be loosely split into two categories: the Industrious/Natural tree and the Shy/Unnatural tree. Both have distinct genetic lines and safe mutations within them, though breeding between the trees is considered taboo and is an area lacking research.";
			CloseText = "Interesting.";
		}else if(outcome==1){
			EventText = "Cross breeding is an interesting and crucial part of an bee keepers life. While it has the potential to create new and exciting variations of bees, it also has the potential for undesirable outcomes. Much thought should be put into cross breeding and which bees may have favourable genetic mutations.";
			CloseText = "Interesting.";
		}
		return null;
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
			ScenarioSix();
		}else if(rand==7){
			ScenarioSeven();
		}else if(rand==8){
			ScenarioEight();
		
		//BOOK SCENARIOS: LORE ABOUT BEES. ALL HAVE THE SAME STARTING PASSAGE NEARLY
		}else if(rand==9&&backpack.itemTruth["book"]){
			ScenarioNine();
		}else if(rand==10&&backpack.itemTruth["book"]){
			ScenarioTen();
		}else if(rand==11&&backpack.itemTruth["book"]){
			ScenarioEleven();
		}else if(rand==12&&backpack.itemTruth["book"]){
			ScenarioTwelve();
		}else if(rand==13&&backpack.itemTruth["book"]){
			ScenarioThirteen();
		}else if(rand==14&&backpack.itemTruth["book"]){
			ScenarioFourteen();
		}else if(rand==15&&backpack.itemTruth["book"]){
			ScenarioFifteen();
		}else if(rand==16&&backpack.itemTruth["book"]){
			ScenarioSixteen();
		}else if(rand==17&&backpack.itemTruth["book"]){
			ScenarioSeventeen();
		}else if(rand==18&&backpack.itemTruth["book"]){
			ScenarioEighteen();
		}else if(rand==19&&backpack.itemTruth["book"]){
			ScenarioNineteen();		
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
		}else{
			rand = Random.Range(0,15);
			yuckyIf(rand);
		}
	}

	public Texture yuckyOutcomeIf(int rand){
		if(ScenarioId==1){
			return ScenarioOneOutcome(rand);
		}else if(ScenarioId==2){
			return ScenarioTwoOutcome(rand);
		}else if(ScenarioId==3){
			return ScenarioThreeOutcome(rand);
		}else if(ScenarioId==4){
			return ScenarioFourOutcome(rand);
		}else if(ScenarioId==5){
			return ScenarioFiveOutcome(rand);
		}else if(ScenarioId==6){
			return ScenarioSixOutcome(rand);
		}else if(ScenarioId==7){
			return ScenarioSevenOutcome(rand);
		}else if(ScenarioId==8){
			return ScenarioEightOutcome(rand);
		}else if(ScenarioId==9){
			return ScenarioNineOutcome(rand);
		}else if(ScenarioId==10){
			return ScenarioTenOutcome(rand);
		}else if(ScenarioId==11){
			return ScenarioElevenOutcome(rand);
		}else if(ScenarioId==12){
			return ScenarioTwelveOutcome(rand);
		}else if(ScenarioId==13){
			return ScenarioThirteenOutcome(rand);
		}else if(ScenarioId==14){
			return ScenarioFourteenOutcome(rand);
		}else if(ScenarioId==15){
			return ScenarioFifteenOutcome(rand);
		}else if(ScenarioId==16){
			return ScenarioSixteenOutcome(rand);
		}else if(ScenarioId==17){
			return ScenarioSeventeenOutcome(rand);
		}else if(ScenarioId==18){
			return ScenarioEighteenOutcome(rand);
		}else if(ScenarioId==19){
			return ScenarioNineteenOutcome(rand);		
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
		return null;
	}
}
