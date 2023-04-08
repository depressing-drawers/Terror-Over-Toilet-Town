using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public StringBuilder builder = new StringBuilder();

	void SetupGame(){
		Gameboss.currentState = Gameboss.gameStates.loading;
		Gameboss.gameStage = Gameboss.stageOfGame.title;
	//	Cursor.visible = false;
	//	Cursor.lockState = CursorLockMode.Locked;

		Gameboss.gameControl = GetComponent<GameController> ();
		Gameboss.inputControl = GetComponent<InputController> ();
		Gameboss.stalls = GetComponent<StallSystem> ();
		Gameboss.movement = GetComponent<MovementCode> ();
		Gameboss.blur = GetComponent<ScreenBlur> ();
		Gameboss.rank = GetComponent<StallRankingSystem> ();

		StartNewGame ();
	}

	void StartNewGame(){

		//Gameboss.fader.SetupScreenFader ();

		Gameboss.ResetPlayerData ();
		Gameboss.stalls.SetupStalls ();
		Gameboss.blur.SetupBlurSystem ();

		StartCoroutine (StartTiming ());

	//	Gameboss.LoadPlayerOptions ();
	//	Gameboss.ApplyPlayerOptions ();
	}

	IEnumerator StartTiming(){
		float startDelay = 0.8f;

		float i = 0.0f;
		float rate = 1.0f / 420f;

		while (true) {
			i += Time.deltaTime * rate;
			if (Input.anyKeyDown) {break;}
			yield return null;
		}

		Gameboss.blur.ToggleBlur (false, true, true, startDelay);
		yield return new WaitForSeconds (startDelay);
		Gameboss.currentState = Gameboss.gameStates.ingame;
		Gameboss.gameStage = Gameboss.stageOfGame.game;
	}


	public void PrepareToShit(){
		Gameboss.gameStage = Gameboss.stageOfGame.shitCheck;
		Gameboss.isAnimating = true;
		Gameboss.blur.ToggleTextHolders (false);
		Gameboss.blur.ToggleTextHolders (true, 1);
		Gameboss.blur.ToggleBlur (true, true, true);
	}

	public void DenyTheShit(){
		Gameboss.gameStage = Gameboss.stageOfGame.game;
		Gameboss.isAnimating = true;
		Gameboss.blur.ToggleBlur (false, true, true,0.4f);
	}


	// Use this for initialization
	void Start () {
		SetupGame ();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
