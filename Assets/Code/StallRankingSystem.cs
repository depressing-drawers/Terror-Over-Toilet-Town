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
		/*0*/	"☻\tYou shit straight on the floor like a filthy animal, without even going into a stall\n\r",
		/*1*/	"☻\tYou shit straight on the floor like a dirty ape, but at least you did it in a stall\n\r",
		/*2*/	"☻\tYou forgot to put the toilet lid up and shit right on top of the bastard, DISGUSTING\n\r",
		/*3*/	"☻\tYou left the door open, so someone walked in and watched you taking a shit and saw your junk\n\r",
		/*4*/	"☻\tThe ceiling was disgusting and it dripped filth right on you during your shit\n\r",
		/*5*/	"☻\tThe floor was disgusting and you got shit all over your shoes\n\r",
		/*6*/	"☻\tThe occupation sign was missing and someone rudely rattled the door during your shit\n\r",
		/*7*/	"☻\tThe lock was missing, so someone interrupted your shit and saw your junk\n\r",
		/*8*/	"☻\tThere was no toilet seat, so you had to shit on the bare toilet bowl\n\r",
		/*9*/	"☻\tYou had to sit on a filthy toilet seat, and caught a disease\n\r",
		/*10*/	"☻\tThe toilet lid was disgusting and you got shit on your hand when you lifted it up\n\r",
		/*11*/	"☻\tYou couldnt flush the toilet, branding you as a subhuman cretin\n\r",
		/*12*/	"☻\tThere was no toilet paper so you had to leave with a dirty ass and got shit all over your underpants\n\r",
		/*13*/	"☻\tThe wall was missing, so someone watched you taking a shit and saw your junk\n\r",
		/*14*/	"☻\tThe door was missing, so someone watched you taking a shit and saw your junk\n\r",
		/*15*/	"☻\tThere was no toilet bowl, so you shit on the floor like a feral dog\n\r",
		/*16*/	"☻\tThe toilet bowl was revolting and it smelled real bad\n\r",
		/*17*/	"☻\tYou wiped your ass with dirty toilet paper, your ass is dirtier than when you started\n\r",
		/*18*/	"☻\tYou left the door unlocked, so someone walked in and watched you taking a shit and saw your junk\n\r",
		/*19*/	"☻\tThe lock was disgusting and you got shit on your hand when you used it\n\r",
		/*20*/	"☻\tThe interior handle was disgusting and you got shit on your hand when you used it\n\r",
		/*21*/	"☻\tThe exterior handle was disgusting and you got shit on your hand when you used it\n\r",
		/*22*/	"☻\tThe flusher handle was disgusting and you got shit on your hand when you used it\n\r"


	};

	private string[] resultsGenerics = new string[]{
		"RESULTS\n\r-----------\n\r",
		"\n\r\n\rYOUR SCORE: ",
		"\n\r\n\rPRESS ANY KEY TO CONTINUE",
		"YOU CHOSE STALL NUMBER: ",
		"\n\rTHE BEST STALL WAS NUMBER "
	};

	public float ReturnStallScore(StallSystem.StallData stall){
		float stallScore = 100;
		float minusScore = 0;
		foreach (KeyValuePair<string,StallSystem.StallComponent> stallPart in stall.stallContents) {
			minusScore = (stallPart.Value.filth * 10) * stallPart.Value.multiplier;
			minusScore += (stallPart.Value.ruination * 10) * stallPart.Value.multiplier;
		}
		return stallScore - minusScore;
	}

	public ResultsData BuildResultsText(StallSystem.StallData stallChosen){
		builder.Length = 0;
		ResultsData bestStallData = ReturnBestStall ();
		ResultsData newResults = new ResultsData ();
		newResults.overallScore = -666;
		builder.Append (resultsGenerics [0]);
		builder.Append (resultsGenerics[3]);
		builder.Append (Gameboss.movement.playerCoord [0].ToString ());
		builder.Append (resultsGenerics[4]);
		builder.Append (bestStallData.resultsText);
		builder.Append ("\n\rWHICH WAS WORTH ");
		builder.Append (bestStallData.overallScore.ToString ());
		builder.Append (" POINTS OUT OF 100\n\r\n\r");

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


			if (stallChosen.stallContents["right_wall"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["right_wall"].filth > Gameboss.stalls.flithLimit){} 
			if (stallChosen.stallContents["door"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["door"].filth > Gameboss.stalls.flithLimit){} 
			if (stallChosen.stallContents["exterior_handle"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["exterior_handle"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [21]);} 
			if (stallChosen.stallContents["occupied_sign"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["occupied_sign"].filth > Gameboss.stalls.flithLimit){} 
			if (stallChosen.stallContents["interior_handle"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["interior_handle"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [20]);} 
			if (stallChosen.stallContents["lock"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["lock"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [19]);} 
			if (stallChosen.stallContents["floor"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["floor"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [5]);} 
			if (stallChosen.stallContents["ceiling"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["ceiling"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [4]);} 
			if (stallChosen.stallContents["back_wall"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["back_wall"].filth > Gameboss.stalls.flithLimit){} 
			if (stallChosen.stallContents["dispenser"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["dispenser"].filth > Gameboss.stalls.flithLimit){} 
			if (stallChosen.stallContents["paper"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["paper"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [17]);} 
			if (stallChosen.stallContents["bowl"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["bowl"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [16]);} 
			if (stallChosen.stallContents["cistern"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["cistern"].filth > Gameboss.stalls.flithLimit){} 
			if (stallChosen.stallContents["cistern_lid"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["cistern_lid"].filth > Gameboss.stalls.flithLimit){} 
			if (stallChosen.stallContents["flusher"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["flusher"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [22]);} 
			if (stallChosen.stallContents["toilet_lid"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["toilet_lid"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [10]);} 
			if (stallChosen.stallContents["toilet_seat"].ruination < Gameboss.stalls.ruinationLimit && stallChosen.stallContents["toilet_seat"].filth > Gameboss.stalls.flithLimit){builder.Append (resultsText [9]);} 


		
		}
		builder.Append (resultsGenerics [1]);
		if (newResults.overallScore != -666) {
			builder.Append (newResults.overallScore.ToString ());
		} else {
			builder.Append (stallChosen.stallScore.ToString());
		}
		builder.Append (" out of 100");
		builder.Append (resultsGenerics [2]);
		newResults.resultsText = builder.ToString ();
		return newResults;
	}

	ResultsData ReturnBestStall(){
		int bestStall = -666;
		float bestScore = -99999;
		ResultsData stallData = new ResultsData ();

		for (int i = 0; i < Gameboss.stalls.currentStalls.Count; i++) {
			if (Gameboss.stalls.currentStalls [i].stallScore > bestScore) {
				bestStall = i + 0;
				bestScore = Gameboss.stalls.currentStalls [i].stallScore + 0;
			}
		}

		stallData.resultsText = bestStall.ToString ();
		stallData.overallScore = bestScore;
		return stallData;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
