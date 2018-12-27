using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Update_CurrentFunction = Update_DetectModeStart;
	}
	//Generic variables
	Vector3 lastMousePosition; //from Input.mousePosition
	//Camera drag variables

	Vector3 lastMouseGroundPlanePosition, hitPos;

	delegate void UpdateFunc();

	private Character characterSelected;
	UpdateFunc Update_CurrentFunction;
	
	//CLICK is reserved for moving to a tile/exploring the map/interacting with things
	//moving mouse around is for exploring the world???
	// Update is called once per frame

	void Update(){

		if(Input.GetKeyDown(KeyCode.Escape)){
			CancelUpdateFunction();
		}
		Update_CurrentFunction();

		Update_ScrollZoom(); //want this to run every frame regardless

		lastMousePosition = Input.mousePosition;
	}

	void CancelUpdateFunction(){
		Update_CurrentFunction = Update_DetectModeStart;
		//Also do any cleanup and UI associated stuff
	}

	//detects current mouse situation. Only runs if already not in a mode
	void Update_DetectModeStart(){
		if(Input.GetMouseButtonDown(0)){
			//left mouse button went down. Doesn't do anything by itself we need more context
			//if mouse is down and mouse starts moving then we start a drag
		
		}else if(Input.GetMouseButtonUp(0)){
			//TODO: Are we clicking on a hex or a unit

		}else if(Input.GetMouseButton(0) && Input.mousePosition != lastMousePosition){
			//TODO: consider adding in some pixel jitter threshold
			//left mouse is being held down and camera is being dragged
			Update_CurrentFunction = Update_CameraDrag;
			lastMouseGroundPlanePosition = CheckHitPos(Input.mousePosition);
			Update_CurrentFunction();

		}else if(Input.GetMouseButton(1)){//characterSelected!=null && ){
			//at the moment just want to move character if we are a right click
			Debug.Log("Updating character movement");
			Update_CharacterMovement();
		}
	}

	void Update_CharacterMovement(){
		//if this is called call updateCameraFollow or something so that the camera
		//follows the player as he moves
	
		GameObject hitObject = GetHexHit();
		if(hitObject==null){
			Debug.Log("Raycast did not hit a game object: UpdateCharacterMovement");
			return;
		}

		if(hitObject.GetComponentInChildren<HexComponent>()!=null){
			if(characterSelected==null){
				Map map = hitObject.GetComponentInChildren<HexComponent>().hexMap;
				characterSelected = map.player;
			}
			//initialise character
			characterSelected.moveToHex(hitObject.GetComponentInChildren<HexComponent>().hex);

		}
		//TODO: Change mesh of neighbours to be highlighted for selection
		//THESE SHOULD BE HIGHLIGHTED AT ALL TIMES

		CancelUpdateFunction();
		return;
	}

	void Update_CameraDrag () {
		//DO SOME CODE FOR IF IT HITS AND DOES NOT DRAG
		/*RaycastHit hitInfo;
		if( Physics.Raycast(mouseRay, out hitInfo) ) {
			GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject;
			if(ourHitObject.GetComponent<HexComponent>() != null) {
				// Ah! We are over a hex!
				Debug.Log("Raycast hit: " + ourHitObject.name );
				if(Input.GetMouseButtonDown(0)) {
					AlertSection.NewAlert("default","default",ourHitObject);
				}
			}
		}*/
		if(Input.GetMouseButtonUp(0)){
			CancelUpdateFunction();
			return;
		}

		Vector3 hitPos = CheckHitPos(Input.mousePosition);

		Vector3 diff = hitPos-lastMouseGroundPlanePosition;
		Camera.main.transform.Translate(diff, Space.World);

		lastMouseGroundPlanePosition=hitPos=CheckHitPos(Input.mousePosition);
	}

	void Update_ScrollZoom(){
		//FOR ZOOMING (Zoom to scrollwheel)
		float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
		float minHeight = 2;
		float maxHeight = 10;
		Vector3 hitPos = CheckHitPos(Input.mousePosition);
		
		if(Mathf.Abs(scrollAmount)>0.01f){

			//Move camera towards hitPos
			Vector3 dir = Camera.main.transform.position-hitPos;
			Vector3 p = Camera.main.transform.position;
			
			if(scrollAmount>0 || p.y<maxHeight-0.01f){
				Camera.main.transform.Translate(dir * scrollAmount, Space.World);
			}
			
			p = Camera.main.transform.position;
			if(p.y<minHeight){
				p.y=minHeight;
			}
			if(p.y>maxHeight){
				p.y=maxHeight;
			}
			//This SHOULD be fixing the "drag-through-the-ground" error
			Camera.main.transform.position = p;

			//Change camera angle when you get to the extremes
			float lowZoom = minHeight+2;
			float highZoom = maxHeight-5;

			//TODO: Fix bug where xooming in and pulling land makes you go through it
			//TODO: fix initial angle and y of camera so it doesn't look so weird
			Camera.main.transform.rotation=Quaternion.Euler(
					Mathf.Lerp(35, 90, (p.y/(maxHeight/1.5f))),
					Camera.main.transform.rotation.eulerAngles.y,
					Camera.main.transform.rotation.eulerAngles.z
				);
		}
	}

	Vector3 CheckHitPos(Vector3 mousePos){
		Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
		if(mouseRay.direction.y>=0){
			//Debug.LogError("why is mouse pointing up?");
			return Vector3.zero;
		}
		
		float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
		return mouseRay.origin + (mouseRay.direction * rayLength);
	}

	GameObject GetHexHit(){
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if( Physics.Raycast(mouseRay, out hitInfo) ) {
			GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject;
			return ourHitObject;
		}
		return null;
	}
}
