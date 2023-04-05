using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallSystem : MonoBehaviour {

	public GameObject stallBasis;
	public Transform stallEnd;
	private float ruinationLimit = 0.8f;

	public class StallData{
		public Dictionary<string, StallComponent> stallContents;
		public GameObject stallProp;
	}


	public class StallComponent	{
		public string name;
		public float filth;		//0.0 - 1.0
		public float ruination;	//0.0 - 1.0

	}

	public List<StallData> currentStalls = new List<StallData>();

	public void SetupStalls(){
		for (int i = 0; i < 11; i++) {
			currentStalls.Add(CreateNewStall ());
		}
		GenerateStalls ();
	}

	void GenerateStalls(){
		for (int i = 0; i < currentStalls.Count; i++) {
			if (currentStalls [i].stallProp == null) {				
				GameObject stallClone = (GameObject)Instantiate (stallBasis, null);
				stallClone.transform.position = new Vector3 (i * 7.5f, 0, 7.5f);
				currentStalls [i].stallProp = stallClone;
				ApplyStallCondition (currentStalls [i]);
			}
		}
		stallEnd.position = new Vector3 ((currentStalls.Count - 1) * 7.5f, 0, 7.5f);
	}


	void ApplyStallCondition(StallData stall){

	//	for (int i = 0; i < playerParts.Length; i++){
	//		playerSplatMat.Add(playerParts[i].GetComponent<Renderer>().material);
	//		playerSplatMat[i].SetFloat("_OcclusionStrength",0);
	//	}


		foreach (KeyValuePair<string, StallComponent> stallPart in stall.stallContents) {
			Transform foundObject = stall.stallProp.transform.Find (stallPart.Value.name);
			if (foundObject != null) {
				Renderer foundRenderer = foundObject.gameObject.GetComponent<Renderer> ();

					if (stallPart.Value.ruination > ruinationLimit) {
						foundObject.gameObject.SetActive (false);
				} else if (foundRenderer != null) {
						foundObject.gameObject.GetComponent<Renderer> ().material.SetFloat ("_OcclusionStrength", stallPart.Value.filth);

				}
			}
		}
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
		foreach (KeyValuePair<string, StallComponent> stallPart in stall.stallContents) {
			
			switch (stallPart.Value.name) {
			case("ceiling"):
			case("floor"):
				if (stallPart.Value.ruination > ruinationLimit) {
					stallPart.Value.ruination = 0.7f;
				}
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
		SetupStalls ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
