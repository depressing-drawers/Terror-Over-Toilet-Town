using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StallRankingSystem : MonoBehaviour {

	public StringBuilder builder = new StringBuilder();
	public class ResultsData{
		public string resultsText;
		public float overallScore;
	}

	private string[] resultsText = new string[]{
		/*0*/	"֍\tYou shit straight on the floor like a filthy animal, without even going into a stall\n\r",
		/*1*/	"֍\tYou shit straight on the floor like a dirty ape, but at least you did it in a stall\n\r",
		/*2*/	"֍\tYou forgot to put the toilet lid up and shit right on top of the bastard, DISGUSTING\n\r",
		/*3*/	"֍\tYou left the door open, so someone walked in and watched you taking a shit and saw your junk\n\r",
		/*4*/	"֍\tThe ceiling was disgusting and it dripped filth right on you during your shit\n\r",
		/*5*/	"֍\tThe floor was disgusting and you got shit all over your shoes\n\r",
		/*6*/	"֍\tThe occupation sign was missing and someone rudely rattled the door during your shit\n\r",
		/*7*/	"֍\tThe lock was missing, so someone interrupted your shit and saw your junk\n\r",
		/*8*/	"֍\tThere was no toilet seat, so you had to shit on the bare toilet bowl\n\r",
		/*9*/	"֍\tYou had to sit on a filthy toilet seat, and caught a disease\n\r",
		/*10*/	"֍\tThe toilet lid was disgusting and you got shit on your hand when you lifted it up\n\r",
		/*11*/	"֍\tYou couldnt flush the toilet, branding you as a subhuman cretin\n\r",
		/*12*/	"֍\tThere was no toilet paper so you had to leave with a dirty ass and got shit all over your underpants\n\r",
		/*13*/	"֍\tThe wall was missing, so someone watched you taking a shit and saw your junk\n\r",
		/*14*/	"֍\tThe door was missing, so someone watched you taking a shit and saw your junk\n\r",
		/*15*/	"֍\tThere was no toilet bowl, so you shit on the floor like a feral dog\n\r",
		/*16*/	"֍\tThe toilet bowl was revolting and it smelled real bad\n\r",
		/*17*/	"֍\tYou wiped your ass with dirty toilet paper, your ass is dirtier than when you started\n\r",
		/*18*/	"֍\tYou left the door unlocked, so someone walked in and watched you taking a shit and saw your junk\n\r"
	};

	private string[] resultsGenerics = new string[]{
		"RESULTS\n\r-----------\n\r\n\r",
		"\n\r\n\r\n\rOVERALL SCORE: ",
		"\n\r\n\r\n\rPRESS ESC TO EXIT\n\rOR ANY OTHER KEY TO CONTINUE"
	};

	public float ReturnStallScore(StallSystem.StallData stall){
		float stallScore = 10000;
		float minusScore = 0;
		foreach (KeyValuePair<string,StallSystem.StallComponent> stallPart in stall.stallContents) {
			minusScore = (stallPart.Value.filth * 10) * stallPart.Value.multiplier;
			minusScore += (stallPart.Value.ruination * 10) * stallPart.Value.multiplier;
		}
		return stallScore - minusScore;
	}

	public ResultsData BuildResultsText(StallSystem.StallData stallChosen){
		builder.Length = 0;
		ResultsData newResults = new ResultsData ();
		builder.Append (resultsGenerics [0]);

		if (Gameboss.movement.playerCoord [1] == 0 || Gameboss.movement.playerCoord [1] == 1) {
			builder.Append (resultsText [0]);
			newResults.overallScore = 0;
		} else if (Gameboss.movement.playerCoord [1] == 2) {
			builder.Append (resultsText [1]);
			newResults.overallScore = 0;
		} else {
			
			if (stallChosen.doorState != StallSystem.lidState.closed && stallChosen.doorState != StallSystem.lidState.missing ) {builder.Append (resultsText [3]);}
			else if (stallChosen.doorState == StallSystem.lidState.missing){builder.Append (resultsText [14]);}

			if (stallChosen.stallContents["right_wall"].ruination > Gameboss.stalls.ruinationLimit ){builder.Append (resultsText [13]);}

			if (stallChosen.isLocked == false && stallChosen.stallContents["lock"].ruination <= Gameboss.stalls.ruinationLimit) {builder.Append (resultsText [18]);}
			else if (stallChosen.stallContents["lock"].ruination > Gameboss.stalls.ruinationLimit) {builder.Append (resultsText [7]);}





		
		}
		builder.Append (resultsGenerics [1]);
		builder.Append (newResults.overallScore.ToString ());
		builder.Append (resultsGenerics [2]);
		newResults.resultsText = builder.ToString ();
		return newResults;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
