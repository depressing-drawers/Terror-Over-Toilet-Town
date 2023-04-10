using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	void InputLogic(){

		if(Gameboss.gameStage == Gameboss.stageOfGame.game){
			
		if (Input.GetKeyDown (KeyCode.W)) 		{Gameboss.movement.MovePlayerPosition (true);	}
		if (Input.GetKeyDown (KeyCode.S)) 		{Gameboss.movement.MovePlayerPosition (false);	}
		if (Input.GetKeyDown (KeyCode.D)) 		{Gameboss.movement.MovePlayerLocation (true);	}
		if (Input.GetKeyDown (KeyCode.A)) 		{Gameboss.movement.MovePlayerLocation (false);	}
		if (Input.GetKeyDown (KeyCode.Space)) 	{ContextSensitiveInput ();						}
		if (Input.GetKeyDown (KeyCode.Return))	{Gameboss.gameControl.PrepareToShit ();			}

		}else if(Gameboss.gameStage == Gameboss.stageOfGame.shitCheck){
			if (Input.GetKeyDown (KeyCode.N)) 	{Gameboss.gameControl.DenyTheShit ();			}
			if (Input.GetKeyDown (KeyCode.Y)) 	{Gameboss.gameControl.AuthoriseShit ();			}

		}
	}


	void ContextSensitiveInput(){
		switch (Gameboss.movement.playerCoord [1]) {
		case(0):
		case(1):			
			Gameboss.stalls.ToggleDoor ();break;
		case(2):
		case(3):
			if (!Gameboss.movement.facingForward) {
				Gameboss.stalls.ToggleDoor ();
			} else {
				Gameboss.stalls.ToggleLid ();
			}break;
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Gameboss.currentState == Gameboss.gameStates.ingame && !Gameboss.isAnimating) {
			InputLogic ();
		}
	}
}
