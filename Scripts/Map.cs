using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using QPath;

//QPATH is just a set of interfaces to make pathfinding generic for later projects
public class Map : MonoBehaviour {

	//make map stay around after being rendered once
	public GameObject hexPrefab;
	public GameObject hexTop;
	public GameObject Boy;
	public Character player;

	//can here have arrays of mesh filters for different biomes
	//NEED BASE COLOR MESH FOR GROUND THEN TO DRAW MESH ON TOP USING COMPONENTS

	//ONLY need top meshes. Change colour of base hex but it's always the same. Change mesh
	//of topHex
	public Mesh MeshWater;//DEFAULT hex mesh (hex11)
	public Mesh TopMountain; public Mesh houseMesh; public Mesh[] TopDesert; public Mesh[] TopForest; public Mesh[] TopPlains; public Mesh[] TopSnow;
	public Material MatOcean;public Material MatPlains;public Material MatDesert;public Material MatMountains;public Material MatForest;public Material MatGrasslands;public Material MatSnow;

	//tiles with height above ____ are considered ____
	public float HeightMountain=0.5f;
	public float HeightSnow=0.46f;
	public float HeightForest=0.5f;
	public float HeightGrass = 0.1f;
	public float HeightFlat = 0.0f;


	// Size of the map in terms of number of hex tiles
	// This is NOT representative of the amount of 
	// world space that we're going to take up.
	// (i.e. our tiles might be more or less than 1 Unity World Unit)
	public int numRows = 20;
	public int numCols = 40;

	private Hex[,] hexes;
	public Dictionary<Hex,GameObject> hexToGameObjectMap;
	private Dictionary<Character, GameObject> charToGameObject;
	public bool allowWrapNorthSouth=false, allowWrapEastWest=false;

	// Use this for initialization
	void Start () {
		GenerateMap();
		//after generate map can generate more ocean tiles around the whole world that are in
		//a new array and are never interacted with
		PlaceBoy(Boy, 12, 11);
		FocusCameraOnBoy();
	}

	void Update(){
		//TEST: hit spacebar to advance to next turn
		if(Input.GetKeyDown(KeyCode.Space)){
			if(player!=null){
				player.doTurn();
			}
		}
	}

	public Hex GetHexAt(int x, int y){
		if(hexes == null){
			Debug.LogError("Hexes not instantiated");
			return null;
		}
		if(allowWrapEastWest){
			x = x % numRows;
			if(x<0){
				x+=numRows;
			}
		}
		if(allowWrapNorthSouth){
			y = y % numCols;
			if(y<0){
				y+=numCols;
			}
		}	

		//args must be passed in as col:row
		try{
			return hexes[x,y];
		}catch{
			Debug.LogError("GetHexAt: "+x + "," + y);
			return null;
		}
	}

	public Vector3 GetHexPosition(int q, int r){
		Hex h = GetHexAt(q,r);
		return GetHexPosition(h);
	}
	public Vector3 GetHexPosition(Hex hex){
		return hex.PositionFromCamera(Camera.main.transform.position, numRows, numCols);
	}

	//CURRENTLY generates map in a rhombus shape... not bad for landing on corner of map/boat?
	virtual public void GenerateMap(){
		
		//stores all hex tiles with columns first them rows as index
		//Dictionary maps hex to game object at that location
		hexes = new Hex[numCols,numRows];
		hexToGameObjectMap = new Dictionary<Hex, GameObject>();

		//Generate map filled with ocean
		for (int col = 0; col < numCols; col++) {
			for (int row = 0; row < numRows; row++) {

				Hex h = new Hex(this,col,row);
				h.Elevation = -0.5f; //-1 is default elevation for water
				hexes[col,row] = h;

				Vector3 pos = h.PositionFromCamera(Camera.main.transform.position,numRows,numCols);

				GameObject hexGo = (GameObject) Instantiate(hexPrefab,pos,
					Quaternion.identity,this.transform);
				//currently spawns candy everywhere
				//GameObject hexT = (GameObject) Instantiate(hexTop, pos,
					//Quaternion.identity,hexGo.transform);

				//hexToGameObjectMap.Add(h, hexGo);
				hexToGameObjectMap[h] = hexGo;

				hexGo.name = string.Format("HEX: {0}, {1}", col, row);
				hexGo.GetComponent<HexComponent>().hex = h;
				hexGo.GetComponent<HexComponent>().hexMap = this;

				hexGo.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", col, row);
				
				//lowPolyHex has a combined material. Could consider using this???
				MeshRenderer mr = hexGo.GetComponentInChildren<MeshRenderer>();
				mr.material = MatOcean; //hexMaterials[Random.Range(0, hexMaterials.Length)];

				MeshFilter mf = hexGo.GetComponentInChildren<MeshFilter>();
				mf.mesh = MeshWater;

			}
		}
		//StaticBatchingUtility.Combine(this.gameObject);
	}

