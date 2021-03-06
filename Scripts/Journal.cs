﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour {

	public Sprite inactiveTexture, activeTexture;
	public GameObject journal; private static Journal j;
	public Toggle[] tabs;
	public GameObject[] content; public Texture defaultBee; public GameObject[] rumours; public Texture shopkeeper, book;
	public ApiaryOrganiser apiary; private Character player;
	
	void Start(){
		journal=this.gameObject; j = this;
		tabs=journal.GetComponentsInChildren<Toggle>();
		apiary=GameObject.Find("Canvas").GetComponentInChildren<ScenarioManager>().apiary.GetComponentInChildren<ApiaryOrganiser>();
		foreach(Toggle tab in tabs){
			ChangeActiveState(tab);
		}
		foreach(GameObject obj in rumours){
			obj.SetActive(false);
		}
		journal.SetActive(false);
		rumoursHad= new List<string>();
	}

	public void ChangeActiveState(Toggle tab){
		if(tab.isOn){
			tab.GetComponentInChildren<Image>().sprite = activeTexture;
			tab.GetComponentInChildren<Text>().color = Color.black;
			changeContent(tab);
		}else{
			tab.GetComponentInChildren<Image>().sprite = inactiveTexture;
			tab.GetComponentInChildren<Text>().color = Color.white;
		}
	}

	public void enableJournalWindow(bool enable){
		journal.SetActive(enable);
		updateStoryContent();
		updateBeeContent();
	}
	//should set content active by default when clicked

	public void changeContent(Toggle tab){
		if(tab.name=="TabOne"){	
			displayContentOne();
		}else if(tab.name=="TabTwo"){
			displayContentTwo();
		}else if(tab.name=="TabThree"){
			displayContentThree();
		}
	}
	
	public void displayContentOne(){
		content[1].SetActive(false); content[0].SetActive(true);content[2].SetActive(false);
	}

	public void displayContentTwo(){
		content[1].SetActive(true); content[0].SetActive(false);content[2].SetActive(false);
		updateBeeContent(); //fixes bees being displayed even if the bee tab of the journal is currently open
	}
	public void updateBeeContent(){
		foreach(RawImage bee in content[1].GetComponentsInChildren<RawImage>()){
			//if apiary.bee.quantity>0
			foreach(GameObject obj in apiary.bees){
				Bee item = obj.GetComponentInChildren<Bee>();
				if(item.name==bee.name){
					if(item.quantity>=0){
						bee.texture=obj.GetComponentInChildren<RawImage>().texture;
					}else{
						bee.texture=defaultBee;
						bee.CrossFadeAlpha(0.5f,1f,true);
					}
				}
			}
		}
	}

	public GameObject[] storyTriggers; public Texture one,two,three,four,five;
	public void displayContentThree(){
		content[1].SetActive(false); content[0].SetActive(false);content[2].SetActive(true);
		updateStoryContent();
	}

	public void updateStoryContent(){
		string textVariable="DEFAULT";
		Texture texture=null;

		if(player==null){
			player=GameObject.Find("Map").GetComponentInChildren<Map_Continent>().player;
		}

		for(int i=0;i<=4;i++){
			if(i==player.storyProgress){
				storyTriggers[i].SetActive(true);//this is pre discovery so keep all the shiz default
			}else if(i<player.storyProgress){
				if(i==0){
					textVariable="'There are good men at this camp, but tomorrow I will have to leave to meet a wise man in town. He will help guide me where books have failed...'";
					texture=one;
				}else if(i==1){
					textVariable="'I have been reading some old texts and believe there exists strange and extravagent species with talents beyond our wildest imaginations. I have been ridiculed by my peers, but have heard of another such as myself near the idustrial district. I will seek him out...'";
					texture=two;
				}else if(i==2){
					textVariable="'Disaster has struck my friend down! After weeks of experimentation we were able to create a bee of marvelous disposition, but at a great cost. I need to continue my research with this bee, and have heard my aquaintance speak many times of the tomes deep within the castle...'";
					texture=three;
				}else if(i==3){
					textVariable="'I have found what I was looking for... nothing can stop me now... no one can stop me now. My dreams and aspirations will finally be complete! Those fools at the university HAHAHA...'";
					texture=four;
				}else if(i==4){
					textVariable="*buzzes hornily*";
					texture=five;
				}
				storyTriggers[i].GetComponentInChildren<Text>().text = textVariable;//set text to post story text
				storyTriggers[i].GetComponentsInChildren<RawImage>()[1].texture = texture;//setToPostTexture
				storyTriggers[i].SetActive(true);
			}else{
				storyTriggers[i].SetActive(false);
			}
		}
	}

	private List<string> rumoursHad;
	public static void addStringToRumoursPage(string rumour, string textureType){
		if(j.rumoursHad.Contains(rumour)){
			return;
		}
		foreach(GameObject obj in j.rumours){
			if(!obj.activeSelf){
				obj.GetComponentInChildren<Text>().text = rumour;
				if(textureType=="book"){
					obj.GetComponentsInChildren<RawImage>()[1].texture=j.book;
				}else if(textureType=="shop"){
					obj.GetComponentsInChildren<RawImage>()[1].texture=j.shopkeeper;
				}
				obj.SetActive(true);
				j.rumoursHad.Add(rumour);
				return;
			}
		}
		//if the code here is reached than all thr rumours are full. so pick a random one and replace it
		int rand = (int)Random.Range(0,4.99f);
		j.rumours[rand].GetComponentInChildren<Text>().text=rumour;
		if(textureType=="book"){
			j.rumours[rand].GetComponentsInChildren<RawImage>()[1].texture=j.book;
		}else if(textureType=="shop"){
			j.rumours[rand].GetComponentsInChildren<RawImage>()[1].texture=j.shopkeeper;
		}
		j.rumours[rand].SetActive(true);
		j.rumoursHad.Add(rumour);

	}

	
}
