using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallSystem : MonoBehaviour {


	public class StallData{
		public Dictionary<string, StallComponent> stallContents;
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
		CreateStallComponent ("paper_dispenser", newStall);
		CreateStallComponent ("paper", newStall);
		CreateStallComponent ("bowl", newStall);
		CreateStallComponent ("cistern", newStall);
		CreateStallComponent ("cistern_lid", newStall);
		CreateStallComponent ("flusher", newStall);
		CreateStallComponent ("toilet_lid", newStall);
		CreateStallComponent ("toilet_seat", newStall);
		return newStall;
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