	public void UpdateHexVisuals(){

		for (int col = 0; col < numCols; col++) {
			for (int row = 0; row < numRows; row++) {
				Hex h = hexes[col, row];
				GameObject hexGo = hexToGameObjectMap[h];
	
				MeshRenderer mr = hexGo.GetComponentInChildren<MeshRenderer>();
				//MeshFilter mf = hexGo.GetComponentInChildren<MeshFilter>();
				
				string objString = string.Format("HEX: " + col + ", " + row);
				GameObject obj = GameObject.Find(objString);

				Transform t = obj.GetComponentInChildren<Transform>().Find("Hex11").Find("HexTop");
				MeshFilter mf = t.GetComponentInChildren<MeshFilter>();
				//GameObject temp = hexGo.GetComponent<GameObject>();
				//MeshFilter mf = temp.GetComponentInChildren<MeshFilter>();

				//Gives a 1 in 2 chance of items being placed
				//WOOHOO! Can change random values and dilute arrays to make chance of getting
				//certain items to spawn to be smaller
				int placeEntity = Random.Range(0,100);

				//TODO: Check neighbours of water tiles to put in topMeshes when it's a lake
				//TODO: Add in magical tiles for plot progression/flower specifics
				//TODO: Add in town
				//TODO: Add in actual terrain height generation

				/*Make castle and magic tiles spawn over on special island


					for(int i in range [0,5]){
						if(ELEVATION ABOVE A CERTAIN HEIGHT){
							mr.material = magic_purple_material;
						}
					} */

				//CAN include moisture in this as a component to seperate elevation from tile colour
				if(h.Elevation >= HeightMountain){
					mr.material = MatMountains;
					if(placeEntity>20){
						mf.mesh = TopMountain;
					}else{
						mf.mesh=null;
					}
				}else if(h.Elevation>=HeightSnow){
					mr.material=MatSnow;
					if(placeEntity>35){
						mf.mesh = TopSnow[Random.Range(0,TopSnow.Length-1)];
					}else{
						mf.mesh=null;
					}
				}else if(h.Elevation>=HeightForest){
					mr.material = MatForest;
					if(placeEntity>25){
						mf.mesh = TopForest[Random.Range(0,TopForest.Length-1)];
					}else{
						mf.mesh=null;
					}
				}else if(h.Elevation>= HeightGrass){
					mr.material = MatGrasslands;
					if(placeEntity>55){
						mf.mesh = TopPlains[Random.Range(0,TopPlains.Length-1)];
					}else{
						mf.mesh=null;
					}
				}else if(h.Elevation>= HeightFlat){
					mr.material = MatDesert;
					if(placeEntity>45){
						mf.mesh = TopDesert[Random.Range(0,TopDesert.Length-1)];
					}else{
						mf.mesh=null;
					}
				}else{
					mr.material = MatOcean;
					mf.mesh = MeshWater;
				}
				//mf.mesh=MeshWater;
			}
		}
	}

	public Hex[] GetHexesWithinRangeOf(Hex centreHex, int range){
		List<Hex> results = new List<Hex>();
		
		for(int dx=-range;dx<range-1;dx++){
			for(int dy=Mathf.Max(-range+1, -dx-range);dy<Mathf.Min(range,-dx+range-1);dy++){
				results.Add(GetHexAt(centreHex.Q +dx, centreHex.R +dy));
			}
		}
		return results.ToArray();
	}

	public void PlaceBoy(GameObject preFab, int q, int r){
		if(charToGameObject==null){
			charToGameObject = new Dictionary<Character, GameObject>();
		}
		if(player == null){
			player = new Character();
		}

		Hex a = GetHexAt(q,r);
		GameObject hex = hexToGameObjectMap[a];
		//Boy should have a class like Hex which contains its position and current tile etc info
		GameObject boy = (GameObject) Instantiate(Boy,hex.transform.position,
					Quaternion.identity,this.transform);

		player.SetHex(a);
		player.OnCharacterMoved += boy.GetComponent<CharacterView>().OnCharacterMoved;
		
		charToGameObject[player] = boy;

		PlaceHouse(q, r);
	}

	//IN MAP.UPDATE call PLAYER.UPDATE which is just if(moving) then MOVE or sumthn?
	public void FocusCameraOnBoy(){
		Vector3 pos = charToGameObject[player].transform.position;
		pos.y = 4; pos.z -=3;
		Camera.main.transform.position=pos;
	}

	//kinda works. Can fix later
	public void PlaceHouse(int col, int row){
		string objString = string.Format("HEX: " + col + ", " + row);
		GameObject obj = GameObject.Find(objString);

		Transform t = obj.GetComponentInChildren<Transform>().Find("Hex11").Find("HexTop");
		MeshFilter mf = t.GetComponentInChildren<MeshFilter>();

		mf.mesh = houseMesh;
	}
}
