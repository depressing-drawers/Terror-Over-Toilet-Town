using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public StringBuilder builder = new StringBuilder();
	public bool standAloneBuild = false;

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
		Gameboss.sound = GetComponent<SoundMachine> ();

		StartNewGame ();
	}

	void StartNewGame(){

		//Gameboss.fader.SetupScreenFader ();

		Gameboss.ResetPlayerData ();
		Gameboss.sound.SetupSoundMachine ();

		Gameboss.stalls.SetupStalls ();
		Gameboss.blur.SetupBlurSystem ();

		StartCoroutine (StartTiming ());

	//	Gameboss.LoadPlayerOptions ();
	//	Gameboss.ApplyPlayerOptions ();
	}

	IEnumerator StartTiming(){
		Gameboss.blur.gameplayText.gameObject.SetActive (false);
		Gameboss.blur.textObjects [0].color = new Color (Gameboss.blur.textObjects [0].color.r, Gameboss.blur.textObjects [0].color.b, Gameboss.blur.textObjects [0].color.g, 0);
		Gameboss.blur.textObjects [1].color = new Color (Gameboss.blur.textObjects [1].color.r, Gameboss.blur.textObjects [1].color.b, Gameboss.blur.textObjects [1].color.g, 0);
		Gameboss.blur.textObjects [2].color = new Color (Gameboss.blur.textObjects [2].color.r, Gameboss.blur.textObjects [2].color.b, Gameboss.blur.textObjects [2].color.g, 0);
		Gameboss.blur.textObjects [3].color = new Color (Gameboss.blur.textObjects [3].color.r, Gameboss.blur.textObjects [3].color.b, Gameboss.blur.textObjects [3].color.g, 0);
		Gameboss.blur.textObjects [4].color = new Color (Gameboss.blur.textObjects [4].color.r, Gameboss.blur.textObjects [4].color.b, Gameboss.blur.textObjects [4].color.g, 0);


		Gameboss.blur.ToggleFade (false,2f);
		yield return new WaitForSeconds (0.6f);

		StartCoroutine (Gameboss.blur.SeperateTextTiming (true, 1f,Gameboss.blur.textObjects[0]));
		yield return new WaitForSeconds (0.3f);

		StartCoroutine (Gameboss.blur.SeperateTextTiming (true, 1f,Gameboss.blur.textObjects[1]));
		yield return new WaitForSeconds (0.3f);

		StartCoroutine (Gameboss.blur.SeperateTextTiming (true, 1f,Gameboss.blur.textObjects[2]));
		yield return new WaitForSeconds (0.3f);

		StartCoroutine (Gameboss.blur.SeperateTextTiming (true, 1f,Gameboss.blur.textObjects[3]));
		yield return new WaitForSeconds (0.3f);

		StartCoroutine (Gameboss.blur.SeperateTextTiming (true, 1f,Gameboss.blur.textObjects[4]));
		yield return new WaitForSeconds (0.3f);


		float startDelay = 0.8f;

		float i = 0.0f;
		float rate = 1.0f / 420f;

		while (true) {
			i += Time.deltaTime * rate;
			if (Input.anyKeyDown) {break;}
			yield return null;
		}
		Gameboss.sound.PlaySound ("start", 0.2f);

		Gameboss.blur.gameplayText.gameObject.SetActive (true);
		StartCoroutine (Gameboss.blur.SeperateTextTiming (true,startDelay,Gameboss.blur.gameplayText));

		Gameboss.blur.ToggleBlur (false, true, true, startDelay);
		yield return new WaitForSeconds (startDelay);
		Gameboss.RunStats.startTime = System.DateTime.Now;
		Gameboss.currentState = Gameboss.gameStates.ingame;
		Gameboss.gameStage = Gameboss.stageOfGame.game;
	}


	public void PrepareToShit(){
		Gameboss.sound.PlaySound ("rising", 0.3f);
		Gameboss.gameStage = Gameboss.stageOfGame.shitCheck;
		Gameboss.isAnimating = true;
		Gameboss.blur.ToggleTextHolders (false);
		Gameboss.blur.ToggleTextHolders (true, 1);
		Gameboss.blur.ToggleBlur (true, true, true);
	}

	public void DenyTheShit(){
		Gameboss.sound.PlaySound ("falling", 0.3f);
		Gameboss.gameStage = Gameboss.stageOfGame.game;
		Gameboss.isAnimating = true;
		Gameboss.blur.ToggleBlur (false, true, true,0.4f);
	}

	public void AuthoriseShit(){
		Gameboss.sound.PlaySound ("boom", 0.3f);
		Gameboss.RunStats.endTime = System.DateTime.Now;
		Gameboss.gameStage = Gameboss.stageOfGame.results;	
		Gameboss.isAnimating = true;
		Gameboss.currentState = Gameboss.gameStates.loading;
		Gameboss.blur.ToggleBlur (false, true, true,0.4f);
		Gameboss.blur.ToggleFade (true,0.4f);
		Gameboss.blur.endTexts[0].gameObject.SetActive (false);
		Gameboss.blur.endTexts[1].gameObject.SetActive (false);

		Gameboss.blur.ToggleTextHolders (true, 2);
		StartCoroutine (Gameboss.blur.SeperateTextTiming (false, 0.4f,Gameboss.blur.gameplayText));
		StartCoroutine (EndTextTiming ());
	}

	IEnumerator EndTextTiming(){
		StallRankingSystem.ResultsData newResults = Gameboss.rank.BuildResultsText (Gameboss.stalls.currentStalls [Gameboss.movement.playerCoord [0]]);
		yield return new WaitForSeconds (0.4f);
		Gameboss.sound.PlaySound ("finalImpact", 0.3f);

		Gameboss.blur.endTexts[0].gameObject.SetActive (true);
		Gameboss.blur.ToggleTextHolders (true, 2);
		StartCoroutine (Gameboss.blur.SeperateTextTiming (true, 0.4f,Gameboss.blur.endTexts[0]));
		yield return new WaitForSeconds (0.8f);
		Gameboss.blur.endTexts[1].gameObject.SetActive (true);
		StartCoroutine (Gameboss.blur.SeperateTextTiming (true, 0.4f,Gameboss.blur.endTexts[1]));
		yield return new WaitForSeconds (0.8f);
		Gameboss.blur.endTexts [2].text = newResults.resultsText;
		StartCoroutine (Gameboss.blur.SeperateTextTiming (true, 0.4f,Gameboss.blur.endTexts[2]));
		yield return new WaitForSeconds (0.4f);
		Gameboss.currentState = Gameboss.gameStates.ingame;

	}

	public void RestartGame(){
		Gameboss.currentState = Gameboss.gameStates.loading;
		StartCoroutine(RestartTiming());
	}

	IEnumerator RestartTiming(){
		StartCoroutine (Gameboss.blur.SeperateTextTiming (false, 0.4f,Gameboss.blur.endTexts[0]));
		StartCoroutine (Gameboss.blur.SeperateTextTiming (false, 0.4f,Gameboss.blur.endTexts[1]));
		StartCoroutine (Gameboss.blur.SeperateTextTiming (false, 0.4f,Gameboss.blur.endTexts[2]));
		Gameboss.sound.PlaySound ("falling", 0.3f);

		yield return new WaitForSeconds (0.6f);
		SceneManager.LoadScene ("mainLevel");
	}

	// Use this for initialization
	void Start () {
		SetupGame ();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
