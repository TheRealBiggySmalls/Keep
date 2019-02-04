using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniquesBackpack : MonoBehaviour {

	public Texture defaultTexture;

	public GameObject UniquesWindow; public GameObject tooltipWindow;
	//OBJECTS: boat, book-o-compliments, pickaxe, honeycomb, sword, flower, robot, shoes, hazmat suit, oneMore
	public List<GameObject> uniqueSlots;

	public Dictionary<string, bool> itemTruth;

	void Start(){

		tooltipWindow = UniquesWindow.GetComponentsInChildren<RawImage>()[UniquesWindow.GetComponentsInChildren<RawImage>().Length-1].gameObject;			

		initUniqueItems();

		updateTextures();
		createDict();

		tooltipWindow.SetActive(false);

		toggleBackpack();
	}

	public void createDict(){
		if(itemTruth!=null){
			itemTruth=null;
		}
		itemTruth = new Dictionary<string, bool>(); //HAVE TO REININT DICT
		foreach(GameObject obj in uniqueSlots){
			UniqueItem item = obj.GetComponentInChildren<UniqueItem>();
			itemTruth.Add(item.uniqueName, item.activated);
		}
	}
	

	public void updateTextures(){
		foreach(GameObject obj in uniqueSlots){
			UniqueItem item = obj.GetComponentInChildren<UniqueItem>();
			if(!item.activated){
				obj.GetComponentInChildren<RawImage>().texture = defaultTexture;
			}else{
				obj.GetComponentInChildren<RawImage>().texture = item.texture;
			}
		}
	}
	public void initUniqueItems(){
		
		foreach(GameObject obj in uniqueSlots){
			UniqueItem uniItem = obj.GetComponent<UniqueItem>();
			RawImage img = obj.GetComponent<RawImage>();
			//HAVE TO DO LIKE THIS AT THE MOMENT AS WE DONT HAVE ALL TEXTURES FILLED OUT
			if(img.texture!=null){
				Texture texture = obj.GetComponent<RawImage>().texture;

				if(texture.name=="boat"){
					uniItem.SetUniqueItemParams("boat", "Fishermans Rowey", "An old rowboat used for basic fishing", texture, tooltipWindow);
				}else if(texture.name=="book"){
					uniItem.SetUniqueItemParams("book", "Bee Compendium","A must have for every budding beekeeper and bee enthusiast!", texture, tooltipWindow);
				}else if(texture.name=="pickaxe"){
					uniItem.SetUniqueItemParams("pickaxe", "Glass Pickaxe","A delicate pickaxe left behind from when the hills ran dry. Stronger than diamond.", texture,tooltipWindow);
				}else if(texture.name=="honeycomb"){
					uniItem.SetUniqueItemParams("honeycomb", "Sticky Comb","A luscious comb, sure to make the bees feel at home!", texture,tooltipWindow);
				}else if(texture.name=="sword"){
					uniItem.SetUniqueItemParams("sword", "Shiny Sword","A toy sword gleaming with intensity.",texture,tooltipWindow);
				}else if(texture.name=="rose"){
					uniItem.SetUniqueItemParams("rose", "Eternal Rose","A delicate flower which defies the hours. It never grows old or weary.",texture,tooltipWindow);
				}else if(texture.name=="robot"){
					uniItem.SetUniqueItemParams("robot", "Compliment-O-Bot", "Once bent on destroying all sentient life, this little robot has been reprogrammed to endlessly bark out compliments out the user.",texture,tooltipWindow);
				}else if(texture.name=="shoe"){
					uniItem.SetUniqueItemParams("shoe", "Yeezys","A pair of squeak proof sneakers.",texture,tooltipWindow);
				}else if(texture.name=="cloak"){
					uniItem.SetUniqueItemParams("cloak", "Magic Proof Robe","A special robe that protects the wearer from all kinds of unnatural effects!",texture,tooltipWindow);
				}
			}else{
				uniItem.SetUniqueItemParams("PLACEHOLDER", "PLACEHOLDER","PLACEHOLDER", defaultTexture,tooltipWindow);
			}
		}
	}

	public void toggleBackpack(){
		if(UniquesWindow.activeSelf==true){
			UniquesWindow.SetActive(false);
		}else{
			UniquesWindow.SetActive(true);
		}
	}

	//why are they buttons? Show tooltip on click probably?

	//cannibalise from apiaryOrganiser. Will share much of the same "set amount of items in slots 
	//that are revealed when aquired. Each has a tooltip which vaguely says what it is. Doesn't
	//give hints as to how its used. Character will have a uniques backpack which can be checked
	//if(contains) from scenario manager for unique options
}
