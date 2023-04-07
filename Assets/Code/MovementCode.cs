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
			Gameboss.isAnimating = true;
			StartCoroutine(MovementTiming(new int[]{playerCoord[0]+0, expectedValue},true));
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
			//	playerCoord [0] = expectedValue;
			//	MovePlayer ();
				Gameboss.isAnimating = true;
				StartCoroutine(MovementTiming(new int[]{expectedValue, playerCoord[1]+0}));
			}
		}
	}


	IEnumerator MovementTiming(int[] targetCoord, bool doRotation = false){
		float i = 0.0f;
		float rate = 1.0f / 0.19f;

		Vector3 startPos 	= new Vector3 (playerCoord [0] * widthValue, playerHeight, playerCoord [1] * widthValue);
		Vector3 endPos		= new Vector3 (targetCoord [0] * widthValue, playerHeight, targetCoord [1] * widthValue);
		Vector3[] rotationEulers = ReturnTargetEulers (targetCoord [1]);	


		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			playerObject.position = Vector3.Lerp (startPos, endPos, i);
			if (doRotation) {
				playerObject.eulerAngles = Vector3.Lerp (rotationEulers [0], rotationEulers [1], i);
			}
			yield return null;
		}
		playerCoord [0] = targetCoord [0] + 0;
		playerCoord [1] = targetCoord [1] + 0;

		playerObject.position = endPos;
		if (doRotation) {playerObject.eulerAngles = rotationEulers [1];}
			
		Gameboss.isAnimating = false;
	}


	Vector3[] ReturnTargetEulers(int targetCoord){
		Vector3[] returnedEulers = new Vector3[2];
		Vector3 currentEuler = new Vector3(0,playerObject.transform.eulerAngles.y + 0, 0);
		Vector3 targetEuler = Vector3.zero;
		switch (targetCoord) {
		//	case(0):targetRotation = Vector3.zero;break;
		//	case(1):targetRotation = Vector3.zero;break;
		case(2):if(!facingForward){targetEuler = new Vector3(0,180,0);facingForward=false;}break;
		case(3):targetEuler = new Vector3(0,180,0);facingForward=false;break;
		default:facingForward = true;break;
		}
		returnedEulers [0] = currentEuler;
		returnedEulers [1] = targetEuler;

		return returnedEulers;
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
