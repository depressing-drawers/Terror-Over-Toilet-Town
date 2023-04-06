using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public StringBuilder builder = new StringBuilder();

	void SetupGame(){
		Gameboss.currentState = Gameboss.gameStates.loading;

	//	Cursor.visible = false;
	//	Cursor.lockState = CursorLockMode.Locked;

		Gameboss.gameControl = GetComponent<GameController> ();
		Gameboss.stalls = GetComponent<StallSystem> ();
		Gameboss.movement = GetComponent<MovementCode> ();


		StartNewGame ();
	}

	void StartNewGame(){

		//Gameboss.fader.SetupScreenFader ();
		Gameboss.ResetPlayerData ();
		Gameboss.stalls.SetupStalls ();

	//	Gameboss.LoadPlayerOptions ();
	//	Gameboss.ApplyPlayerOptions ();
	}

	// Use this for initialization
	void Start () {
		SetupGame ();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
