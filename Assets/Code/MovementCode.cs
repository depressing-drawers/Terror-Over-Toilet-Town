using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCode : MonoBehaviour {


	public Transform playerObject;
	public int[] playerCoord = { 0, 0 };//x,z
	private float playerHeight = 1.5f;
	private float widthValue = 7.5f;
	public bool facingForward = true;

	void InputLogic(){
		if (Input.GetKeyDown (KeyCode.W)) {
			MovePlayerPosition (true);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			MovePlayerPosition (false);
		}
		if (Input.GetKeyDown (KeyCode.D)) {
		}
		if (Input.GetKeyDown (KeyCode.A)) {
		}
	}



	void MovePlayerPosition(bool forward){
		int moveIncrement = 1;
		if (!forward || !facingForward) 						{moveIncrement *= -1; 	}
		if (!forward && !facingForward && playerCoord [1] == 2) {moveIncrement = 1;		}
		if (!forward && !facingForward && playerCoord [1] == 3) {moveIncrement = 1;		}
		int expectedValue = playerCoord [1] + moveIncrement;
		if (expectedValue > -1 && expectedValue < 4) {
			playerCoord [1] = expectedValue;
			MovePlayer ();
			RotatePlayer (forward);
		}
	}

	void MovePlayer(){
		playerObject.position = new Vector3 (playerCoord [0] * widthValue, playerHeight, playerCoord [1] * widthValue);
	}

	void RotatePlayer(bool moveForward){
		Vector3 targetRotation = Vector3.zero;
		switch (playerCoord [1]) {
	//	case(0):targetRotation = Vector3.zero;break;
	//	case(1):targetRotation = Vector3.zero;break;
		case(2):if(!facingForward){targetRotation = new Vector3(0,180,0);facingForward=false;}break;
		case(3):targetRotation = new Vector3(0,180,0);facingForward=false;break;
		default:facingForward = true;break;
			
		}
		playerObject.eulerAngles = targetRotation;
	}

	void ChangeStallPosition(){
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		InputLogic ();
	}
}
