using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gameboss : MonoBehaviour {


	public static GameController gameControl;
	public static InputController inputControl;
	public static MovementCode movement;
	public static StallSystem stalls;
	public static ScreenBlur blur;
	public static StallRankingSystem rank;
	public static SoundMachine sound;

	public static class GameOptions{			
		//audio
		public static float sfxVolume = 1f;
		public static float musicVolume = 1f;
		public static float ambientVolume = 1f;
		public static float masterVolume = 1f;
		//video
		public static int  resolutionRef = 0;
		public static bool fullscreen = false;
		public static bool screenEffects = false;
		//dev
		public static bool showDebug = true;

	} 


	public enum gameStates{
		ingame,
		paused,
		loading
	}

	public enum stageOfGame{
		title,
		game,
		shitCheck,
		results
	}

	public static stageOfGame gameStage = stageOfGame.title;
	public static gameStates currentState = gameStates.loading;
	public static bool isAnimating = false;

	public static class PlayerData{
		public static bool gameComplete = false;
	}

	public static class RunStats{
		public static System.DateTime startTime;
		public static System.DateTime endTime;
	}

	public static void ResetPlayerData(){
		PlayerData.gameComplete = false;

		RunStats.startTime = System.DateTime.MinValue;
		RunStats.endTime = System.DateTime.MinValue;
	}

	public static TItem GetRandom<TItem>(TItem[] array){
		return array[Random.Range(0, array.Length)];
	}

	public static IEnumerator CameraShakeTiming(){
		Vector3 originalPosition = Camera.main.transform.position;
		for (int i = 0; i<5; i++){
			Vector3 pos = Random.insideUnitSphere * 0.05f;
			Camera.main.transform.position += new Vector3(pos.x, pos.y, 0);

			yield return new WaitForSeconds(0.025f);
			Camera.main.transform.position = originalPosition;
			yield return new WaitForSeconds(0.025f);

		}
		Camera.main.transform.position = originalPosition;
	}

	public static IEnumerator ShakeObject(Transform objectToShake){
		Vector3 originalPosition = objectToShake.localPosition;
		Vector3 originalRotation = objectToShake.localEulerAngles;

		for (int i = 0; i<2; i++){
			Vector3 pos = Random.insideUnitSphere * 0.2f;
			objectToShake.localPosition += new Vector3(pos.x, 0, pos.z);
		//	objectToShake.localEulerAngles -= new Vector3(0,0,Random.Range (2, 6));

			yield return new WaitForSeconds(0.02f);
			objectToShake.localPosition = originalPosition;
		//	objectToShake.localEulerAngles += new Vector3(0,0,Random.Range (2, 6));

			yield return new WaitForSeconds(0.02f);

		}
		objectToShake.localPosition = originalPosition;
		objectToShake.localEulerAngles = originalRotation;
	}





	public static void LoadPlayerOptions(){
		if (PlayerPrefs.HasKey("masterVolume")) {

			GameOptions.masterVolume = 		PlayerPrefs.GetFloat ("masterVolume");
			GameOptions.sfxVolume = 		PlayerPrefs.GetFloat ("sfxVolume");
			GameOptions.musicVolume = 		PlayerPrefs.GetFloat ("musicVolume");
			GameOptions.ambientVolume = 	PlayerPrefs.GetFloat ("ambientVolume");

			GameOptions.resolutionRef = 	PlayerPrefs.GetInt ("resolution");
			GameOptions.fullscreen = 		(PlayerPrefs.GetString ("fullscreen")=="True");
			GameOptions.screenEffects = 	(PlayerPrefs.GetString ("screenEffects")=="True");

		} else {
			SavePlayerOptions ();
		}
	}

	public static void SavePlayerOptions(){

		PlayerPrefs.SetFloat ("masterVolume", 	GameOptions.masterVolume);
		PlayerPrefs.SetFloat ("sfxVolume", 		GameOptions.sfxVolume);
		PlayerPrefs.SetFloat ("musicVolume", 	GameOptions.musicVolume);
		PlayerPrefs.SetFloat ("ambientVolume", 	GameOptions.ambientVolume);

		PlayerPrefs.SetInt 	  ("resolution", 	GameOptions.resolutionRef);
		PlayerPrefs.SetString ("fullscreen", 	GameOptions.fullscreen.ToString());
		PlayerPrefs.SetString ("screenEffects", GameOptions.screenEffects.ToString());

		PlayerPrefs.Save ();
	}


	public static void ApplyPlayerOptions(){
	//	sound.musicPlayer.volume = (0.5f * Gameboss.GameOptions.musicVolume) * Gameboss.GameOptions.masterVolume;
	//	Screen.SetResolution (
	//		Gameboss.options.resolutions [GameOptions.resolutionRef].width,
	//		Gameboss.options.resolutions [GameOptions.resolutionRef].height,
	//		GameOptions.fullscreen);
	}



	public static void ChangeAudioSetting(bool increase, int settingToChange){

		// MASTER
		// MUSIC
		// SFX
		// AMBIENT
		// BACK

		float targetSetting = 0;
		float expectedValue = 0;

		switch (settingToChange) {
			case(0):targetSetting = GameOptions.masterVolume;break;
			case(1):targetSetting = GameOptions.musicVolume;break;
			case(2):targetSetting = GameOptions.sfxVolume;break;
			case(3):targetSetting = GameOptions.ambientVolume;break;				
		}

		float changeValue = 0.1f;
		if (!increase) {changeValue = -0.1f;}

		expectedValue = targetSetting + changeValue;

		if (expectedValue < 0) {expectedValue = 0;}
		if (expectedValue > 1) {expectedValue = 1;}


		switch (settingToChange) {
		case(0):GameOptions.masterVolume 	= expectedValue;break;
		case(1):GameOptions.musicVolume 	= expectedValue;break;
		case(2):GameOptions.sfxVolume 		= expectedValue;break;
		case(3):GameOptions.ambientVolume 	= expectedValue;break;				
		}



		SavePlayerOptions ();
		ApplyPlayerOptions ();
	}



	public static void ChangeVideoSetting(bool increase, int settingToChange){
		
		//	VIDEO RESOLUTION
		//	FULL SCREEN
		//	EFFECTS
		//	APPLY
		//	CANCEL

	//	int expectedValue = 0;

	//	switch (settingToChange) {
	//	case(0):
	//		int change = 1; if(!increase){change = -1;}
	//		expectedValue = change + options.uncommitedResolution;
	//		if (expectedValue < 0) {expectedValue = 0;}
	//		if (expectedValue > options.resolutions.Count) {expectedValue = options.resolutions.Count-1;}
	//		options.uncommitedResolution = expectedValue;

	//		break;

	//	case(1):GameOptions.fullscreen = !GameOptions.fullscreen		;break;
	//	case(2):GameOptions.screenEffects = !GameOptions.screenEffects	;break;		
	//	}
	}






	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
