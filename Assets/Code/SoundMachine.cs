using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMachine : MonoBehaviour {

	public AudioClip[] clips;
	public AudioSource soundPlayer;
	public AudioSource ambientPlayer1;
	public AudioSource ambientPlayer2;


	public Dictionary<string, SoundData> soundLibrary = new Dictionary<string, SoundData>();

	public class SoundData{
		public int soundRef;
		public string soundKey;
	}

	public void SetupSoundMachine(){
		soundLibrary.Clear ();
		//soundPlayer.volume = (0.5f * Gameboss.GameOptions.musicVolume) * Gameboss.GameOptions.masterVolume;
		//ambientPlayer1.volume = (0.5f * Gameboss.GameOptions.musicVolume) * Gameboss.GameOptions.masterVolume;
		//ambientPlayer2.volume = (0.5f * Gameboss.GameOptions.musicVolume) * Gameboss.GameOptions.masterVolume;

		CreateSoundEntry (0, "step");
		CreateSoundEntry (1, "wetStep");
		CreateSoundEntry (2, "woosh");
		CreateSoundEntry (3, "flush");
		CreateSoundEntry (4, "creak");
		CreateSoundEntry (5, "rattle");
		CreateSoundEntry (6, "doorClunk");
		CreateSoundEntry (7, "doorMove");
		CreateSoundEntry (8, "doorLock");
		CreateSoundEntry (9, "lid");


	}

	void CreateSoundEntry(int soundRef, string soundKey){
		SoundData soundClone = new SoundData ();
		soundClone.soundRef = soundRef;
		soundClone.soundKey = soundKey;
		soundLibrary.Add (soundClone.soundKey,soundClone);
	}


	public void PlaySound(string clipToPlay, float soundVolume, float forcedPitch = -1f){
		if (forcedPitch != -1) {
			soundPlayer.pitch = forcedPitch;
				} else {
			soundPlayer.pitch = 1 + (Random.Range (-2, 3)*0.1f);
		}
		soundPlayer.PlayOneShot (clips[soundLibrary[clipToPlay].soundRef], 
			(soundVolume * Gameboss.GameOptions.sfxVolume)*Gameboss.GameOptions.masterVolume);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
