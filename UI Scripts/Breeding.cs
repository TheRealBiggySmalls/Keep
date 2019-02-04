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
		string beeO = beeOne.GetComponentInChildren<Bee>().type;
		string beeT = beeTwo.GetComponentInChildren<Bee>().type;

		string key = beeO + "_" + beeT;
		List<string> keys = new List<string>(beeRecipes.Keys);
		//it stopped initialising here due to the duplicate key: FIXED
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
		
		//BASE HONEY RESULT IS 15
		//TODO: fill out lower level bee recipes (only toxic left)
		//TODO: fill out higher level bee recipes (go through one breed at a time)
		//---->>>> suss out names for the breeds so they can be treated properly on the back end
		//add recipes breeding into exotic and mutant bees

		//UP TO TOXIC 
		//MAKE SO SHINY/HIGHER LEVEL BEES WONT BREED WITH LOWER LEVEL BEES: DILIGENT, INTELLIGENT, STRANGE (exotic), TOXIC (mutant) only breed with each other and those above
		//--->>>each of these bees has a distinct name/unique personality

		//<---BLAND---> 
		recipe = new Recipe("bland", "common", 50, "common", "plains", 14,2); //one in four chance for the unnatural tree
		beeRecipes.Add("bland_common", recipe);

		recipe = new Recipe("bland", "bland", 70, "common", "common", 13,2);
		beeRecipes.Add("bland_bland", recipe);

		recipe = new Recipe("bland", "plains", 49, "common", "plains", 14,3);
		beeRecipes.Add("bland_plains", recipe);

		recipe = new Recipe("bland", "forest", 55, "forest", "common", 14,3);
		beeRecipes.Add("bland_forest", recipe);

		recipe = new Recipe("bland", "icy", 50, "icy", "common", 13,3);
		beeRecipes.Add("bland_icy", recipe);

		recipe = new Recipe("bland", "stone", 60, "common", "stone", 11,4); 
		beeRecipes.Add("bland_stone", recipe);

		recipe = new Recipe("bland", "worker", 50, "common", "common", 12,3);
		beeRecipes.Add("bland_worker", recipe);

		recipe = new Recipe("bland", "warrior", 60, "common", "worker", 13,3);
		beeRecipes.Add("bland_warrior", recipe);

		recipe = new Recipe("bland", "shore", 60, "common", "common", 14,3);
		beeRecipes.Add("bland_shore", recipe);

		recipe = new Recipe("bland", "nice", 50, "common", "plains", 13,3);
		beeRecipes.Add("bland_nice", recipe);

		recipe = new Recipe("bland", "ocean", 70, "shore", "shore", 15,3);
		beeRecipes.Add("bland_ocean", recipe);

		recipe = new Recipe("bland", "magic", 40, "forest", "shore", 18,3);
		beeRecipes.Add("bland_magic", recipe);

		recipe = new Recipe("bland", "toxic", 50, "toxic", "toxic", 13,3);
		beeRecipes.Add("bland_toxic", recipe);

		//<---COMMON---> breeds into worker
		recipe = new Recipe("common", "common", 66, "common", "worker", 15,2); //one in three for the natural tree
		beeRecipes.Add("common_common", recipe);

		recipe = new Recipe("common", "bland", 50, "common", "plains", 16,2);
		beeRecipes.Add("common_bland", recipe);

		recipe = new Recipe("common", "plains", 50, "plains", "plains", 16,4);
		beeRecipes.Add("common_plains", recipe);

		recipe = new Recipe("common", "forest", 60, "forest", "forest", 18,3);
		beeRecipes.Add("common_forest", recipe);

		recipe = new Recipe("common", "icy", 50, "icy", "plains", 15,4);
		beeRecipes.Add("common_icy", recipe);

		recipe = new Recipe("common", "stone", 30, "stone", "stone", 18,4); 
		beeRecipes.Add("common_stone", recipe);

		recipe = new Recipe("common", "worker", 60, "worker", "worker", 16,3);
		beeRecipes.Add("common_worker", recipe);

		recipe = new Recipe("common", "warrior", 70, "worker", "warrior", 16,3);
		beeRecipes.Add("common_warrior", recipe);

		recipe = new Recipe("common", "shore", 80, "common", "shore", 16,3);
		beeRecipes.Add("common_shore", recipe);

		recipe = new Recipe("common", "nice", 75, "plains", "nice", 17,3);
		beeRecipes.Add("common_nice", recipe);

		recipe = new Recipe("common", "ocean", 75, "ocean", "shore", 15,3);
		beeRecipes.Add("common_ocean", recipe);

		recipe = new Recipe("common", "magic", 50, "forest", "shore", 19,3);
		beeRecipes.Add("common_magic", recipe);

		recipe = new Recipe("common", "toxic", 50, "toxic", "toxic", 18,3);
		beeRecipes.Add("common_toxic", recipe);

		//<---WORKER---> breeds into warrior and intelligent
		recipe = new Recipe("worker", "forest", 58, "forest", "warrior", 23,3);
		beeRecipes.Add("worker_forest", recipe);

		recipe = new Recipe("worker", "ocean", 33, "ocean", "intelligent", 48,5);
		beeRecipes.Add("worker_ocean", recipe);

		recipe = new Recipe("worker", "warrior", 30, "warrior", "diligent", 41,5);
		beeRecipes.Add("worker_warrior", recipe);

		recipe = new Recipe("worker", "bland", 70, "common", "bland", 19,3); 
		beeRecipes.Add("worker_bland", recipe);

		recipe = new Recipe("worker", "common", 70, "common", "common", 21,3);
		beeRecipes.Add("worker_common", recipe);

		recipe = new Recipe("worker", "plains", 80, "common", "bland", 8,4);
		beeRecipes.Add("worker_plains", recipe);

		recipe = new Recipe("worker", "icy", 70, "common", "common", 9,4);
		beeRecipes.Add("worker_icy", recipe);

		recipe = new Recipe("worker", "stone", 60, "common", "stone", 20,5); 
		beeRecipes.Add("worker_stone", recipe);

		recipe = new Recipe("worker", "worker", 20, "worker", "common", 26,4);
		beeRecipes.Add("worker_worker", recipe);

		recipe = new Recipe("worker", "shore", 90, "common", "shore", 22,3);
		beeRecipes.Add("worker_shore", recipe);

		recipe = new Recipe("worker", "nice", 40, "common", "common", 15,3);
		beeRecipes.Add("worker_nice", recipe);

		recipe = new Recipe("worker", "magic", 50, "magic", "warrior", 28,4);
		beeRecipes.Add("worker_magic", recipe);

		recipe = new Recipe("worker", "toxic", 60, "toxic", "toxic", 21,4);
		beeRecipes.Add("worker_toxic", recipe);

		//<---FOREST---> breeds into ocean
		recipe = new Recipe("forest", "shore", 44, "shore", "ocean", 34,4);
		beeRecipes.Add("forest_shore", recipe);

		recipe = new Recipe("forest", "worker", 59, "worker", "warrior", 24,3);
		beeRecipes.Add("forest_worker", recipe);

		recipe = new Recipe("forest", "bland", 60, "common", "bland", 15,3); 
		beeRecipes.Add("forest_bland", recipe);

		recipe = new Recipe("forest", "common", 65, "common", "common", 20,3);
		beeRecipes.Add("forest_common", recipe);

		recipe = new Recipe("forest", "plains", 80, "common", "bland", 11,5);
		beeRecipes.Add("forest_plains", recipe);

		recipe = new Recipe("forest", "forest", 40, "forest", "common", 25,3);
		beeRecipes.Add("forest_forest", recipe);

		recipe = new Recipe("forest", "icy", 50, "icy", "common", 18,4);
		beeRecipes.Add("forest_icy", recipe);

		recipe = new Recipe("forest", "stone", 75, "stone", "stone", 22,4); 
		beeRecipes.Add("forest_stone", recipe);

		recipe = new Recipe("forest", "warrior", 60, "worker", "warrior", 20,4);
		beeRecipes.Add("forest_warrior", recipe);

		recipe = new Recipe("forest", "nice", 10, "nice", "intelligent", 26,3);
		beeRecipes.Add("forest_nice", recipe);

		recipe = new Recipe("forest", "ocean", 80, "forest", "shore", 25,3);
		beeRecipes.Add("forest_ocean", recipe);

		recipe = new Recipe("forest", "magic", 60, "magic", "magic", 35,4);
		beeRecipes.Add("forest_magic", recipe);

		recipe = new Recipe("forest", "toxic", 70, "toxic", "toxic", 23,4);
		beeRecipes.Add("forest_toxic", recipe);
		
		//<---SHORE---> breeds into ocean 
		recipe = new Recipe("shore", "forest", 44, "shore", "ocean", 33,3);
		beeRecipes.Add("shore_forest", recipe);

		recipe = new Recipe("shore", "bland", 50, "common", "bland", 17,3);
		beeRecipes.Add("shore_bland", recipe);

		recipe = new Recipe("shore", "common", 65, "common", "common", 19,3);
		beeRecipes.Add("shore_common", recipe);

		recipe = new Recipe("shore", "plains", 80, "common", "bland", 11,4);
		beeRecipes.Add("shore_plains", recipe);

		recipe = new Recipe("shore", "icy", 50, "icy", "common", 17,4);
		beeRecipes.Add("shore_icy", recipe);

		recipe = new Recipe("shore", "stone", 60, "stone", "stone", 22,5); 
		beeRecipes.Add("shore_stone", recipe);

		recipe = new Recipe("shore", "worker", 60, "worker", "common", 25,4);
		beeRecipes.Add("shore_worker", recipe);

		recipe = new Recipe("shore", "warrior", 60, "worker", "forest", 22,4);
		beeRecipes.Add("shore_warrior", recipe);

		recipe = new Recipe("shore", "shore", 10, "shore", "ocean", 26,3);
		beeRecipes.Add("shore_shore", recipe);

		recipe = new Recipe("shore", "nice", 20, "nice", "ocean", 24,3);
		beeRecipes.Add("shore_nice", recipe);

		recipe = new Recipe("shore", "ocean", 90, "ocean", "ocean", 25,3);
		beeRecipes.Add("shore_ocean", recipe);

		recipe = new Recipe("shore", "magic", 60, "magic", "ocean", 34,4);
		beeRecipes.Add("shore_magic", recipe);

		recipe = new Recipe("shore", "toxic", 80, "toxic", "toxic", 24,4);
		beeRecipes.Add("shore_toxic", recipe);

		//<---OCEAN---> breeds into intelligent 
		recipe = new Recipe("ocean", "worker", 33, "ocean", "intelligent", 45,5);
		beeRecipes.Add("ocean_worker", recipe);

		recipe = new Recipe("ocean", "bland", 75, "bland", "shore", 19,3); 
		beeRecipes.Add("ocean_bland", recipe);

		recipe = new Recipe("ocean", "common", 65, "forest", "shore", 28,3);
		beeRecipes.Add("ocean_common", recipe);

		recipe = new Recipe("ocean", "plains", 80, "common", "bland", 15,5);
		beeRecipes.Add("ocean_plains", recipe);

		recipe = new Recipe("ocean", "forest", 65, "forest", "forest", 24,4);
		beeRecipes.Add("ocean_forest", recipe);

		recipe = new Recipe("ocean", "icy", 80, "bland", "bland", 13,5);
		beeRecipes.Add("ocean_icy", recipe);

		recipe = new Recipe("ocean", "stone", 45, "icy", "bland", 22,6); 
		beeRecipes.Add("ocean_stone", recipe);

		recipe = new Recipe("ocean", "warrior", 50, "warrior", "warrior", 23,4);
		beeRecipes.Add("ocean_warrior", recipe);

		recipe = new Recipe("ocean", "shore", 55, "shore", "forest", 26,4);
		beeRecipes.Add("ocean_shore", recipe);

		recipe = new Recipe("ocean", "nice", 30, "nice", "nice", 29,4);
		beeRecipes.Add("ocean_nice", recipe);

		recipe = new Recipe("ocean", "ocean", 2, "shore", "forest", 30,3);
		beeRecipes.Add("ocean_ocean", recipe);

		recipe = new Recipe("ocean", "magic", 30, "magic", "mutant", 34,4);
		beeRecipes.Add("ocean_magic", recipe);

		recipe = new Recipe("ocean", "toxic", 80, "toxic", "toxic", 28,4);
		beeRecipes.Add("ocean_toxic", recipe);

		//<---WARRIOR---> breeds into diligent
		recipe = new Recipe("warrior", "worker", 30, "worker", "diligent", 47,5);
		beeRecipes.Add("warrior_worker", recipe);

		recipe = new Recipe("warrior", "bland", 50, "worker", "bland", 14,3); 
		beeRecipes.Add("warrior_bland", recipe);

		recipe = new Recipe("warrior", "common", 50, "forest", "worker", 28,3);
		beeRecipes.Add("warrior_common", recipe);

		recipe = new Recipe("warrior", "plains", 90, "common", "bland", 4,5);
		beeRecipes.Add("warrior_plains", recipe);

		recipe = new Recipe("warrior", "forest", 50, "forest", "worker", 26,4);
		beeRecipes.Add("warrior_forest", recipe);

		recipe = new Recipe("warrior", "icy", 98, "bland", "bland", 4,6);
		beeRecipes.Add("warrior_icy", recipe);

		recipe = new Recipe("warrior", "stone", 80, "worker", "bland", 19,6); 
		beeRecipes.Add("warrior_stone", recipe);

		recipe = new Recipe("warrior", "warrior", 50, "warrior", "warrior",32,4);
		beeRecipes.Add("warrior_warrior", recipe);

		recipe = new Recipe("warrior", "shore", 20, "shore", "shore", 25,4);
		beeRecipes.Add("warrior_shore", recipe);

		recipe = new Recipe("warrior", "nice", 100, "bland", "bland", 4,8);
		beeRecipes.Add("warrior_nice", recipe);

		recipe = new Recipe("warrior", "ocean", 74, "worker", "common", 25,4);
		beeRecipes.Add("warrior_ocean", recipe);

		recipe = new Recipe("warrior", "magic", 100, "nice", "nice", 40,4);
		beeRecipes.Add("warrior_magic", recipe);

		recipe = new Recipe("warrior", "toxic", 80, "toxic", "toxic", 27,4);
		beeRecipes.Add("warrior_toxic", recipe);

		//<<<<----------UNNATURAL TREE----------->>>>

		//<---PLAINS---> breeds into nice
		recipe = new Recipe("plains", "icy", 50, "icy", "nice", 25,4);
		beeRecipes.Add("plains_icy", recipe);

		recipe = new Recipe("plains", "bland", 90, "common", "bland", 11,2);
		beeRecipes.Add("plains_bland", recipe);

		recipe = new Recipe("plains", "common", 50, "common", "bland", 19,3);
		beeRecipes.Add("plains_common", recipe);

		recipe = new Recipe("plains", "plains", 20, "common", "common", 18,2);
		beeRecipes.Add("plains_plains", recipe);

		recipe = new Recipe("plains", "forest", 70, "common", "forest", 18,3);
		beeRecipes.Add("plains_forest", recipe);

		recipe = new Recipe("plains", "stone", 60, "common", "stone", 22,4); 
		beeRecipes.Add("plains_stone", recipe);

		recipe = new Recipe("plains", "worker", 90, "common", "common", 20,3);
		beeRecipes.Add("plains_worker", recipe);

		recipe = new Recipe("plains", "warrior", 90, "common", "common", 12,3);
		beeRecipes.Add("plains_warrior", recipe);

		recipe = new Recipe("plains", "shore", 70, "shore", "common", 19,3);
		beeRecipes.Add("plains_shore", recipe);

		recipe = new Recipe("plains", "nice", 80, "nice", "worker", 24,3);
		beeRecipes.Add("plains_nice", recipe);

		recipe = new Recipe("plains", "ocean", 60, "shore", "shore", 18,3);
		beeRecipes.Add("plains_ocean", recipe);

		recipe = new Recipe("plains", "magic", 50, "icy", "metallic", 26,4);
		beeRecipes.Add("plains_magic", recipe);

		recipe = new Recipe("plains", "toxic", 80, "toxic", "toxic", 22,4);
		beeRecipes.Add("plains_toxic", recipe);

		//<<---ICY---> breeds into nice
		recipe = new Recipe("icy", "plains", 50, "plains", "nice", 24,4);
		beeRecipes.Add("icy_plains", recipe);

		recipe = new Recipe("icy", "bland", 80, "common", "bland", 12,3);
		beeRecipes.Add("icy_bland", recipe);

		recipe = new Recipe("icy", "common", 55, "plain", "common", 21,3);
		beeRecipes.Add("icy_common", recipe);

		recipe = new Recipe("icy", "forest", 70, "forest", "forest", 17,4);
		beeRecipes.Add("icy_forest", recipe);

		recipe = new Recipe("icy", "icy", 50, "icy", "nice", 39,4);
		beeRecipes.Add("icy_icy", recipe);

		recipe = new Recipe("icy", "stone", 60, "stone", "nice", 22,5); 
		beeRecipes.Add("icy_stone", recipe);

		recipe = new Recipe("icy", "worker", 80, "common", "plains", 21,4);
		beeRecipes.Add("icy_worker", recipe);

		recipe = new Recipe("icy", "warrior", 90, "bland", "bland", 8,5);
		beeRecipes.Add("icy_warrior", recipe);

		recipe = new Recipe("icy", "shore", 10, "shore", "ocean", 20,4);
		beeRecipes.Add("icy_shore", recipe);

		recipe = new Recipe("icy", "nice", 70, "nice", "nice", 24,3);
		beeRecipes.Add("icy_nice", recipe);

		recipe = new Recipe("icy", "ocean", 90, "shore", "shore", 20,4);
		beeRecipes.Add("icy_ocean", recipe);

		recipe = new Recipe("icy", "toxic", 80, "toxic", "toxic", 32,4);
		beeRecipes.Add("icy_toxic", recipe);

		recipe = new Recipe("icy", "magic", 75, "worker", "magic", 40,4);
		beeRecipes.Add("icy_magic", recipe);

		//<<---NICE---> breeds into mutant(toxic) and magic
		recipe = new Recipe("nice", "stone", 35, "stone", "toxic", 28,5);
		beeRecipes.Add("nice_stone", recipe);

		recipe = new Recipe("nice", "toxic", 5, "magic", "magic", 28,5);
		beeRecipes.Add("nice_toxic", recipe);

		recipe = new Recipe("nice", "bland", 90, "icy", "plain", 14,3);
		beeRecipes.Add("nice_bland", recipe);

		recipe = new Recipe("nice", "common", 55, "icy", "common", 24,3);
		beeRecipes.Add("nice_common", recipe);

		recipe = new Recipe("nice", "plains", 60, "icy", "plains", 22,4);
		beeRecipes.Add("nice_plains", recipe);

		recipe = new Recipe("nice", "forest", 60, "forest", "icy", 20,4);
		beeRecipes.Add("nice_forest", recipe);

		recipe = new Recipe("nice", "icy", 40, "icy", "icy", 27,3);
		beeRecipes.Add("nice_icy", recipe);

		recipe = new Recipe("nice", "worker", 70, "worker", "worker", 23,4);
		beeRecipes.Add("nice_worker", recipe);

		recipe = new Recipe("nice", "warrior", 90, "bland", "bland", 11,5);
		beeRecipes.Add("nice_warrior", recipe);

		recipe = new Recipe("nice", "shore", 70, "shore", "plains", 21,4);
		beeRecipes.Add("nice_shore", recipe);

		recipe = new Recipe("nice", "nice", 30, "forest", "forest", 25,4);
		beeRecipes.Add("nice_nice", recipe);

		recipe = new Recipe("nice", "ocean", 82, "ocean", "ocean", 21,4);
		beeRecipes.Add("nice_ocean", recipe);

		recipe = new Recipe("nice", "magic", 100, "warrior", "warrior", 40,4);
		beeRecipes.Add("nice_magic", recipe);

		//<<---STONE---> breeds into mutant(toxic) and toxic(mutant)
		recipe = new Recipe("stone", "nice", 40, "nice", "toxic", 28,5);
		beeRecipes.Add("stone_nice", recipe);

		recipe = new Recipe("stone", "toxic", 25, "toxic", "mutant", 31,6); //MIGHT BE WRONG TOXIC/MUTANT
		beeRecipes.Add("stone_toxic", recipe);

		recipe = new Recipe("stone", "bland", 80, "common", "bland", 12,3);
		beeRecipes.Add("stone_bland", recipe);

		recipe = new Recipe("stone", "common", 50, "common", "common", 28,3);
		beeRecipes.Add("stone_common", recipe);

		recipe = new Recipe("stone", "plains", 75, "common", "common", 22,4);
		beeRecipes.Add("stone_plains", recipe);

		recipe = new Recipe("stone", "forest", 60, "plain", "forest", 21,4);
		beeRecipes.Add("stone_forest", recipe);

		recipe = new Recipe("stone", "icy", 70, "icy", "nice", 26,4);
		beeRecipes.Add("stone_icy", recipe);

		recipe = new Recipe("stone", "stone", 100, "stone", "stone", 65,7); 
		beeRecipes.Add("stone_stone", recipe);

		recipe = new Recipe("stone", "worker", 95, "common", "common", 54,6);
		beeRecipes.Add("stone_worker", recipe);

		recipe = new Recipe("stone", "warrior", 60, "common", "common", 13,3);
		beeRecipes.Add("stone_warrior", recipe);

		recipe = new Recipe("stone", "shore", 70, "bland", "common", 24,4);
		beeRecipes.Add("stone_shore", recipe);

		recipe = new Recipe("stone", "ocean", 80, "icy", "ocean", 26,5);
		beeRecipes.Add("stone_ocean", recipe);

		recipe = new Recipe("stone", "magic", 75, "magic", "ocean", 38,5);
		beeRecipes.Add("stone_magic", recipe);

		//<<---MUTANT---> breeds into magic, strange (exotic) and mutant(toxic)
		recipe = new Recipe("toxic", "nice", 6, "magic", "magic", 28,5);
		beeRecipes.Add("toxic_nice", recipe);

		recipe = new Recipe("toxic", "magic", 5, "magic", "exotic", 38,6);
		beeRecipes.Add("toxic_magic", recipe);

		recipe = new Recipe("toxic", "stone", 25, "stone", "mutant", 30,6); //MIGHT BE WRONG TOXIC/MUTANT
		beeRecipes.Add("toxic_stone", recipe);

		recipe = new Recipe("toxic", "bland", 30, "stone", "mutant", 20,4);
		beeRecipes.Add("toxic_bland", recipe);

		recipe = new Recipe("toxic", "common", 55, "nice", "common", 29,3);
		beeRecipes.Add("toxic_common", recipe);

		recipe = new Recipe("toxic", "plains", 65, "nice", "nice", 24,5);
		beeRecipes.Add("toxic_plains", recipe);

		recipe = new Recipe("toxic", "forest", 80, "bland", "bland", 16,5);
		beeRecipes.Add("toxic_forest", recipe);

		recipe = new Recipe("toxic", "icy", 80, "bland", "plains", 33,4);
		beeRecipes.Add("toxic_icy", recipe);

		recipe = new Recipe("toxic", "worker", 80, "common", "bland", 29,5);
		beeRecipes.Add("toxic_worker", recipe);

		recipe = new Recipe("toxic", "warrior", 70, "worker", "worker", 26,4);
		beeRecipes.Add("toxic_warrior", recipe);

		recipe = new Recipe("toxic", "shore", 40, "shore", "forest", 28,4);
		beeRecipes.Add("toxic_shore", recipe);

		recipe = new Recipe("toxic", "ocean", 80, "bland", "bland", 23,5);
		beeRecipes.Add("toxic_ocean", recipe);

		recipe = new Recipe("toxic", "toxic", 30, "toxic", "mutant", 28,5);
		beeRecipes.Add("toxic_toxic", recipe);

		//<<---MAGIC---> breeds into mutant(toxic) and toxic(mutant)
		recipe = new Recipe("magic", "toxic", 5, "toxic", "exotic", 42,6);
		beeRecipes.Add("magic_toxic", recipe);

		recipe = new Recipe("magic", "magic", 4, "exotic", "exotic", 50,6);
		beeRecipes.Add("magic_magic", recipe);

		recipe = new Recipe("magic", "bland", 12, "toxic", "exotic", 22,5); 
		beeRecipes.Add("magic_bland", recipe);

		recipe = new Recipe("magic", "common", 55, "nice", "common", 45,4);
		beeRecipes.Add("magic_common", recipe);

		recipe = new Recipe("magic", "plains", 60, "nice", "nice", 44,5);
		beeRecipes.Add("magic_plains", recipe);

		recipe = new Recipe("magic", "forest", 70, "worker", "ocean", 34,5);
		beeRecipes.Add("magic_forest", recipe);

		recipe = new Recipe("magic", "icy", 70, "nice", "icy", 48,5);
		beeRecipes.Add("magic_icy", recipe);

		recipe = new Recipe("magic", "stone", 65, "stone", "mutant", 49,6); 
		beeRecipes.Add("magic_stone", recipe);

		recipe = new Recipe("magic", "worker", 50, "worker", "warrior", 48,7);
		beeRecipes.Add("magic_worker", recipe);

		recipe = new Recipe("magic", "warrior", 70, "forest", "warrior", 48,5);
		beeRecipes.Add("magic_warrior", recipe);

		recipe = new Recipe("magic", "shore", 55, "forest", "ocean", 47,5);
		beeRecipes.Add("magic_shore", recipe);

		recipe = new Recipe("magic", "nice", 75, "forest", "shore", 43,4);
		beeRecipes.Add("magic_nice", recipe);

		recipe = new Recipe("magic", "ocean", 80, "forest", "ocean", 46,5);
		beeRecipes.Add("magic_ocean", recipe);

	}	
}
