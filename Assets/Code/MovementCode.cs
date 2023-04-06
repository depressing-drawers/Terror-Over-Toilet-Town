using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCode : MonoBehaviour {


	public Transform playerObject;
	public int[] playerCoord = { 0, 0 };//x,z
	private float playerHeight = 1.5f;
	private float widthValue = 7.5f;
	public bool facingForward = true;

	public void MovePlayerPosition(bool forward){
		int moveIncrement = 1;
		if (!forward || !facingForward) 						{moveIncrement *= -1; 	}
		if (!forward && !facingForward && playerCoord [1] == 2) {moveIncrement = 1;		}
		if (!forward && !facingForward && playerCoord [1] == 3) {moveIncrement = 1;		}
		int expectedValue = playerCoord [1] + moveIncrement;
		if (expectedValue > -1 && expectedValue < 4 && PlayerAllowedToMove(moveIncrement > 0)) {
			playerCoord [1] = expectedValue;
			MovePlayer ();
			RotatePlayer (forward);
		}
	}


	bool PlayerAllowedToMove(bool movingForward){
		bool allowed = true;
		if(playerCoord[1] == 1 && facingForward && movingForward &&
			
			(Gameboss.stalls.currentStalls[playerCoord[0]].doorState == StallSystem.lidState.closed ||
			 Gameboss.stalls.currentStalls[playerCoord[0]].doorState == StallSystem.lidState.ajar)){allowed = false;}
		
		if(playerCoord[1] == 2 && !facingForward && !movingForward &&
			
			(Gameboss.stalls.currentStalls[playerCoord[0]].doorState == StallSystem.lidState.closed||
			 Gameboss.stalls.currentStalls[playerCoord[0]].doorState == StallSystem.lidState.ajar)){allowed = false;}

		return allowed;
	}


	public void MovePlayerLocation(bool right){
		if (playerCoord [1] == 0) {
			int moveIncrement = 1;
			if (!right) {moveIncrement *= -1;}

			int expectedValue = playerCoord [0] + moveIncrement;
			if (expectedValue > -1 && expectedValue < Gameboss.stalls.currentStalls.Count) {
				playerCoord [0] = expectedValue;
				MovePlayer ();
			}
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


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
