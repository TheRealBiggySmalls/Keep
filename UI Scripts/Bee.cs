using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bee : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public string type;

	//DESCRIPTIVE VARIABLES FOR TOOLTIP
	private string nature, description,displayName;
	private bool dominant;

	private bool activated=false;
	public int quantity;
	public GameObject beeObject;
	private GameObject toolTip;
	
	public void setToolTip(GameObject obj){
		toolTip = obj;
		initText();
		//assignHover();
	}

	public bool updateVisuals(){
		if(quantity>=0){
			activated=true;
		}
		return activated;
	}

	public void showToolTip(){
		toolTip.SetActive(true);
		Text[] fields = toolTip.GetComponentsInChildren<Text>();
		fields[0].text = displayName;
		fields[1].text = "Nature:  " + nature;
		if(dominant){
			fields[2].text = "Genes:  " + "Dominant";
		}else{
			fields[2].text = "Genes:  " + "Recessive";
		}
		fields[3].text = "'" + description + ".'";
	}

	public void hideToolTip(){
		toolTip.SetActive(false);
	}

	public void OnPointerEnter(PointerEventData eventData){
		showToolTip();
	}

	public void OnPointerExit(PointerEventData eventData){
		hideToolTip();
	}

	//in here have it pass its texture


	public void initText(){
		//HARD CODE THESE
		if(this.type=="bland"){
			displayName = "Bland";
			nature = "Oblivious";
			dominant = false;
			description = "The most plain bee I ever did see";
		}else if(this.type=="common"){
			displayName = "Common";
			nature = "Mild";
			dominant = false;
			description = "A staple of apiaries around the globe";
		}else if(this.type=="diligent"){
			displayName = "Diligent";
			nature = "Busy";
			dominant = true;
			description = "A whir of activity";
		}else if(this.type=="diligentWorker"){
			displayName = "Factory";
			nature = "Industrious";
			dominant = true;
			description = "So focused on producing he forgets to smell the flowers";
		//will kill off other bees in your hive if above a certain percentage
		}else if(this.type=="diligentWarrior"){
			displayName = "Killer";
			nature = "Hostile";
			dominant = true;
			description = "Watch out! This one cares only for death";
		//exotic should have a chance to produce any bee
		}else if(this.type=="exotic"){
			displayName = "Strange";
			nature = "???";
			dominant = false;
			description = "???";
		}else if(this.type=="exoticShore"){
			displayName = "Strange";
			nature = "???";
			dominant = false;
			description = "???";
		}else if(this.type=="exoticWorker"){
			displayName = "Strange";
			nature = "???";
			dominant = false;
			description = "???";
		}else if(this.type=="forest"){
			displayName = "Natural";
			nature = "Pure";
			dominant = true;
			description = "A child of the Earth";
		}else if(this.type=="icy"){
			displayName = "Icy";
			nature = "Distant";
			dominant = false;
			description = "Cold to the touch. Prefers to live in isolation outside of the hive";
		}else if(this.type=="intelligent"){
			displayName = "Intelligent";
			nature = "Clever";
			dominant = true;
			description = "Always finding ways to work smarter, not harder";
		//will form a hive mind and create a npc that tries to steal your resources
		}else if(this.type=="intelligentCommon"){
			displayName = "Practical";
			nature = "Inspired";
			dominant = false;
			description = "Unconfirmed sources report their ability to play chess";
		}else if(this.type=="intelligentNice"){
			displayName = "Genius";
			nature = "Lazy";
			dominant = false;
			description = "More likely to make you work for it";
		}else if(this.type=="magic"){
			displayName = "Magical";
			nature = "Miraculous";
			dominant = true;
			description = "This peculiar bee sees to exist half in our reality and half in another, phasing in and out at will!";
		}else if(this.type=="mutant"){ //MUTANT AND TOXIC ARE SWITCHED
			displayName = "Mutant";
			nature = "Lonely";
			dominant = true;
			description = "Hideous and deformed";
		}else if(this.type=="mutantMagic"){
			displayName = "Abomination";
			nature = "Disgusting";
			dominant = true;
			description = "Some things are best left forgotten";
		}else if(this.type=="mutantToxic"){
			displayName = "Stank";
			nature = "Incredibly Dangerous";
			dominant = true;
			description = "The result of a horribly failed experiment in which a 20-something year old Spanish man tried to turn himself into a bee to fullfill his lifelong sexual fantasies.";
		}else if(this.type=="nice"){
			displayName = "Nicey";
			nature = "Kind";
			dominant = false;
			description = "This strange species has no ability to sting";
		}else if(this.type=="ocean"){
			displayName = "Oceanic";
			nature = "Soothing";
			dominant = true;
			description = "Hums in waves";
		}else if(this.type=="plains"){
			displayName = "Plain";
			nature = "Simple";
			dominant = false;
			description = "Simple and hardy. Found everywhere you wouldn't expect it to be";
		}else if(this.type=="shore"){
			displayName = "Water";
			nature = "Agitated";
			dominant = true;
			description = "At night it sings out to its lost brothers, as if stranded";
		}else if(this.type=="stone"){
			displayName = "Metallic";
			nature = "Stubborn";
			dominant = true;
			description = "Heavier than a dog, and slower than a snail";
		}else if(this.type=="toxic"){ //TOXIC AND MUTANT ARE SWITCHED NAMEWISE
			displayName = "Toxic";
			nature = "???";
			dominant = true;
			description = "This bee does not seem well";
		}else if(this.type=="warrior"){
			displayName = "Warrior";
			nature = "Aggressive";
			dominant = true;
			description = "A quick way to a sore thumb";
		}else if(this.type=="worker"){
			displayName = "Worker";
			nature = "Busy";
			dominant = true;
			description = "Always busy at work";
		}
	}
}
