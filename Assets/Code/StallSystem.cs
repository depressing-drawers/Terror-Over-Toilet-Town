﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallSystem : MonoBehaviour {

	public GameObject stallBasis;
	public Transform stallEnd;
	private float ruinationLimit = 0.8f;

	public class StallData{
		public Dictionary<string, StallComponent> stallContents;
		public GameObject stallProp;
		public TextMesh stallNumber;
		public lidState doorState;
		public Transform doorObject;
		public bool isLocked;
	}

	public class StallComponent	{
		public string name;
		public float filth;		//0.0 - 1.0
		public float ruination;	//0.0 - 1.0
	}

	public enum lidState{
		open,
		closed,
		missing,
		ajar
	}

	private Vector3[] doorAngles = new Vector3[]{
		new Vector3(0,-80,0),
		new Vector3(0,0,0),
		new Vector3(0,0,0),
		new Vector3(0,-15,0)
	};

	public List<StallData> currentStalls = new List<StallData>();



	public void ToggleDoor(){
		StallData currentStall = currentStalls [Gameboss.movement.playerCoord [0]];
		if (!currentStall.isLocked) {

			lidState targetState = lidState.open;
			switch (currentStall.doorState) {
			case(lidState.ajar):
			case(lidState.closed):
				targetState = lidState.open;break;
			case(lidState.open):targetState = lidState.closed;break;
			}
			currentStall.doorState = targetState;
			ApplyDoorState (currentStall);
		}
	}


	public void SetupStalls(){
		for (int i = 0; i < 11; i++) {
			currentStalls.Add(CreateNewStall ());
		}
		GenerateStalls ();
	}

	void GenerateStalls(){
		for (int i = 0; i < currentStalls.Count; i++) {
			if (currentStalls [i].stallProp == null) {				
				GameObject stallClone 						= (GameObject)Instantiate (stallBasis, null);
				stallClone.transform.position 				= new Vector3 (i * 7.5f, 0, 7.5f);
				currentStalls [i].stallProp 				= stallClone;
				currentStalls [i].stallNumber 				= stallClone.transform.Find ("stall_number").GetComponent<TextMesh>();
				currentStalls [i].stallNumber.text 			= i.ToString ();
				ApplyStallCondition (currentStalls [i]);
			}
		}
		stallEnd.position = new Vector3 ((currentStalls.Count - 1) * 7.5f, 0, 7.5f);
	}

	void ApplyDoorState(StallData stall){
		stall.doorObject.localEulerAngles = doorAngles[(int)stall.doorState];
		if (stall.doorState == lidState.ajar) {
			stall.doorObject.localEulerAngles += new Vector3 (0, Random.Range (-15, 16), 0);		
		}
	}


	void ApplyStallCondition(StallData stall){
		
		for (int i = 0; i < stall.stallProp.transform.childCount; i++) {
			string childName = stall.stallProp.transform.GetChild (i).name;

			if (childName == "door_axis") { stall.doorObject = stall.stallProp.transform.GetChild (i);	}

			if (stall.stallContents.ContainsKey (childName)) {
				StallComponent foundComponent = stall.stallContents [childName];
				Renderer childRenderer = stall.stallProp.transform.GetChild (i).GetComponent<Renderer>();

				if (childRenderer != null) {
					if (foundComponent.ruination > ruinationLimit) {
						childRenderer.gameObject.SetActive (false);
							} else {
						childRenderer.material.SetFloat ("_OcclusionStrength", foundComponent.filth);
								}
							}			
						}


			if (stall.stallProp.transform.GetChild (i).childCount > 0) {
				for (int j = 0; j < stall.stallProp.transform.GetChild (i).childCount; j++) {
					string subChildName = stall.stallProp.transform.GetChild (i).GetChild (j).name;
					if (stall.stallContents.ContainsKey (subChildName)) {
						StallComponent foundComponent = stall.stallContents [subChildName];
						Renderer subChildRenderer = stall.stallProp.transform.GetChild (i).GetChild(j).GetComponent<Renderer>();

						if (subChildRenderer != null) {
							if (foundComponent.ruination > ruinationLimit) {
								subChildRenderer.gameObject.SetActive (false);
							} else {
								subChildRenderer.material.SetFloat ("_OcclusionStrength", foundComponent.filth);
							}
						}			
					}
				}
			}		
		}
		ApplyDoorState (stall);
	}




	StallData CreateNewStall(){
		StallData newStall = new StallData ();
		newStall.stallContents = new Dictionary<string, StallComponent> ();
		CreateStallComponent ("right_wall", newStall);
		CreateStallComponent ("door", newStall);
		CreateStallComponent ("exterior_handle", newStall);
		CreateStallComponent ("occupied_sign", newStall);
		CreateStallComponent ("interior_handle", newStall);
		CreateStallComponent ("lock", newStall);
		CreateStallComponent ("floor", newStall);
		CreateStallComponent ("ceiling", newStall);
		CreateStallComponent ("back_wall", newStall);
		CreateStallComponent ("dispenser", newStall);
		CreateStallComponent ("paper", newStall);
		CreateStallComponent ("bowl", newStall);
		CreateStallComponent ("cistern", newStall);
		CreateStallComponent ("cistern_lid", newStall);
		CreateStallComponent ("flusher", newStall);
		CreateStallComponent ("toilet_lid", newStall);
		CreateStallComponent ("toilet_seat", newStall);

		CleanupStallData (newStall);
		return newStall;
	}

	void CleanupStallData(StallData stall){

		//decide door state
		if (stall.stallContents ["door"].ruination > ruinationLimit) {
			stall.doorState = lidState.missing;
		} else {
			int doorchoice = Random.Range (0, 3);
			switch (doorchoice) {
				case(0):stall.doorState = lidState.open		;break;
				case(1):stall.doorState = lidState.closed	;break;
				case(2):stall.doorState = lidState.ajar		;break;				
			}
			if (stall.doorState == lidState.closed) {stall.isLocked = (Random.Range (0, 5) == 0);}
		}



		foreach (KeyValuePair<string, StallComponent> stallPart in stall.stallContents) {
			
			switch (stallPart.Value.name) {
			case("back_wall"):				
			case("ceiling"):
			case("floor"):
				if (stallPart.Value.ruination > ruinationLimit) {stallPart.Value.ruination = 0.7f;}	
				if (stallPart.Value.name == "back_wall") 		{stallPart.Value.filth = stall.stallContents ["ceiling"].filth;}
				break;

			case("bowl"):
				if (stallPart.Value.ruination > ruinationLimit) {
					stall.stallContents ["cistern"].ruination = 1.0f;
					stall.stallContents ["cistern_lid"].ruination = 1.0f;
					stall.stallContents ["toilet_lid"].ruination = 1.0f;
					stall.stallContents ["toilet_seat"].ruination = 1.0f;
					stall.stallContents ["flusher"].ruination = 1.0f;
				}break;
			case("door"):
				if (stallPart.Value.ruination > ruinationLimit) {
					stall.stallContents ["exterior_handle"].ruination = 1.0f;
					stall.stallContents ["occupied_sign"].ruination = 1.0f;
					stall.stallContents ["interior_handle"].ruination = 1.0f;
					stall.stallContents ["lock"].ruination = 1.0f;
				}break;
			case("right_wall"):
				stallPart.Value.filth = stall.stallContents ["ceiling"].filth;	
				if (stallPart.Value.ruination > ruinationLimit) {
					stall.stallContents ["paper"].ruination = 1.0f;
					stall.stallContents ["dispenser"].ruination = 1.0f;
				}break;
			case("dispenser"):
				if (stallPart.Value.ruination > ruinationLimit) {
					stall.stallContents ["paper"].ruination = 1.0f;
				}break;
			case("cistern"):
				if (stallPart.Value.ruination > ruinationLimit) {
					stall.stallContents ["cistern_lid"].ruination = 1.0f;
					stall.stallContents ["flusher"].ruination = 1.0f;
				}
				if (stallPart.Value.filth > stall.stallContents ["cistern_lid"].filth) {
					stall.stallContents ["cistern_lid"].filth = stallPart.Value.filth;
				}break;
			}
		}
	}


	void CreateStallComponent(string componentName, StallData parentStallData){
		StallComponent newComponent = new StallComponent ();
		newComponent.name = componentName;
		newComponent.filth 	   = Random.Range (0, 11) * 0.1f;
		newComponent.ruination = Random.Range (0, 11) * 0.1f;
		parentStallData.stallContents.Add (newComponent.name, newComponent);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
