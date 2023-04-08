using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallRankingSystem : MonoBehaviour {


	public float ReturnStallScore(StallSystem.StallData stall){
		float stallScore = 10000;
		float minusScore = 0;
		foreach (KeyValuePair<string,StallSystem.StallComponent> stallPart in stall.stallContents) {
			minusScore = (stallPart.Value.filth * 10) * stallPart.Value.multiplier;
			minusScore += (stallPart.Value.ruination * 10) * stallPart.Value.multiplier;
		}
		return stallScore - minusScore;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
