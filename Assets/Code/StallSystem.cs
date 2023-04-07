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
		public TextMesh stallNumber;
		public lidState doorState;
		public lidState lidState;
		public Transform doorObject;
		public Transform lidObject;
		public Transform signObject;
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

	private Vector3[] lidAngles = new Vector3[]{
		new Vector3(81,0,0),
		new Vector3(0,0,0),
		new Vector3(0,0,0),
		new Vector3(35,0,0)
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
			Gameboss.isAnimating = true;
			StartCoroutine (LidTiming (currentStall, new lidState[]{ currentStall.doorState, targetState }, true));
	//		currentStall.doorState = targetState;
	//		ApplyDoorState (currentStall);
		}
	}


	public void ToggleLid(){
		StallData currentStall = currentStalls [Gameboss.movement.playerCoord [0]];

			lidState targetState = lidState.open;
			switch (currentStall.lidState) {
			case(lidState.ajar):
			case(lidState.closed):
				targetState = lidState.open;break;
			case(lidState.open):targetState = lidState.closed;break;
			}

			Gameboss.isAnimating = true;
			StartCoroutine (LidTiming (currentStall, new lidState[]{ currentStall.lidState, targetState }, false));
	//		currentStall.lidState = targetState;
	//		ApplyLidState (currentStall);
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
	}

	void ApplyLidState(StallData stall){
		stall.lidObject.localEulerAngles = lidAngles[(int)stall.lidState];
	}






	IEnumerator LidTiming(StallData currentStall, lidState[] lidTargets, bool isDoor){//if change isDoor also need to change ReturnAppropriateLidEulers
		float i = 0.0f;
		float rate = 1.0f / 0.15f;
		Vector3[] eulerTargets = ReturnAppropriateLidEulers(lidTargets,isDoor);
		Transform 		rotationObject = currentStall.doorObject;
		if (!isDoor) {	rotationObject = currentStall.lidObject;	}

		while (i < 1.0f) {
			i += Time.deltaTime * rate;
				rotationObject.localEulerAngles = Vector3.Lerp (eulerTargets[0], eulerTargets[1], i);
			yield return null;
		}
		rotationObject.localEulerAngles = eulerTargets[1];

		if (isDoor) {
			currentStall.doorState = lidTargets[1];
			ApplyDoorState (currentStall);
		} else {
			currentStall.lidState = lidTargets[1];
			ApplyLidState (currentStall);
		}

		Gameboss.isAnimating = false;
	}


	Vector3[] ReturnAppropriateLidEulers(lidState[] lidTargets, bool isDoor){
		Vector3[] returnedEulers = new Vector3[2];
		Vector3[] 		appropriateAngles = doorAngles;
		if (!isDoor) {	appropriateAngles = lidAngles;	}
				
		returnedEulers[0] = appropriateAngles[(int)lidTargets[0]];
		returnedEulers[1] = appropriateAngles[(int)lidTargets[1]];
		return returnedEulers;
	}





	void ApplyLockState(StallData stall){
		if (stall.isLocked) {
			stall.signObject.localEulerAngles = new Vector3 (180, 90, 90);
		} else {
			stall.signObject.localEulerAngles = new Vector3 (0, 90, 90);
		}
	}

	void ApplyStallCondition(StallData stall){
		
		for (int i = 0; i < stall.stallProp.transform.childCount; i++) {
			string childName = stall.stallProp.transform.GetChild (i).name;

			if (childName == "door_axis") 		{ stall.doorObject 	= stall.stallProp.transform.GetChild (i);				}
			if (childName == "toilet_lid_axis") { stall.lidObject 	= stall.stallProp.transform.GetChild (i);				}


			if (stall.stallContents.ContainsKey (childName)) {
				StallComponent foundComponent = stall.stallContents [childName];
				Renderer childRenderer = stall.stallProp.transform.GetChild (i).GetComponent<Renderer>();

					if (foundComponent.ruination > ruinationLimit) {
						stall.stallProp.transform.GetChild (i).gameObject.SetActive (false);
							} else {
					if (childRenderer != null) {childRenderer.material.SetFloat ("_OcclusionStrength", foundComponent.filth);}
						}
					}

			if (stall.stallProp.transform.GetChild (i).childCount > 0) {
				for (int j = 0; j < stall.stallProp.transform.GetChild (i).childCount; j++) {
					string subChildName = stall.stallProp.transform.GetChild (i).GetChild (j).name;

					if (subChildName == "occupied_sign") 	{ stall.signObject 	= stall.stallProp.transform.GetChild (i).GetChild(j).Find("sign");	}


					if (stall.stallContents.ContainsKey (subChildName)) {
						StallComponent foundComponent = stall.stallContents [subChildName];
						Renderer subChildRenderer = stall.stallProp.transform.GetChild (i).GetChild(j).GetComponent<Renderer>();


							if (foundComponent.ruination > ruinationLimit) {
								stall.stallProp.transform.GetChild (i).GetChild(j).gameObject.SetActive (false);
							} else {
								if (subChildRenderer != null) {subChildRenderer.material.SetFloat ("_OcclusionStrength", foundComponent.filth);
							}
						}			
					}
				}
			}		
		}
		ApplyDoorState (stall);
		ApplyLidState  (stall);
		ApplyLockState (stall);
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
			if (stall.doorState == lidState.closed && stall.stallContents["door"].ruination < ruinationLimit &&
				stall.stallContents["lock"].ruination < ruinationLimit) {stall.isLocked = (Random.Range (0, 5) == 0);}
		}


		if (stall.stallContents ["toilet_lid"].ruination > ruinationLimit) {
			stall.lidState = lidState.missing;
		} else {
			int lidChoice = Random.Range (0, 2);
			switch (lidChoice) {
			case(0):stall.lidState = lidState.open		;break;
			case(1):stall.lidState = lidState.closed	;break;
			case(2):stall.lidState = lidState.ajar		;break;				
			}
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
