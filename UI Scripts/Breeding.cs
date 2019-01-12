using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breeding : MonoBehaviour {

	//TODO: turns
	private List<Recipe> recipes;
	private Bee bees;
	private Dictionary<string, Recipe> beeRecipes;

	public string[] beeResults;
	public int honeyNumberResult;


	
	void Start(){
		beeRecipes = new Dictionary<string, Recipe>();
		//add all recipes
		addAllRecipesGross();
	}


	//have this called from apiaryOrganiser
	public int startBreeding(GameObject beeOne, GameObject beeTwo){
		//get the types of the bees from the slots
		//then based on the rules breed them
		string key = beeOne.name + "_" + beeTwo.name;
		Recipe temp = beeRecipes[key];

		if(temp!=null){
			beeResults = breed(temp);
			honeyNumberResult = honeyResult(temp);
		}

		return temp.turns;
	}

	public string[] breed(Recipe recipe){

		List<string> results = new List<string>();

		results.Add(recipe.beeOne); //first bee is always the first one that went in

		//second bee depends on the chance
		if(Random.Range(0,100)<=recipe.chance){
			results.Add(recipe.beeResultOne);
		}else{
			results.Add(recipe.beeOne);
		}

		//third bee depends on half the chance. Much rarer!
		if(Random.Range(0,100)<=(recipe.chance/2)){
			results.Add(recipe.beeResultTwo);
		}else{
			results.Add(recipe.beeOne);
		}

		//honey result is a base. Have a function that multiplies around/adds to this within a range
		return results.ToArray();
	}

	public int honeyResult(Recipe recipe){
		int result = 0;
		result = Random.Range(recipe.honeyResult-5,recipe.honeyResult+5) + 3;
		return result;
	}

	public void addAllRecipesGross(){
		Recipe recipe;
		recipe = new Recipe("bland","common",67,"common","worker",25,3);//bland common
		beeRecipes.Add("bland_common", recipe);

		recipe = new Recipe("bland","bland",70,"common","common",10,2);//bland bland
		beeRecipes.Add("bland_bland", recipe);

		recipe = new Recipe("common","common",50,"bland","plains",10,3);//common common
		beeRecipes.Add("common_common", recipe);


	}
}
