using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Linq;
//using System.Data;
using Npgsql;
using NpgsqlTypes;
using System.Globalization;

public class ParseGame2023 : MonoBehaviour {

	public Text fileNameInput;
	public Text console;
	private string fileName;
	private string fileDirectory;
	private string[] results;
	private string[] passingLinesHOME, passingLinesAWAY;
	private string[] rushingLinesHOME, rushingLinesAWAY;
	private string[] receivingLinesHOME, receivingLinesAWAY;
	private string[] defenseLinesHOME, defenseLinesAWAY;
	private string[] kickingLinesHOME, kickingLinesAWAY;
	private string[] returnLines;
	private string scoreLine;
	private int olineLine, puntingLine, penaltyLine, sacksLine, thirdDownLine, kickReturnLine, puntReturnLine, fourthDownLine;
	private int homeScore, awayScore;
	private string firstName;
	private string lastName;
	private int games;
	private int completions;
	private int passAttempts;
	private int passYards;
	private int passTD;
	private int interceptions;
	private int catches;
	private int receivingYards;
	private int receivingTD;
	private int drops;
	private int qbFumbles, qbFumblesLost;
	private int fumblesRec, fumblesRecLost;
	private int fumblesRush, fumblesRushLost;
	private int totalFumblesHOME, totalFumblesAWAY, totalFumblesLostHOME, totalFumblesLostAWAY, totalDropsHOME, totalDropsAWAY;
	private int rushAttempts;
	private int rushTD;
	private int rushYards;
	private int hundredYardGames;
	private int tackles, TFL;
	private double sacks;
	private int intDef, pd;
	private int ff;
	private int fr;
	private int safety;
	private int defTD;
	private int block;
	private int kickAttempts = 0;
	private int kickMiss = 0;
	private int xpAttempts = 0;
	private int xpMade = 0;
	private int kickTD;
	private int puntTD;
	private double kickPercent;
	private int kickFifty = 0;
	private int kickFourty = 0;
	private int kickThirty = 0;
	private int kickTwenty = 0;
	private int kickZero = 0;
	private int fgDistance = 0;
	private int thirdconvertedHOME, thirdtotalHOME, thirdconvertedAWAY, thirdtotalAWAY, thirdconverteddefHOME, thirdconverteddefAWAY, thirdtotaldefHOME, thirdtotaldefAWAY;
	private double thirdrateOffense, thirdrateDefense;
	private double completionPercent;
	private double intPercent;
	private double tdPercent;
	private double YPA;
	private double YPCarry;
	private double YPCatch;
	private double YPGpass;
	private double YPGrun;
	private double YPGreceiving;
	private double rating;
	private string team;
	public double anya;
	private NumberFormatInfo nfi = new CultureInfo( "en-US", false ).NumberFormat;

	private int teamRushYards;
	private int teamTotalYards;
	private double teamPassYardsPG;
	private double teamRushYardsPG;
	private double teamTotalYardsPG;
	private int teamPointsForHOME, teamPointsForAWAY;
	private int teamPointsAgainstHOME, teamPointsAgainstAWAY;
	private int teamPointDiffHOME, teamPointDiffAWAY;
	private int teamPassYardsHOME, teamPassYardsAWAY;
	private int teamPassYardsDefHOME, teamPassYardsDefAWAY;
	private int teamRushYardsHOME, teamRushYardsAWAY, teamRushYardsDefHOME, teamRushYardsDefAWAY;
	private int teamRushTdHOME, teamRushTdAWAY, teamRushTdDefHOME, teamRushTdDefAWAY;
	private int totalYardsHOME, totalYardsAWAY , totalTdsHOME, totalTdsAWAY, totalYardsDefHOME, totalYardsDefAWAY;
	private int teamTdHOME, teamTdAWAY, teamTdDefHOME, teamTdDefAWAY;
	private int teamDefensiveTdHOME, teamDefensiveTdAWAY, teamStTdHOME, teamStTdAWAY;
	private int teamIntOffHOME, teamIntDefHOME, teamIntOffAWAY, teamIntDefAWAY;
	private double defensiveYPP, offensiveYPP;
	private double teamSacksHOME, teamSacksAWAY, teamSacksOffenseHOME, teamSacksOffenseAWAY;
	private int teamSafetyOffenseHOME, teamSafetyOffenseAWAY, teamSafetyHOME, teamSafetyAWAY;
	private int teamFfHOME, teamFfAWAY, teamFrHOME, teamFrAWAY;
	private double olineRatingHOME, olineRatingAWAY, puntAvgHOME, puntAvgAWAY;
	private int rushAttemptsHOME, passAttemptsHOME, rushAttemptsAWAY, passAttemptsAWAY, rushAttemptsAgainstHOME, rushAttemptsAgainstAWAY, passAttemptsAgainstHOME, passAttemptsAgainstAWAY;
	private int penaltyNumberHOME, penaltyNumberAWAY, penaltyYardsHOME, penaltyYardsAWAY;
	private string gameLine;
	private string homeTeam, awayTeam;
	private string[] homeLines;
	private string[] awayLines;
	private string[] allLines;
	private string[] teamAbrevs = new string[32];
	private string[] teamNamesFull = new string[32];
	private string[] teamIcons = new String[32];
	private int PRyardsHOME, PRyardsAWAY, KRyardsHOME, KRyardsAWAY, PRtdHOME, PRtdAWAY, KRtdHOME, KRtdAWAY;

	private int scoreIndexHOME, scoreIndexAWAY;


	public void parseGameLog(int file = -1)
	{
		if (file == -1) {
			fileName = fileNameInput.text;
		} else {
			fileName = file.ToString ();
		}
		fileDirectory = Application.dataPath + "/GameReports/" + fileName + ".txt";
		//StreamReader reader=new  StreamReader(fileDirectory);

		results = System.IO.File.ReadAllLines (fileDirectory);
		results = results.Where (x => x != "").ToArray (); // remove empty lines
		results = results.Where (x => x != " ").ToArray (); // sometimes they have a space
		for (int x = 0; x < results.Length; x++) {
			if(results[x].Contains("?"))
			{
				results[x] = results [x].Replace ("?", string.Empty);
				Debug.Log (results [x]);
			}
		}


//		for(int x = 0; x < results.Length; x++)
//		{
//			Debug.Log(results[x]);
//		}
		int passIndexHOME, passIndexAWAY;
		int rushIndexHOME, rushIndexAWAY;
		int receivingIndexHOME, receivingIndexAWAY;
		int defenseIndexHOME, defenseIndexAWAY;
		int kickingIndexHOME, kickingIndexAWAY;
		passIndexHOME = passIndexAWAY = 0;
		rushIndexHOME = rushIndexAWAY = 0;
		receivingIndexHOME = receivingIndexAWAY = 0;
		defenseIndexHOME = defenseIndexAWAY = 0;
		kickingIndexHOME = kickingIndexAWAY = 0;
//		scoreIndexHOME = scoreIndexAWAY = 0;

		gameLine = results[0];
		for (int x = 0; x < results.Length; x++) {
			if (results [x].StartsWith ("Punt Return Yards")) {
				puntReturnLine = x;
			} else if (results [x].StartsWith ("Kick Return Yards")) {
				kickReturnLine = x;
			} else if (results [x].StartsWith ("Offensive Line")) {
				olineLine = x;
			} else if (results [x].StartsWith ("Punting")) {
				puntingLine = x;
			} else if (results [x].StartsWith ("Penalties")) {
				penaltyLine = x;
			} else if (results [x].StartsWith ("Sacks")) {
				sacksLine = x;
			} else if (results [x].StartsWith ("3rd Down")) {
				thirdDownLine = x;
			} else if (results [x].StartsWith ("4th Down")) {
				fourthDownLine = x;
			} else if (results [x].StartsWith ("PASSING")) {
				if (passIndexAWAY == 0)
					passIndexAWAY = x;
				else
					passIndexHOME = x;
			} else if (results [x].StartsWith ("RUSHING")) {
				if (rushIndexAWAY == 0)
					rushIndexAWAY = x;
				else
					rushIndexHOME = x;
			} else if (results [x].StartsWith ("RECEIVING")) {
				if (receivingIndexAWAY == 0)
					receivingIndexAWAY = x;
				else
					receivingIndexHOME = x;
			} else if (results [x].StartsWith ("DEFENSE")) {
				if (defenseIndexAWAY == 0)
					defenseIndexAWAY = x;
				else
					defenseIndexHOME = x;
			} else if (results [x].StartsWith ("KICKING")) {
				if (kickingIndexAWAY == 0)
					kickingIndexAWAY = x;
				else
					kickingIndexHOME = x;
			} else if (results [x].Contains ("1ST")){
				scoreIndexAWAY = x+1;
				//Debug.Log (scoreIndexAWAY);
				scoreIndexHOME = x+2;
			}
		}


		//Debug.Log (results[scoreIndexAWAY] + "\n" + results[scoreIndexHOME]);
//		Debug.Log (results[passIndexAWAY+1]);
//		Debug.Log (results [passIndexHOME + 1]);

		passingLinesHOME = new string[rushIndexHOME - passIndexHOME -1];
		rushingLinesHOME = new string[receivingIndexHOME - rushIndexHOME -1];
		receivingLinesHOME = new string[defenseIndexHOME - receivingIndexHOME - 1];
		defenseLinesHOME = new string[kickingIndexHOME - defenseIndexHOME - 1];
		kickingLinesHOME = new string[1];

		passingLinesAWAY = new string[rushIndexAWAY - passIndexAWAY -1];
		rushingLinesAWAY = new string[receivingIndexAWAY - rushIndexAWAY -1];
		receivingLinesAWAY = new string[defenseIndexAWAY - receivingIndexAWAY - 1];
		defenseLinesAWAY = new string[kickingIndexAWAY - defenseIndexAWAY - 1];
		kickingLinesAWAY = new string[1];
		//Debug.Log(injuriesIndex + " " + kickingIndex);

//		for(int x =0; x < allLines.Length; x++)
//		{
//			allLines[x] = results[x+passIndex+1];
//		}
//		Debug.Log ("all lines in");
		for(int x = 0; x < passingLinesHOME.Length; x++)
		{
			passingLinesHOME[x] = results[x+passIndexHOME +1];
			//console.text += passingLines[x] + "\n";
		}
		//Debug.Log ("pass lines in");
		for(int x = 0; x < rushingLinesHOME.Length; x++)
		{
			rushingLinesHOME[x] = results[x+rushIndexHOME +1];
			//console.text += rushingLines[x] + "\n";
		}
		//Debug.Log ("rush lines in");
		for(int x = 0; x < receivingLinesHOME.Length; x++)
		{
			receivingLinesHOME[x] = results[x+receivingIndexHOME +1];
			//console.text += receivingLines[x] + "\n";
		}
		//Debug.Log ("receiving lines in");
		for(int x = 0; x < defenseLinesHOME.Length; x++)
		{
			defenseLinesHOME[x] = results[x+defenseIndexHOME +1];
			//console.text += defenseLines[x] + "\n";
		}
		//Debug.Log ("defense lines in");
		for (int x = 0; x < kickingLinesHOME.Length; x++) {
			kickingLinesHOME [x] = results [x + kickingIndexHOME + 1];
		}

		for(int x = 0; x < passingLinesAWAY.Length; x++)
		{
			passingLinesAWAY[x] = results[x+passIndexAWAY +1];
			//console.text += passingLines[x] + "\n";
		}
		//Debug.Log ("pass lines in");
		for(int x = 0; x < rushingLinesAWAY.Length; x++)
		{
			rushingLinesAWAY[x] = results[x+rushIndexAWAY +1];
			//console.text += rushingLines[x] + "\n";
		}
		//Debug.Log ("rush lines in");
		for(int x = 0; x < receivingLinesAWAY.Length; x++)
		{
			receivingLinesAWAY[x] = results[x+receivingIndexAWAY +1];
			//console.text += receivingLines[x] + "\n";
		}
		//Debug.Log ("receiving lines in");
		for(int x = 0; x < defenseLinesAWAY.Length; x++)
		{
			defenseLinesAWAY[x] = results[x+defenseIndexAWAY +1];
			//console.text += defenseLines[x] + "\n";
		}
		//Debug.Log ("defense lines in");
		for (int x = 0; x < kickingLinesAWAY.Length; x++) {
			kickingLinesAWAY [x] = results [x + kickingIndexAWAY + 1];
		}
		//string[] teamParse = results[scoreIndexAWAY].Split ('(',')');

		string[] teamParse = gameLine.Split ('(',')' );
		awayTeam = teamParse[0];
		string tempHomeTeam = teamParse[teamParse.Length - 3];
		string[] homeParse = tempHomeTeam.Split (' ');

		homeTeam = String.Join (" ", homeParse, 2, homeParse.Length - 2);
		homeTeam = convertTeam(homeTeam);
		awayTeam = convertTeam(awayTeam);
		//		Debug.Log (homeTeam);
		//		Debug.Log (awayTeam);

//		homeScore = awayScore = 0;
//		string[] scoreAwayText = results[scoreIndexAWAY].Split (' ' , ',');
//		scoreAwayText = scoreAwayText.Where(j => j != "").ToArray();
//
//		string[] scoreHomeText = results[scoreIndexHOME].Split (' ' , ',');
//		scoreHomeText = scoreHomeText.Where(j => j != "").ToArray();
//		awayScore = int.Parse(scoreAwayText [scoreAwayText.Length - 1]);
//		homeScore = int.Parse(scoreHomeText [scoreHomeText.Length - 1]);
//		Debug.Log (homeTeam + ": " + homeScore);
//		Debug.Log (awayTeam + ": " + awayScore);

		parseLines ();
		//Debug.Log (results[puntReturnLine]);
	}

	public void parseLines()
	{
		

		for(int x = 0; x < passingLinesAWAY.Length; x++)
		{
			completions = 0;
			passAttempts = 0;
			passYards = 0;
			passTD = 0;
			interceptions = 0;
			qbFumbles = 0;
			qbFumblesLost = 0;

			string[] parsedText = passingLinesAWAY[x].Split (' ' , ',', '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			if (parsedText [0] != "-") {
				//			for (int y = 0; y < parsedText.Length; y++) {
				//				Debug.Log ("[" + y + "] " + parsedText [y]);
				//			}

				firstName = parsedText [0];
				lastName = parsedText [1];
				team = awayTeam;
				completions = int.Parse (parsedText [2]);
				passAttempts = int.Parse (parsedText [3]);
				passYards = int.Parse (parsedText [4]);
				passTD = int.Parse (parsedText [5]);
				interceptions = int.Parse (parsedText [6]);
				qbFumbles = int.Parse (parsedText [7]);
				qbFumblesLost = int.Parse (parsedText [8]);

				//Debug.Log (firstName + " " + lastName + " " + completions + "/" + passAttempts + " for " + passYards + " " + passTD + "/" + interceptions + " TD/INT ");

				sendToDB ("passing");
			}
		}
		for(int x = 0; x < passingLinesHOME.Length; x++)
		{
			completions = 0;
			passAttempts = 0;
			passYards = 0;
			passTD = 0;
			interceptions = 0;
			qbFumbles = 0;
			qbFumblesLost = 0;

			string[] parsedText = passingLinesHOME[x].Split (' ' , ',', '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			if (parsedText [0] != "-") {
				//			for (int y = 0; y < parsedText.Length; y++) {
				//				Debug.Log ("[" + y + "] " + parsedText [y]);
				//			}

				firstName = parsedText [0];
				lastName = parsedText [1];

				team = homeTeam;
				completions = int.Parse (parsedText [2]);
				passAttempts = int.Parse (parsedText [3]);
				passYards = int.Parse (parsedText [4]);
				passTD = int.Parse (parsedText [5]);
				interceptions = int.Parse (parsedText [6]);
				qbFumbles = int.Parse (parsedText [7]);
				qbFumblesLost = int.Parse (parsedText [8]);

				//Debug.Log (firstName + " " + lastName + " " + completions + "/" + passAttempts + " for " + passYards + " " + passTD + "/" + interceptions + " TD/INT ");

				sendToDB ("passing");
			}
		}

		for (int x = 0; x < rushingLinesAWAY.Length; x++) {
			rushAttempts = 0;
			rushYards = 0;
			rushTD = 0;
			fumblesRush = 0;
			fumblesRushLost = 0;
			hundredYardGames = 0;

			string[] parsedText = rushingLinesAWAY [x].Split (' ', ',', '/');
			parsedText = parsedText.Where (j => j != "").ToArray ();
			if (parsedText [0] != "-") {
				//			for (int y = 0; y < parsedText.Length; y++) {
				//				Debug.Log ("[" + y + "] " + parsedText [y]);
				//			}

				if (parsedText [0].Contains ("-") || parsedText [1].Contains ("-") || parsedText.Length > 8) {
					//Debug.Log (parsedText[1] + " " + parsedText[2]);
					firstName = parsedText [0] + " " + parsedText [1];
					lastName = parsedText [2];
					team = awayTeam;
					rushAttempts = int.Parse (parsedText [3]);
					rushYards = int.Parse (parsedText [4]);
					rushTD = int.Parse (parsedText [5]);
					fumblesRush = int.Parse (parsedText [7]);
					fumblesRushLost = int.Parse (parsedText [8]);
				} else {
//					Debug.Log (parsedText[1] + " " + parsedText[2]);
					firstName = parsedText [0];
					lastName = parsedText [1];
					team = awayTeam;
					rushAttempts = int.Parse (parsedText [2]);
					rushYards = int.Parse (parsedText [3]);
					rushTD = int.Parse (parsedText [4]);
					fumblesRush = int.Parse (parsedText [6]);
					fumblesRushLost = int.Parse (parsedText [7]);
				}
				if (rushYards >= 100)
					hundredYardGames = 1;

				//Debug.Log (firstName + " " + lastName + " " + rushAttempts + " for " + rushYards + " yards " + rushTD + " TDs ");

				sendToDB ("rushing");
			}
		}

		for(int x = 0; x < rushingLinesHOME.Length; x++)
		{
			rushAttempts = 0;
			rushYards = 0;
			rushTD = 0;
			fumblesRush = 0;
			fumblesRushLost = 0;
			hundredYardGames = 0;

			string[] parsedText = rushingLinesHOME[x].Split (' ' , ',', '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			if (parsedText [0] != "-") {

				//			for (int y = 0; y < parsedText.Length; y++) {
				//				Debug.Log ("[" + y + "] " + parsedText [y]);
				//			}

				if (parsedText [0].Contains ("-") || parsedText [1].Contains ("-") || parsedText.Length > 8) {
					//Debug.Log (parsedText[1] + " " + parsedText[2]);
					firstName = parsedText [0] + " " + parsedText [1];
					lastName = parsedText [2];
					team = homeTeam;
					rushAttempts = int.Parse (parsedText [3]);
					rushYards = int.Parse (parsedText [4]);
					rushTD = int.Parse (parsedText [5]);
					fumblesRush = int.Parse (parsedText [7]);
					fumblesRushLost = int.Parse (parsedText [8]);
				} else {
					//Debug.Log (parsedText[1] + " " + parsedText[2]);
					firstName = parsedText [0];
					lastName = parsedText [1];
					team = homeTeam;
					rushAttempts = int.Parse (parsedText [2]);
					rushYards = int.Parse (parsedText [3]);
					rushTD = int.Parse (parsedText [4]);
					fumblesRush = int.Parse (parsedText [6]);
					fumblesRushLost = int.Parse (parsedText [7]);
				}
				if (rushYards >= 100)
					hundredYardGames = 1;

				//Debug.Log (firstName + " " + lastName + " " + rushAttempts + " for " + rushYards + " yards " + rushTD + " TDs ");

				sendToDB ("rushing");
			}
		}
		for(int x = 0; x < receivingLinesAWAY.Length; x++)
		{
			fumblesRec = 0;
			fumblesRecLost = 0;
			receivingYards = 0;
			catches = 0;
			receivingTD = 0;
			hundredYardGames = 0;
			drops = 0;
			string[] parsedText = receivingLinesAWAY[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			if (parsedText [0] != "-") {
//				if (parsedText [0].Contains ("-") || parsedText [1].Contains ("-")) {
//					firstName = parsedText [0] + " " + parsedText [1];
//					lastName = parsedText [2];
//					if (firstName.IndexOf ("'") != -1) {
//						firstName = firstName.Insert (firstName.IndexOf ("'"), "\'");
//						//Debug.Log (firstName);
//					}
//					if (lastName.IndexOf ("'") != -1) {
//						lastName = lastName.Insert (lastName.IndexOf ("'"), "\'");
//						//Debug.Log (lastName);
//					}
//					team = awayTeam;
//					catches = int.Parse (parsedText [3]);
//					receivingYards = int.Parse (parsedText [4]);
//					receivingTD = int.Parse (parsedText [5]);
//					drops = int.Parse (parsedText [6]);
//					fumblesRec = int.Parse (parsedText [7]);
//					fumblesRecLost = int.Parse (parsedText [8]);
//				} else {
				if (parsedText.Length <= 8) {
					firstName = parsedText [0];
					lastName = parsedText [1];
					team = awayTeam;
					catches = int.Parse (parsedText [2]);
					receivingYards = int.Parse (parsedText [3]);
					receivingTD = int.Parse (parsedText [4]);
					drops = int.Parse (parsedText [5]);
					fumblesRec = int.Parse (parsedText [6]);
					fumblesRecLost = int.Parse (parsedText [7]);
				} else {
					firstName = (parsedText [0] + " " + parsedText[1]);
					lastName = parsedText [2];
					team = awayTeam;
					catches = int.Parse (parsedText [3]);
					receivingYards = int.Parse (parsedText [4]);
					receivingTD = int.Parse (parsedText [5]);
					drops = int.Parse (parsedText [6]);
					fumblesRec = int.Parse (parsedText [7]);
					fumblesRecLost = int.Parse (parsedText [8]);
				}
//				}
				//Debug.Log (firstName + " " + lastName + " " + catches + " for " + receivingYards + " yards " + receivingTD + " TDs ");
				sendToDB ("receiving");
			}	
		}
		for(int x = 0; x < receivingLinesHOME.Length; x++)
		{
			fumblesRec = 0;
			fumblesRecLost = 0;
			receivingYards = 0;
			catches = 0;
			receivingTD = 0;
			hundredYardGames = 0;
			drops = 0;
			string[] parsedText = receivingLinesHOME[x].Split (' ' , ',' , '/', '\t');
			parsedText = parsedText.Where(j => j != "").ToArray();

//			for (int y = 0; y < parsedText.Length; y++) {
//				Debug.Log(parsedText[y]);			
//			}

			if (parsedText [0] != "-") {
				//				if (parsedText [0].Contains ("-") || parsedText [1].Contains ("-")) {
				//					firstName = parsedText [0] + " " + parsedText [1];
				//					lastName = parsedText [2];
				//					if (firstName.IndexOf ("'") != -1) {
				//						firstName = firstName.Insert (firstName.IndexOf ("'"), "\'");
				//						//Debug.Log (firstName);
				//					}
				//					if (lastName.IndexOf ("'") != -1) {
				//						lastName = lastName.Insert (lastName.IndexOf ("'"), "\'");
				//						//Debug.Log (lastName);
				//					}
				//					team = HOMETeam;
				//					catches = int.Parse (parsedText [3]);
				//					receivingYards = int.Parse (parsedText [4]);
				//					receivingTD = int.Parse (parsedText [5]);
				//					drops = int.Parse (parsedText [6]);
				//					fumblesRec = int.Parse (parsedText [7]);
				//					fumblesRecLost = int.Parse (parsedText [8]);
				//				} else {
				if (parsedText.Length <= 8) {
					firstName = parsedText [0];
					lastName = parsedText [1];
					team = homeTeam;
					catches = int.Parse (parsedText [2]);
					receivingYards = int.Parse (parsedText [3]);
					receivingTD = int.Parse (parsedText [4]);
					drops = int.Parse (parsedText [5]);
					fumblesRec = int.Parse (parsedText [6]);
					fumblesRecLost = int.Parse (parsedText [7]);
				} else {
					firstName = (parsedText [0] + " " + parsedText[1]);
					lastName = parsedText [2];
					team = homeTeam;
					catches = int.Parse (parsedText [3]);
					receivingYards = int.Parse (parsedText [4]);
					receivingTD = int.Parse (parsedText [5]);
					drops = int.Parse (parsedText [6]);
					fumblesRec = int.Parse (parsedText [7]);
					fumblesRecLost = int.Parse (parsedText [8]);
				}

				//				}
				//Debug.Log (firstName + " " + lastName + " " + catches + " for " + receivingYards + " yards " + receivingTD + " TDs ");
				sendToDB ("receiving");
			}	
		}

		for(int x = 0; x < defenseLinesAWAY.Length; x++)
		{
			string[] parsedText = defenseLinesAWAY[x].Split (' ' , '/', '\t');
			parsedText = parsedText.Where(j => j != "").ToArray();

			firstName = parsedText[0];
			lastName = parsedText[1];
//			if(firstName.IndexOf ("'") != -1)
//			{
//				firstName = firstName.Insert (firstName.IndexOf ("'") , "\\");
//				Debug.Log (firstName);
//			}
//			if(lastName.IndexOf ("'") != -1)
//			{
//				lastName = lastName.Insert (lastName.IndexOf ("'") , "\\");
//				//Debug.Log (lastName);
//			}
			team = awayTeam;

			tackles = int.Parse (parsedText [2]);
			TFL = int.Parse (parsedText [3]);
			if (parsedText [4] == "-") {
				sacks = 0;
			} else {
				sacks = double.Parse (parsedText [4]);
			}
			intDef = int.Parse (parsedText [5]);
			pd = int.Parse (parsedText [6]);
			ff = int.Parse (parsedText [7]);
			fr = int.Parse (parsedText [8]);
			defTD = int.Parse (parsedText [9]);
			safety = int.Parse (parsedText [10]);

//			Debug.Log (firstName + " " + lastName + " Tackles: " + tackles + " - Sacks: " + sacks + " - INTS: " + intDef);
			sendToDB("defense");
		}

		for(int x = 0; x < defenseLinesHOME.Length; x++)
		{
			string[] parsedText = defenseLinesHOME[x].Split (' ' , '/', '\t');
			parsedText = parsedText.Where(j => j != "").ToArray();

//		
			firstName = parsedText[0];
			lastName = parsedText[1];
//			if(firstName.IndexOf ("'") != -1)
//			{
//				firstName = firstName.Insert (firstName.IndexOf ("'") , "\\");
//				Debug.Log (firstName);
//			}
//			if(lastName.IndexOf ("'") != -1)
//			{
//				lastName = lastName.Insert (lastName.IndexOf ("'") , "\\");
//				//Debug.Log (lastName);
//			}
			team = homeTeam;

			tackles = int.Parse (parsedText [2]);
			TFL = int.Parse (parsedText [3]);
			if (parsedText [4] == "-") {
				sacks = 0;
			} else {
				sacks = double.Parse (parsedText [4]);
			}
			intDef = int.Parse (parsedText [5]);
			pd = int.Parse (parsedText [6]);
			ff = int.Parse (parsedText [7]);
			fr = int.Parse (parsedText [8]);
			defTD = int.Parse (parsedText [9]);
			safety = int.Parse (parsedText [10]);

//			Debug.Log (firstName + " " + lastName + " Tackles: " + tackles + " - Sacks: " + sacks + " - INTS: " + intDef);
			sendToDB("defense");
		}

		/*for (int x = 0; x < kickingLinesAWAY.Length; x++) {
			kickAttempts = 0;
			kickMiss = 0;
			kickPercent = 0;
			kickFifty = 0;
			kickFourty = 0;
			kickThirty = 0;
			kickTwenty = 0;
			kickZero = 0;
			fgDistance = 0;
			string[] parsedText = kickingLinesAWAY [x].Split (' ', ',', '-', '/', '(', ')', '\t');
			parsedText = parsedText.Where (j => j != "").ToArray ();
			firstName = parsedText [0];
			lastName = parsedText [1];
			if (firstName.IndexOf ("'") != -1) {
				firstName = firstName.Insert (firstName.IndexOf ("'"), "\'");
				//Debug.Log (firstName);
			}
			if (lastName.IndexOf ("'") != -1) {
				lastName = lastName.Insert (lastName.IndexOf ("'"), "\'");
				//Debug.Log (lastName);
			}
			kickAttempts = int.Parse (parsedText [3]);
			int kicksMade = int.Parse (parsedText [2]);
			kickMiss = kickAttempts - kicksMade;
			xpMade = int.Parse (parsedText [4]);
			xpAttempts = int.Parse (parsedText [5]);
//			Debug.Log (firstName + " " + lastName + " - " + kicksMade + "/" + kickAttempts + " FGs, " + xpMade + "/" + xpAttempts + " XPs");
			if (kickAttempts != 0) {
				for (int y = 0; y < kickAttempts - kickMiss; y++) {
					fgDistance = int.Parse (parsedText [y + 6]);
					if (fgDistance < 20) {
						kickZero += 1;
					} else if (fgDistance < 30) {
						kickTwenty += 1;
					} else if (fgDistance < 40) {
						kickThirty += 1;
					} else if (fgDistance < 50) {
						kickFourty += 1;
					} else {
						kickFifty += 1;
					}

				}
				sendToDB ("kicking");
			}
		}
		for (int x = 0; x < kickingLinesHOME.Length; x++) {
			kickAttempts = 0;
			kickMiss = 0;
			kickPercent = 0;
			kickFifty = 0;
			kickFourty = 0;
			kickThirty = 0;
			kickTwenty = 0;
			kickZero = 0;
			fgDistance = 0;
			string[] parsedText = kickingLinesHOME [x].Split (' ', ',', '-', '/', '(', ')', '\t');
			parsedText = parsedText.Where (j => j != "").ToArray ();
			firstName = parsedText [0];
			lastName = parsedText [1];
			if (firstName.IndexOf ("'") != -1) {
				firstName = firstName.Insert (firstName.IndexOf ("'"), "\'");
				//Debug.Log (firstName);
			}
			if (lastName.IndexOf ("'") != -1) {
				lastName = lastName.Insert (lastName.IndexOf ("'"), "\'");
				//Debug.Log (lastName);
			}
			kickAttempts = int.Parse (parsedText [3]);
			int kicksMade = int.Parse (parsedText [2]);
			kickMiss = kickAttempts - kicksMade;
			xpMade = int.Parse (parsedText [4]);
			xpAttempts = int.Parse (parsedText [5]);
//			Debug.Log (firstName + " " + lastName + " - " + kicksMade + "/" + kickAttempts + " FGs, " + xpMade + "/" + xpAttempts + " XPs");
			if (kickAttempts != 0) {
				for (int y = 0; y < kickAttempts - kickMiss; y++) {
					fgDistance = int.Parse (parsedText [y + 6]);
					if (fgDistance < 20) {
						kickZero += 1;
					} else if (fgDistance < 30) {
						kickTwenty += 1;
					} else if (fgDistance < 40) {
						kickThirty += 1;
					} else if (fgDistance < 50) {
						kickFourty += 1;
					} else {
						kickFifty += 1;
					}

				}
				sendToDB ("kicking");
			}
		}*/
		doTeamStats();
//		Debug.Log ("TEAM STATS DONE");
	}





	public void sendToDB(string tableName)
	{
		if (firstName.IndexOf ("'") != -1) {
			firstName = firstName.Insert (firstName.IndexOf ("'"), "\'");
			//Debug.Log (firstName);
		}
		if (lastName.IndexOf ("'") != -1) {
			lastName = lastName.Insert (lastName.IndexOf ("'"), "\'");
			//Debug.Log (lastName);
		}
		NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=mighty;Database=2023 NFL Stats;");
		string sqlString = "SELECT * FROM " + tableName + " WHERE last = '" + lastName + "' AND first = '" + firstName + "';";

		NpgsqlCommand command = new NpgsqlCommand(sqlString, conn);

		if(tableName == "passing")
		{
			try
			{
				conn.Open();
				//console.text += "Connected to db...\n";
				//Debug.Log ("Adding Passers");
				NpgsqlDataReader dr = command.ExecuteReader();

				while(dr.Read())
				{
					games = (int)dr[9] +1;
					completions += (int) dr[10];
					passAttempts += (int)dr[11];
					string cmpString = (((double) completions / (double) passAttempts) * 100).ToString ("N" , nfi );
					completionPercent = double.Parse (cmpString);					
					passYards += (int)dr[13];
					string YPAstring = ((double) passYards / (double) passAttempts).ToString ("N" , nfi );
					YPA = double.Parse (YPAstring);
					passTD += (int)dr[14];
					interceptions += (int)dr[15];
					qbFumbles += (int)dr[21];
					qbFumblesLost += (int)dr[22];
					string YPGstring = ((double) passYards / (double) games).ToString ("N", nfi );
					YPGpass = double.Parse (YPGstring);
					string tdPctString = (((double) passTD / (double) passAttempts) *100).ToString ("N", nfi );
					tdPercent = double.Parse (tdPctString);
					string intPctString = (((double) interceptions / (double) passAttempts) *100).ToString ("N", nfi );
					intPercent = double.Parse (intPctString);
					string passRatingString = passerRating(completionPercent, tdPercent, intPercent, YPA).ToString ("N", nfi);
					rating = double.Parse (passRatingString);

				}

			}

			finally
			{
				conn.Close();
			}

			try
			{
				conn.Open();
				string sqlString2 = "UPDATE passing SET completions = " + completions + ",  attempts = " + passAttempts + ",  yards = " + passYards + ",  td = " + passTD + ",  int = " + interceptions + ",  completion_pct = " + completionPercent + ",  ypa = " + YPA + ",  ypg = " + YPGpass + ",  games = " + games + ",  td_pct = " + tdPercent + ",  int_pct = " + intPercent + ",  rating = " + rating + ", fumbles = " + qbFumbles + ", fumlost = " + qbFumblesLost + " WHERE last = '" + lastName + "' AND first = '" + firstName + "';";
				NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);
				int rowsaffected;
				rowsaffected = WriteCommand.ExecuteNonQuery();
				//Debug.Log (rowsaffected);
			}
			finally
			{
				conn.Close();
			}
		}
		else if(tableName == "defense")
		{

			try
			{
				conn.Open();
				//console.text += "Connected to db...\n";
				//Debug.Log ("Adding Defense");
				NpgsqlDataReader dr = command.ExecuteReader();

				while(dr.Read())
				{
					games = (int)dr[9] +1;
					tackles += (int)dr[10];
					intDef += (int)dr[11];
					sacks = sacks + (double)dr[12];
					ff += (int)dr[13];
					fr += (int)dr[14];
					block += (int)dr[15];
					defTD += (int)dr[16];
					safety += (int)dr[17];
					pd += (int)dr[18];
					TFL += (int)dr[19];
				}

			}

			finally
			{
				conn.Close();
			}

			try
			{
				conn.Open();

//				Debug.Log(firstName);
				string sqlString2 = "UPDATE defense SET games = " + games + ",  tackles = " + tackles + ",  int = " + intDef + ",  td = " + defTD + ",  safety = " + safety + ",  sack = " + sacks + ",  ff = " + ff + ",  fr = " + fr + ",  block = " + block + ", pd = " + pd + ", tfl = " + TFL + " WHERE last = '" + lastName + "' AND first = '" + firstName + "';";
				if(firstName.Contains("\\"))
				{
					Debug.Log(sqlString2);
				}
				NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);
				int rowsaffected;
				rowsaffected = WriteCommand.ExecuteNonQuery();
				//Debug.Log (rowsaffected);
			}
			finally
			{
				conn.Close();
			}

		}
		else if(tableName == "rushing")
		{

			try
			{
				conn.Open();
				//console.text += "Connected to db...\n";
				//Debug.Log ("Adding rushers");
				//Debug.Log (firstName + " " + lastName);
				NpgsqlDataReader dr = command.ExecuteReader();

				while(dr.Read())
				{
					games = (int)dr[9] +1;
					rushAttempts += (int)dr[10];
					rushYards += (int)dr[11];
					rushTD += (int)dr[12];
					fumblesRush += (int)dr[13];
					hundredYardGames += (int)dr[14];
					fumblesRushLost += (int)dr[17];

					string YPCstring = ((double) rushYards / (double) rushAttempts).ToString ("N", nfi );
					YPCarry = double.Parse (YPCstring);
					string YPGstring = ((double) rushYards / (double) games).ToString ("N", nfi );
					YPGrun = double.Parse (YPGstring);
				}

			}

			finally
			{
				conn.Close();
			}

			try
			{
				conn.Open();
				string sqlString2 = "UPDATE rushing SET games = " + games + ",  attempts = " + rushAttempts + ",  yards = " + rushYards + ",  td = " + rushTD + ",  fumble = " + fumblesRush + ",  hunydgames = " + hundredYardGames + ",  ypc = " + YPCarry + ",  ypg = " + YPGrun + ", fumlost = " + fumblesRushLost + " WHERE last = '" + lastName + "' AND first = '" + firstName + "';";
				NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);
				int rowsaffected;
				rowsaffected = WriteCommand.ExecuteNonQuery();
				//Debug.Log (rowsaffected);
			}
			finally
			{
				conn.Close();
			}
		}
		else if(tableName == "receiving")
		{

			try
			{
				conn.Open();
				//console.text += "Connected to db...\n";
				//Debug.Log ("Adding Receivers");
				//Debug.Log (firstName + " " + lastName);
				NpgsqlDataReader dr = command.ExecuteReader();

				while(dr.Read())
				{
					games = (int)dr[9] +1;
					catches += (int)dr[10];
					receivingYards += (int)dr[11];
					receivingTD += (int)dr[12];
					fumblesRec += (int)dr[13];
					drops += (int)dr[16];
					fumblesRecLost += (int) dr[17];

					string YPCstring = ((double) receivingYards / (double) catches).ToString ("N" , nfi );
					YPCatch = double.Parse (YPCstring);
					string YPGstring = ((double) receivingYards / (double) games).ToString ("N" , nfi );
					YPGreceiving = double.Parse (YPGstring);
				}

			}

			finally
			{
				conn.Close();
			}

			try
			{
				conn.Open();
				string sqlString2 = "UPDATE receiving SET games = " + games + ",  catches = " + catches + ",  yards = " + receivingYards + ",  td = " + receivingTD + ",  fumble = " + fumblesRec + ",  ypc = " + YPCatch + ",  ypg = " + YPGreceiving + ", drops = " + drops + ", fumlost = " + fumblesRecLost + " WHERE last = '" + lastName + "' AND first = '" + firstName + "';";
				NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);
				int rowsaffected;
				rowsaffected = WriteCommand.ExecuteNonQuery();
				//Debug.Log (rowsaffected);
			}
			finally
			{
				conn.Close();
				//Debug.Log ("Finisehd receiver!");
			}
		}
		else if(tableName == "kicking")
		{

			try
			{
				conn.Open();
				//console.text += "Connected to db...\n";
				//Debug.Log ("Adding kickers");
				NpgsqlDataReader dr = command.ExecuteReader();

				while(dr.Read())
				{
					kickAttempts += (int)dr[9];
					kickMiss += (int)dr[10];
					string kpctString = (((double) (kickAttempts - kickMiss) / (double) kickAttempts) * 100).ToString ("N" , nfi );
					kickPercent = double.Parse (kpctString);
					kickFifty += (int)dr[12];
					kickFourty += (int)dr[13];
					kickThirty += (int)dr[14];
					kickTwenty += (int)dr[15];
					kickZero += (int)dr[16];
				}

			}

			finally
			{
				conn.Close();
			}

			try
			{
				conn.Open();
				string sqlString2 = "UPDATE kicking SET attempts = " + kickAttempts + ",  missed = " + kickMiss + ",  pct = " + kickPercent + ",  kick50 = " + kickFifty + ",  kick40 = " + kickFourty + ",  kick30 = " + kickThirty + ",  kick20 = " + kickTwenty + ",  kick00 = " + kickZero + " WHERE last = '" + lastName + "' AND first = '" + firstName + "';";
				NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);
				int rowsaffected;
				rowsaffected = WriteCommand.ExecuteNonQuery();
				//Debug.Log (rowsaffected);
			}
			finally
			{
				conn.Close();
			}
		}

//		else if(tableName == "returns")
//		{
//			try
//			{
//				conn.Open();
//				//console.text += "Connected to db...\n";
//				Debug.Log ("Adding returners");
//				NpgsqlDataReader dr = command.ExecuteReader();
//
//				while(dr.Read())
//				{
//					kickTD += (int)dr[9];
//					puntTD += (int)dr[10];
//				}
//			}
//
//			finally
//			{
//				conn.Close();
//			}
//
//			try
//			{
//				conn.Open();
//				string sqlString2 = "UPDATE returns SET kicktd = " + kickTD + ", punttd = " + puntTD + " WHERE last = '" + lastName + "' AND first = '" + firstName + "';";
//				NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);
//				int rowsaffected;
//				rowsaffected = WriteCommand.ExecuteNonQuery();
//				//Debug.Log (rowsaffected);
//			}
//			finally
//			{
//				conn.Close();
//			}
//		}

	}
	public string convertTeam(string teamName)
	{
		if(teamName.StartsWith(" ")){
			teamName = teamName.Remove (0,1);
		}
//				Debug.Log (teamName);
		int index = Array.FindIndex (teamNamesFull, x => x == teamName);
				Debug.Log (index);
				Debug.Log (teamName + ":" + teamAbrevs[index]);
		teamName = teamAbrevs[index];
		return teamName;
	}

	public string convertTeamFromIcon(string teamName)
	{
		int index = Array.FindIndex (teamIcons, x => x == teamName);
		teamName = teamAbrevs [index];
		return teamName;
	}

	private void populateTeamArrays()
	{
		teamAbrevs = new string[]{"ARI" , "ATL" , "BAL" , "BUF" , "CAR" , "CHI" , "CIN" , "CLE" , "DAL" , "DEN" , "DET" , "GB" , "HOU" , "IND" , "JAX" , "KC" , "LAR", "MIA" , "MIN" , "NE" , "NO" , "NYG" , "NYJ" , "LV" , "PHI" , "PIT" , "LAC" , "SEA" , "SF" , "TB" , "TEN" , "WAS"};
		//  	THERE NEEDS TO BE A SPACE AFTER THE TEAM NAME FOR THIS TO WORK
		teamNamesFull = new string[]{"Arizona Cardinals " , "Atlanta Falcons " , "Baltimore Ravens " , "Buffalo Bills " , "Carolina Panthers " , "Chicago Bears " , "Cincinnati Bengals " , "Cleveland Browns " , "Dallas Cowboys " , "Denver Broncos " , "Detroit Lions " , "Green Bay Packers " ,
			"Houston Texans " , "Indianapolis Colts " , "Jacksonville Jaguars " , "Kansas City Chiefs " , "Los Angeles Rams " , "Miami Dolphins " , "Minnesota Vikings " , "New England Patriots " , "New Orleans Saints " , "New York Giants " , "New York Jets " , "Las Vegas Raiders " ,
			"Philadelphia Eagles " , "Pittsburgh Steelers " , "Los Angeles Chargers " , "Seattle Seahawks " , "San Francisco 49ers " , "Tampa Bay Buccaneers " , "Tennessee Titans " , "Washington Redskins "} ;

		teamIcons = new string[]{":cardinals:" , ":falcons:" , ":ravens:" , ":bills:" , ":panthers:" , ":bears:" , ":bengals:" , ":browns:" , ":cowboys:" , ":broncos:" , ":fibur:" , ":packers:" , ":texans:" , ":colts:" , ":jaguars:" , ":chiefs:" , ":rams:", ":dolphins:" , ":vikings:" , ":patriots:" , ":saints:" , ":giants:" , ":jets:" , ":raiders:" , ":eagles:" , ":steelers:" , ":chargers:" , ":seahawks:" , ":49ers:" , ":buccaneers:" , ":titans:" , ":redskins:"};

	}

	public double passerRating(double compPct, double tdPct, double intPct, double yardsPerAttempt)
	{
		double result = (compPct - 30) * 0.05;
		if(result < 0)
			result = 0;
		else if (result > 2.375)
			result = 2.375;
		compPct = result;

		result = (yardsPerAttempt - 3) * 0.25;
		if(result < 0)
			result = 0;
		else if (result > 2.375)
			result = 2.375;
		yardsPerAttempt = result;

		result = tdPct * 0.2;
		if(result > 2.375)
			result = 2.375;
		tdPct = result;

		result = 2.375 - (intPct * 0.25);
		if(result < 0)
			result = 0;
		intPct = result;

		return ((compPct + tdPct + intPct + yardsPerAttempt)/6)*100;

	}

	public void doTeamStats()
	{
		//Debug.Log ("TEAM STATS START");
		teamPassYardsHOME =	teamPassYardsDefAWAY = teamPassYardsAWAY = teamPassYardsDefHOME = teamTdHOME = teamTdAWAY = teamTdDefHOME = teamTdDefAWAY = teamIntOffHOME = teamIntDefHOME = teamIntOffAWAY = teamIntDefAWAY = 0;
		teamRushYardsHOME = teamRushYardsAWAY = teamRushYardsDefHOME = teamRushYardsDefAWAY = 0;
		totalYardsHOME = totalYardsAWAY = totalTdsHOME = totalTdsAWAY = totalYardsDefHOME = totalYardsDefAWAY = 0;
		teamDefensiveTdHOME = teamDefensiveTdAWAY = teamStTdHOME = teamStTdAWAY = games = 0;
		teamSacksHOME = teamSacksAWAY = teamSacksOffenseHOME = teamSacksOffenseAWAY = teamSafetyOffenseHOME = teamSafetyOffenseAWAY = teamSafetyHOME = teamSafetyAWAY = teamFfHOME = teamFfAWAY = teamFrHOME = teamFrAWAY = 0;
		olineRatingHOME = olineRatingAWAY = puntAvgHOME = puntAvgAWAY = 0;
		penaltyNumberHOME = penaltyNumberAWAY = penaltyYardsHOME = penaltyYardsAWAY = 0;
		rushAttemptsHOME = rushAttemptsAWAY = passAttemptsHOME = passAttemptsAWAY = 0;
		rushAttemptsAgainstHOME = rushAttemptsAgainstAWAY = passAttemptsAgainstHOME = passAttemptsAgainstAWAY = 0;
		totalFumblesHOME = totalFumblesAWAY = totalFumblesLostHOME = totalFumblesLostAWAY = totalDropsHOME = totalDropsAWAY = 0;
		thirdconvertedAWAY = thirdconvertedHOME = thirdtotalAWAY = thirdtotalHOME = thirdconverteddefAWAY = thirdconverteddefHOME = thirdtotaldefAWAY = thirdtotaldefHOME = 0; 
		PRtdHOME = PRtdAWAY = KRtdHOME = KRtdAWAY = PRyardsAWAY = PRyardsHOME = KRyardsHOME = KRyardsAWAY = 0;

		homeScore = awayScore = 0;
		string[] scoreAwayText = results[scoreIndexAWAY].Split (' ' , ',');
		//Debug.Log (scoreIndexAWAY);
		scoreAwayText = scoreAwayText.Where(j => j != "").ToArray();

		string[] scoreHomeText = results[scoreIndexHOME].Split (' ' , ',');
		scoreHomeText = scoreHomeText.Where(j => j != "").ToArray();
		awayScore = int.Parse(scoreAwayText [scoreAwayText.Length - 1]);
		homeScore = int.Parse(scoreHomeText [scoreHomeText.Length - 1]);

		for(int x = 0; x < passingLinesAWAY.Length; x++)
		{
			passYards = 0;
			passTD = 0;
			interceptions = 0;
			qbFumbles = 0;
			qbFumblesLost = 0;

			string[] parsedText = passingLinesAWAY[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			if (parsedText [0] != "-") {
				team = awayTeam;
				passYards = int.Parse(parsedText[4]);
				passTD = int.Parse(parsedText[5]);
				interceptions = int.Parse (parsedText[6]);
				qbFumbles = int.Parse (parsedText [7]);
				qbFumblesLost = int.Parse (parsedText [8]);
				//Debug.Log ("Home: " + homeTeam + " --- Away: " + awayTeam);
			
				passAttemptsAWAY += int.Parse(parsedText[3]);
				passAttemptsAgainstHOME += int.Parse(parsedText[3]);
				teamPassYardsAWAY += passYards;
				teamPassYardsDefHOME += passYards;
				teamTdAWAY += passTD;
				teamTdDefHOME += passTD;
				teamIntOffAWAY += interceptions;
				teamIntDefHOME += interceptions;
				totalFumblesAWAY += qbFumbles;
				totalFumblesLostAWAY += qbFumblesLost;
	//			Debug.Log ("Passer " + firstName + " " + lastName + " added.");
				//Debug.Log(parsedText[0] + parsedText[1] + " added (team)");
			}
		}
		for(int x = 0; x < passingLinesHOME.Length; x++)
		{
			passYards = 0;
			passTD = 0;
			interceptions = 0;
			qbFumbles = 0;
			qbFumblesLost = 0;

			string[] parsedText = passingLinesHOME[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			if (parsedText [0] != "-") {
				team = homeTeam;
				passYards = int.Parse(parsedText[4]);
				passTD = int.Parse(parsedText[5]);
				interceptions = int.Parse (parsedText[6]);
				qbFumbles = int.Parse (parsedText [7]);
				qbFumblesLost = int.Parse (parsedText [8]);
				//Debug.Log ("Home: " + homeTeam + " --- Away: " + awayTeam);
				passAttemptsHOME += int.Parse(parsedText[3]);
				passAttemptsAgainstAWAY += int.Parse(parsedText[3]);
				teamPassYardsHOME += passYards;
				teamPassYardsDefAWAY += passYards;
				teamTdHOME += passTD;
				teamTdDefAWAY += passTD;
				teamIntOffHOME += interceptions;
				teamIntDefAWAY += interceptions;
				totalFumblesHOME += qbFumbles;
				totalFumblesLostHOME += qbFumblesLost;
	//			Debug.Log ("Passer " + firstName + " " + lastName + " added.");
				//Debug.Log(parsedText[0] + parsedText[1] + " added (team)");
			}
		}
		for (int x = 0; x < rushingLinesAWAY.Length; x++) {
			rushYards = 0;
			rushTD = 0;
			fumblesRush = 0;
			fumblesRushLost = 0;

			string[] parsedText = rushingLinesAWAY [x].Split (' ', ',', '/');
			parsedText = parsedText.Where (j => j != "").ToArray ();
			//			Debug.Log (parsedText[5]);
			team = awayTeam;
			if (parsedText [0] != "-") {
				if (parsedText.Length > 8) { // Booker T. Washington exception
				
					rushYards = int.Parse (parsedText [4]);
					rushTD = int.Parse (parsedText [5]);
					fumblesRush = int.Parse (parsedText [7]);
					fumblesRushLost = int.Parse (parsedText [8]);
					rushAttemptsAWAY += int.Parse (parsedText [3]);
					rushAttemptsAgainstHOME += int.Parse (parsedText [3]);
					teamRushYardsAWAY += rushYards;
					teamRushYardsDefHOME += rushYards;
					teamTdAWAY += rushTD;
					teamTdDefHOME += rushTD;
					totalFumblesAWAY += fumblesRush;
					totalFumblesLostAWAY += fumblesRushLost;
					//fumblesRush = int.Parse (parsedText[10]);

				} else {
					//				firstName = parsedText[0];  // not needed because this is team stats
					//				lastName = parsedText[1];
					rushYards = int.Parse (parsedText [3]);
					rushTD = int.Parse (parsedText [4]);
					fumblesRush = int.Parse (parsedText [6]);
					fumblesRushLost = int.Parse (parsedText [7]);
					//fumblesRush = int.Parse (parsedText[9]);
					rushAttemptsAWAY += int.Parse (parsedText [2]);
					rushAttemptsAgainstHOME += int.Parse (parsedText [2]);
					teamRushYardsAWAY += rushYards;
					teamRushYardsDefHOME += rushYards;
					teamTdAWAY += rushTD;
					teamTdDefHOME += rushTD;
					totalFumblesAWAY += fumblesRush;
					totalFumblesLostAWAY += fumblesRushLost;

				}
			}
			//Debug.Log(parsedText[0] + parsedText[1] + " added (team)");
		}

		for(int x = 0; x < rushingLinesHOME.Length; x++)
		{
			rushYards = 0;
			rushTD = 0;
			fumblesRush = 0;
			fumblesRushLost = 0;

			string[] parsedText = rushingLinesHOME[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			//			Debug.Log (parsedText[5]);
			team = homeTeam;
			if (parsedText [0] != "-") {
				
				if (parsedText.Length > 8) { // Booker T. Washington exception

					rushYards = int.Parse (parsedText [4]);
					rushTD = int.Parse (parsedText [5]);
					fumblesRush = int.Parse (parsedText [7]);
					fumblesRushLost = int.Parse (parsedText [8]);
					rushAttemptsHOME += int.Parse (parsedText [3]);
					rushAttemptsAgainstAWAY += int.Parse (parsedText [3]);
					teamRushYardsHOME += rushYards;
					teamRushYardsDefAWAY += rushYards;
					teamTdHOME += rushTD;
					teamTdDefAWAY += rushTD;
					totalFumblesHOME += fumblesRush;
					totalFumblesLostHOME += fumblesRushLost;
					//fumblesRush = int.Parse (parsedText[10]);
				} else {
					//				firstName = parsedText[0];  // not needed because this is team stats
					//				lastName = parsedText[1];
					rushYards = int.Parse (parsedText [3]);
					rushTD = int.Parse (parsedText [4]);
					fumblesRush = int.Parse (parsedText [6]);
					fumblesRushLost = int.Parse (parsedText [7]);
					//fumblesRush = int.Parse (parsedText[9]);
					rushAttemptsHOME += int.Parse (parsedText [2]);
					rushAttemptsAgainstAWAY += int.Parse (parsedText [2]);
					teamRushYardsHOME += rushYards;
					teamRushYardsDefAWAY += rushYards;
					teamTdHOME += rushTD;
					teamTdDefAWAY += rushTD;
					totalFumblesHOME += fumblesRush;
					totalFumblesLostHOME += fumblesRushLost;
				}
			}
			//Debug.Log ("Rusher " + firstName + " " + lastName + " added.");
			//Debug.Log(parsedText[0] + parsedText[1] + " added (team)");
		}

		for (int x = 0; x < receivingLinesAWAY.Length; x++) {
			string[] parsedText = receivingLinesAWAY[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			team = awayTeam;
			drops = 0;
			if (parsedText [0] != "-") {

				if (parsedText.Length > 8) {
					drops = int.Parse (parsedText [6]);
					fumblesRec = int.Parse (parsedText [8]);
					fumblesRecLost = int.Parse (parsedText [7]);
					totalDropsAWAY += drops;
					totalFumblesAWAY += fumblesRec;
					totalFumblesLostAWAY += fumblesRecLost;
					if (team == homeTeam) {
						totalDropsHOME += drops;
						totalFumblesHOME += fumblesRec;
						totalFumblesLostHOME += fumblesRecLost;
					}
				} else {
					drops = int.Parse (parsedText [5]);
					fumblesRec = int.Parse (parsedText [7]);
					fumblesRecLost = int.Parse (parsedText [6]);
					totalDropsAWAY += drops;
					totalFumblesAWAY += fumblesRec;
					totalFumblesLostAWAY += fumblesRecLost;
					if (team == homeTeam) {
						totalDropsHOME += drops;
						totalFumblesHOME += fumblesRec;
						totalFumblesLostHOME += fumblesRecLost;
					}
				}
			}
			//Debug.Log(parsedText[0] + parsedText[1] + " added (team)");
		}
		for (int x = 0; x < receivingLinesHOME.Length; x++) {
			string[] parsedText = receivingLinesHOME[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			team = homeTeam;
			drops = 0;
			if (parsedText [0] != "-") {
				if (parsedText.Length > 8) {
					drops = int.Parse (parsedText [6]);
					fumblesRec = int.Parse (parsedText [8]);
					fumblesRecLost = int.Parse (parsedText [7]);
					totalDropsHOME += drops;
					totalFumblesHOME += fumblesRec;
					totalFumblesLostHOME += fumblesRecLost;
				} else {
					drops = int.Parse (parsedText [5]);
					//Debug.Log (parsedText [1]);// + " " + parsedText [7]);
					fumblesRec = int.Parse (parsedText [7]);
					fumblesRecLost = int.Parse (parsedText [6]);
					totalDropsHOME += drops;
					totalFumblesHOME += fumblesRec;
					totalFumblesLostHOME += fumblesRecLost;
				}
			}
			//Debug.Log(parsedText[0] + parsedText[1] + " added (team)");
		}

		for(int x = 0; x < defenseLinesAWAY.Length; x++)
		{
			//Debug.Log ("Starting defense");
			string[] parsedText = defenseLinesAWAY[x].Split (' ' , '/', '\t');
			parsedText = parsedText.Where(j => j != "").ToArray();
			//Debug.Log (parsedText [0] + parsedText [1]);
			team = awayTeam;
			ff = int.Parse(parsedText[7]);
			fr = int.Parse(parsedText[8]);
			safety = int.Parse(parsedText[10]);
			defTD = int.Parse(parsedText[9]);
			teamSafetyAWAY += safety;
			teamSafetyOffenseHOME += safety;
			teamFfAWAY += ff;
			teamFrAWAY += fr;
			teamDefensiveTdAWAY += defTD;
			teamTdDefHOME += defTD;
			//Debug.Log(parsedText[0] + parsedText[1] + " added (team)");
			//Debug.Log ("Defender added.");
		}
		for(int x = 0; x < defenseLinesHOME.Length; x++)
		{
			string[] parsedText = defenseLinesHOME[x].Split (' ' , '/', '\t');
			parsedText = parsedText.Where(j => j != "").ToArray();
			team = homeTeam;

			ff = int.Parse(parsedText[7]);
			fr = int.Parse(parsedText[8]);
			safety = int.Parse(parsedText[10]);
			defTD = int.Parse(parsedText[9]);
			teamSafetyHOME += safety;
			teamSafetyOffenseAWAY += safety;
			teamFfHOME += ff;
			teamFrHOME += fr;
			teamDefensiveTdHOME += defTD;
			teamTdDefAWAY += defTD;
			//Debug.Log(parsedText[0] + parsedText[1] + " added (team)");
			//Debug.Log ("Defender added.");
		}

		string[] sacksText = results[sacksLine].Split (' ' , ',');
		sacksText = sacksText.Where(j => j != "").ToArray();
		teamSacksHOME += double.Parse(sacksText[2]);
		teamSacksOffenseAWAY += double.Parse(sacksText[2]);
		teamSacksAWAY += double.Parse(sacksText[1]);
		teamSacksOffenseHOME += double.Parse(sacksText[1]);

		string[] kickReturnsText = results [kickReturnLine].Split (' ', '\t');
		string[] puntReturnsText = results [puntReturnLine].Split (' ', '\t');
		kickReturnsText = kickReturnsText.Where(j => j != "").ToArray();
		puntReturnsText = puntReturnsText.Where(j => j != "").ToArray();
//		for (int x = 0; x < puntReturnsText.Length; x++) {
//			Debug.Log (puntReturnsText [x]);
//		}
		if (kickReturnsText.Length <= 5) { // no return TDs
			KRyardsAWAY = int.Parse (kickReturnsText [3]);
			KRyardsHOME = int.Parse (kickReturnsText [4]);
		} else if (kickReturnsText [4].Contains ("(") && kickReturnsText.Length <= 6) {  // away team had a TD
			KRyardsAWAY = int.Parse (kickReturnsText [3]);
			KRyardsHOME = int.Parse (kickReturnsText [5]);
			string td = kickReturnsText [4];
			td = td.Remove (0, 1);
			td = td.Remove (1, 1);
			KRtdAWAY += int.Parse (td);
		} else if (kickReturnsText [kickReturnsText.Length - 1].Contains ("(") && kickReturnsText.Length <= 6) {  // home team had a TD
			KRyardsAWAY = int.Parse (kickReturnsText [3]);
			KRyardsHOME = int.Parse (kickReturnsText [4]);
			string td = kickReturnsText [kickReturnsText.Length - 1];
			td = td.Remove (0, 1);
			td = td.Remove (1, 1);
			KRtdHOME += int.Parse (td);

		} else {
			KRyardsAWAY = int.Parse (kickReturnsText [3]);
			KRyardsHOME = int.Parse (kickReturnsText [5]);
			string td = kickReturnsText [kickReturnsText.Length - 1];
			td = td.Remove (0, 1);
			td = td.Remove (1, 1);
			KRtdHOME += int.Parse (td);
			td = kickReturnsText [4];
			td = td.Remove (0, 1);
			td = td.Remove (1, 1);
			KRtdAWAY += int.Parse (td);
		}

		if (puntReturnsText.Length <= 5) { // no return TDs
			PRyardsAWAY = int.Parse(puntReturnsText[3]);
			PRyardsHOME = int.Parse(puntReturnsText[4]);
		} else if (puntReturnsText [4].Contains ("(") && puntReturnsText.Length <= 6) {  // away team had a TD
			PRyardsAWAY = int.Parse (puntReturnsText [3]);
			PRyardsHOME = int.Parse (puntReturnsText [5]);
			string td = puntReturnsText [4];
			td = td.Remove (0, 1);
			td = td.Remove (1,1);
			PRtdAWAY += int.Parse (td);
		} else if (puntReturnsText [puntReturnsText.Length - 1].Contains ("(") && puntReturnsText.Length <= 6) {  // home team had a TD
			PRyardsAWAY = int.Parse (puntReturnsText [3]);
			PRyardsHOME = int.Parse (puntReturnsText [4]);
			string td = puntReturnsText [puntReturnsText.Length - 1];
			td = td.Remove (0, 1);
			td = td.Remove (1, 1);
			PRtdHOME += int.Parse (td);

		} else {
			PRyardsAWAY = int.Parse (puntReturnsText [3]);
			PRyardsHOME = int.Parse (puntReturnsText [5]);
			string td = puntReturnsText [puntReturnsText.Length - 1];
			td = td.Remove (0, 1);
			td = td.Remove (1, 1);
			PRtdHOME += int.Parse (td);
			td = puntReturnsText [4];
			td = td.Remove (0, 1);
			td = td.Remove (1, 1);
			PRtdAWAY += int.Parse (td);
		}

		int pryardsAWAY, pryardsHOME, prtdAWAY, prtdHOME, kryardsAWAY, kryardsHOME, krtdHOME, krtdAWAY;
		pryardsAWAY = PRyardsAWAY;
		pryardsHOME = PRyardsHOME;
		kryardsAWAY = KRyardsAWAY;
		kryardsHOME = KRyardsHOME;
		prtdHOME = PRtdHOME;
		prtdAWAY = PRtdAWAY;
		krtdHOME = KRtdHOME;
		krtdAWAY = KRtdHOME;

//		for(int x = 0; x < returnLines.Length; x++)
//		{
//			puntTD = 0;
//			kickTD = 0;
//
//			string[] parsedText = returnLines[x].Split (' ' , ',' , '(' , ')');
//			parsedText = parsedText.Where(j => j != "").ToArray();
//			team = convertTeamFromIcon(parsedText[0]);
//			//team = parsedText[parsedText.Length -1];
//			//Debug.Log (team);
//
//			if(parsedText[5] == "punt")
//			{
//				puntTD += 1;
//			}
//			else if(parsedText[5] == "kickoff")
//			{
//				kickTD += 1;
//			}
//			if(team == homeTeam)
//			{
//				teamStTdHOME += (kickTD + puntTD);
//				teamTdDefAWAY += (kickTD + puntTD);
//			}
//			else if (team == awayTeam)
//			{
//				teamStTdAWAY += (kickTD + puntTD);
//				teamTdDefHOME += (kickTD + puntTD);
//			}
//			//Debug.Log ("Returner " + firstName + " " + lastName + " added.");
//		}

		totalYardsHOME += (teamPassYardsHOME + teamRushYardsHOME);
		totalYardsAWAY += (teamPassYardsAWAY + teamRushYardsAWAY);
		totalYardsDefHOME += (teamPassYardsDefHOME + teamRushYardsDefHOME);
		totalYardsDefAWAY += (teamPassYardsDefAWAY + teamRushYardsDefAWAY);
		totalTdsHOME += (teamTdHOME + teamStTdHOME + teamDefensiveTdHOME);
		totalTdsAWAY += (teamTdAWAY + teamStTdAWAY + teamDefensiveTdAWAY);

//				Debug.Log ("Score: " + awayScore + "-" + homeScore);
		//		Debug.Log (olineLine);
		string[] olineParse = results[olineLine].Split (' ', '-');
		olineParse = olineParse.Where(k => k != "").ToArray();

		olineRatingAWAY = double.Parse (olineParse[3]);
		olineRatingHOME = double.Parse (olineParse[4]);

		//		Debug.Log ("Home: " + olineRatingHOME);
		//		Debug.Log ("Away: " + olineRatingAWAY);

		string[] puntParse = results[puntingLine].Split (' ', '/');
		puntParse = puntParse.Where(k => k != "").ToArray();

		puntAvgAWAY = double.Parse (puntParse[1]);
		puntAvgHOME = double.Parse (puntParse[2]);

		string[] penaltyParse = results[penaltyLine].Split (' ', '/');

		penaltyNumberHOME = int.Parse (penaltyParse[3]);
		penaltyYardsHOME = int.Parse (penaltyParse[4]);
		penaltyNumberAWAY = int.Parse (penaltyParse[1]);
		penaltyYardsAWAY = int.Parse (penaltyParse[2]);

		string[] thirdDownParse = results[thirdDownLine].Split (' ', '/');

		thirdconvertedHOME = int.Parse (thirdDownParse[5]);
		thirdtotalHOME = int.Parse (thirdDownParse[6]);
		thirdconvertedAWAY = int.Parse (thirdDownParse[3]);
		thirdtotalAWAY = int.Parse (thirdDownParse[4]);

		thirdconverteddefHOME = thirdconvertedAWAY;
		thirdconverteddefAWAY = thirdconvertedHOME;
		thirdtotaldefHOME = thirdtotalAWAY;
		thirdtotaldefAWAY = thirdtotalHOME;

		//		Debug.Log ("Penalties - H: " + penaltyNumberHOME + " | Yards: " + penaltyYardsHOME);
		//		Debug.Log ("Penalties - A: " + penaltyNumberAWAY + " | Yards: " + penaltyYardsAWAY);

		//		Debug.Log ("Home: " + puntAvgHOME);
		//		Debug.Log ("Away: " + puntAvgAWAY);

		NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=mighty;Database=2023 NFL Stats;");
		string sqlString = "SELECT * FROM teams WHERE team = '" + homeTeam + "';";

		NpgsqlCommand command = new NpgsqlCommand(sqlString, conn);
		try
		{
			conn.Open();
			//console.text += "Connected to db...\n";
			//Debug.Log ("Adding team stats");
			NpgsqlDataReader dr = command.ExecuteReader();

			while(dr.Read())
			{
//				if(homeTeam == "PHI" ){
//					Debug.Log(homeTeam + ": " + homeScore + " + " + (int)dr[2]);
//					Debug.Log(awayTeam + ": " + awayScore + " + " + (int)dr[3]);
//				}
				homeScore += (int)dr[2];
				awayScore += (int)dr[3];
				teamPassYardsHOME += (int)dr[5];
				teamRushYardsHOME += (int)dr[6];
				teamPassYardsDefHOME += (int)dr[14];
				teamRushYardsDefHOME += (int)dr[15];
				totalYardsDefHOME += (int)dr[16];
				teamTdHOME += (int)dr[10];
				teamDefensiveTdHOME += (int)dr[11];
				//teamStTdHOME += (int)dr[12];
				teamStTdHOME += (int)dr[65] + (int)dr[67];
				totalTdsHOME += (int)dr[13];
				teamIntOffHOME += (int)dr[24];
				teamIntDefHOME += (int)dr[25];
				totalYardsHOME += (int)dr[31];
				teamTdDefHOME += (int)dr[33];
				teamSacksHOME += (double)dr[21];
				teamSacksOffenseHOME += (double)dr[20];
				teamSafetyHOME += (int)dr[23];
				teamSafetyOffenseHOME += (int)dr[22];
				teamFfHOME += (int)dr[26];
				teamFrHOME += (int)dr[27];
				games = (int)dr[32] + 1;
				olineRatingHOME += (double)dr[38];
				puntAvgHOME += (double)dr[39];
				rushAttemptsHOME += (int)dr[40];
				passAttemptsHOME += (int)dr[41];
				rushAttemptsAgainstHOME += (int)dr[42];
				passAttemptsAgainstHOME += (int)dr[43];
				penaltyNumberHOME += (int)dr[50];
				penaltyYardsHOME += (int)dr[51];
				totalDropsHOME += (int)dr[52];
				totalFumblesHOME += (int)dr[53];
				totalFumblesLostHOME += (int)dr[54];
				thirdconvertedHOME += (int)dr[55];
				thirdtotalHOME += (int)dr[56];
				thirdconverteddefHOME += (int)dr[58];
				thirdtotaldefHOME += (int)dr[59];
				PRyardsHOME += (int)dr[61];
				PRyardsAWAY += (int)dr[62];
				KRyardsHOME += (int)dr[63];
				KRyardsAWAY += (int)dr[64];
				PRtdHOME += (int)dr[65];
				PRtdAWAY += (int)dr[66];
				KRtdHOME += (int)dr[67];
				KRtdAWAY += (int)dr[68];

			}

		}

		finally
		{
			conn.Close();
		}

		try
		{
			conn.Open();

			string sqlString2 = "UPDATE teams SET passyds = " + teamPassYardsHOME + ", passydsdef = " + teamPassYardsDefHOME + ", tdoffense = " + teamTdHOME + ", tdgivenup = " + teamTdDefHOME + ", tddefense = " 
				+ teamDefensiveTdHOME + ", intoff = " + teamIntOffHOME + ", intdef = " + teamIntDefHOME + ", rushyds =" + teamRushYardsHOME + ", rushydsdef = " + teamRushYardsDefHOME + ", totalyds = " 
				+ totalYardsHOME + ", totalydsdef = " + totalYardsDefHOME + ", ff = " + teamFfHOME + ", fr = " + teamFrHOME + ", sacksoff = " + teamSacksOffenseHOME + ", sacksdef = " + teamSacksHOME + 
				", safetyoff = " + teamSafetyOffenseHOME + ", safetydef = " + teamSafetyHOME + ", tdst = " + teamStTdHOME + ", tdtotal = " + totalTdsHOME + ", games = " + games + ", pointsfor = " + homeScore + 
				", pointsagainst = " + awayScore + ", olinerating = " + olineRatingHOME + ", puntavg = " + puntAvgHOME + ", rushattempts = " + rushAttemptsHOME + ", passattempts = " + passAttemptsHOME + 
				", rushattemptsagainst = " + rushAttemptsAgainstHOME + ", passattemptsagainst = " + passAttemptsAgainstHOME + ", penalties = " + penaltyNumberHOME + ", penalty_yards = " + penaltyYardsHOME
				+ ", drops = " + totalDropsHOME + ", fumbles = " + totalFumblesHOME + ", fumlost = " + totalFumblesLostHOME + ", thirdconverted = " + thirdconvertedHOME + ", thirdtotal = " + thirdtotalHOME + 
				", thirdconverteddef = " + thirdconverteddefHOME + ", thirdtotaldef = " + thirdtotaldefHOME + ", pryards = " + PRyardsHOME + ", pryardsagainst = " + PRyardsAWAY + ", kryards = " + KRyardsHOME + 
				", kryardsagainst = " + KRyardsAWAY + ", prtd = " + PRtdHOME + ", prtdagainst = " + PRtdAWAY + ", krtd = " + KRtdHOME + ", krtdagainst = " + KRtdAWAY + " WHERE team = '" + homeTeam + "';";

			NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);
			int rowsaffected;
			rowsaffected = WriteCommand.ExecuteNonQuery();
			//Debug.Log (rowsaffected);
		}
		finally
		{
			conn.Close();
		}

		awayScore = int.Parse(scoreAwayText [scoreAwayText.Length - 1]);
		homeScore = int.Parse(scoreHomeText [scoreHomeText.Length - 1]);
		PRyardsAWAY = pryardsAWAY;
		PRyardsHOME = pryardsHOME;
		KRyardsAWAY = kryardsAWAY;
		KRyardsHOME = kryardsHOME;
		PRtdHOME = prtdHOME;
		PRtdAWAY = prtdAWAY;
		KRtdHOME = krtdHOME;
		KRtdHOME = krtdAWAY;

		sqlString = "SELECT * FROM teams WHERE team = '" + awayTeam + "';";

		command = new NpgsqlCommand(sqlString, conn);
		try
		{
			conn.Open();
			//console.text += "Connected to db...\n";
			Debug.Log ("Adding team stats");
			NpgsqlDataReader dr = command.ExecuteReader();

			while(dr.Read())
			{
//				if(awayTeam == "PHI"){
//					Debug.Log(homeTeam + ": " + homeScore + " + " + (int)dr[3]);
//					Debug.Log(awayTeam + ": " + awayScore + " + " + (int)dr[2]);}
				awayScore += (int)dr[2];
				homeScore += (int)dr[3];
				teamPassYardsAWAY += (int)dr[5];
				//Debug.Log ("PASS YARDS: " + teamPassYardsAWAY);
				teamRushYardsAWAY += (int)dr[6];
				teamPassYardsDefAWAY += (int)dr[14];
				teamRushYardsDefAWAY += (int)dr[15];
				totalYardsDefAWAY += (int)dr[16];
				teamTdAWAY += (int)dr[10];
				teamDefensiveTdAWAY += (int)dr[11];
				//teamStTdAWAY += (int)dr[12];
				teamStTdAWAY = (int)dr[65] + (int)dr[67];
				totalTdsAWAY += (int)dr[13];
				teamIntOffAWAY += (int)dr[24];
				teamIntDefAWAY += (int)dr[25];
				totalYardsAWAY += (int)dr[31];
				teamTdDefAWAY += (int)dr[33];
				teamSacksAWAY += (double)dr[21];
				teamSacksOffenseAWAY += (double)dr[20];
				teamSafetyAWAY += (int)dr[23];
				teamSafetyOffenseAWAY += (int)dr[22];
				teamFfAWAY += (int)dr[26];
				teamFrAWAY += (int)dr[27];
				games = (int)dr[32] + 1;
				olineRatingAWAY += (double)dr[38];
				puntAvgAWAY += (double)dr[39];
				rushAttemptsAWAY += (int)dr[40];
				passAttemptsAWAY += (int)dr[41];
				rushAttemptsAgainstAWAY += (int)dr[42];
				passAttemptsAgainstAWAY += (int)dr[43];
				penaltyNumberAWAY += (int)dr[50];
				penaltyYardsAWAY += (int)dr[51];
				totalDropsAWAY += (int)dr[52];
				totalFumblesAWAY += (int)dr[53];
				totalFumblesLostAWAY += (int)dr[54];
				thirdconvertedAWAY += (int)dr[55];
				thirdtotalAWAY += (int)dr[56];
				thirdconverteddefAWAY += (int)dr[58];
				thirdtotaldefAWAY += (int)dr[59];
				PRyardsAWAY += (int)dr[61];
				PRyardsHOME += (int)dr[62];
				KRyardsAWAY += (int)dr[63];
				KRyardsHOME += (int)dr[64];
				PRtdAWAY += (int)dr[65];
				PRtdHOME += (int)dr[66];
				KRtdAWAY += (int)dr[67];
				KRtdHOME += (int)dr[68];
				//Debug.Log (rushAttemptsAgainstAWAY + " rush attempts against " + awayTeam);
				//Debug.Log (passAttemptsAgainstAWAY + " pass attempts against " + awayTeam);
			}
		}

		finally
		{
			conn.Close();
		}

		try
		{
			conn.Open();

			string sqlString2 = "UPDATE teams SET passyds = " + teamPassYardsAWAY + ", passydsdef = " + teamPassYardsDefAWAY + ", tdoffense = " + teamTdAWAY + ", tdgivenup = " + teamTdDefAWAY + ", tddefense = " 
				+ teamDefensiveTdAWAY + ", intoff = " + teamIntOffAWAY + ", intdef = " + teamIntDefAWAY + ", rushyds =" + teamRushYardsAWAY + ", rushydsdef = " + teamRushYardsDefAWAY + ", totalyds = " 
				+ totalYardsAWAY + ", totalydsdef = " + totalYardsDefAWAY + ", ff = " + teamFfAWAY + ", fr = " + teamFrAWAY + ", sacksoff = " + teamSacksOffenseAWAY + ", sacksdef = " + teamSacksAWAY + 
				", safetyoff = " + teamSafetyOffenseAWAY + ", safetydef = " + teamSafetyAWAY + ", tdst = " + teamStTdAWAY + ", tdtotal = " + totalTdsAWAY + ", games = " + games + ", pointsfor = " + awayScore + 
				", pointsagainst = " + homeScore + ", olinerating = " + olineRatingAWAY + ", puntavg = " + puntAvgAWAY + ", rushattempts = " + rushAttemptsAWAY + ", passattempts = " + passAttemptsAWAY + 
				", rushattemptsagainst = " + rushAttemptsAgainstAWAY + ", passattemptsagainst = " + passAttemptsAgainstAWAY + ", penalties = " + penaltyNumberAWAY + ", penalty_yards = " + penaltyYardsAWAY 
				+ ", drops = " + totalDropsAWAY + ", fumbles = " + totalFumblesAWAY + ", fumlost = " + totalFumblesLostAWAY + ", thirdconverted = " + thirdconvertedAWAY + ", thirdtotal = " + thirdtotalAWAY + 
				", thirdconverteddef = " + thirdconverteddefAWAY + ", thirdtotaldef = " + thirdtotaldefAWAY + ", pryards = " + PRyardsAWAY + ", pryardsagainst = " + PRyardsHOME + ", kryards = " + KRyardsAWAY + 
				", kryardsagainst = " + KRyardsHOME + ", prtd = " + PRtdAWAY + ", prtdagainst = " + PRtdHOME + ", krtd = " + KRtdAWAY + ", krtdagainst = " + KRtdHOME + " WHERE team = '" + awayTeam + "';";

			NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);
			int rowsaffected;
			rowsaffected = WriteCommand.ExecuteNonQuery();
			//Debug.Log (rowsaffected);
		}
		finally
		{
			conn.Close();
		}


	}

	public void doTeamRateStats(int index)
	{
		NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=mighty;Database=2023 NFL Stats;");
		string sqlString = "SELECT * FROM teams WHERE id = " + index + ";";
		int pointdif = 0;
		int totalplays = 0, totalplaysagainst = 0;
		double passydspg, rushydspg, totalydspg, passydsdefpg, rushydsdefpg, totalydsdefpg, ppgfor, ppgagainst, teamypc, teamypca, teamypa, teamypaa;
		double puntAvg = 0.0;
		double olineAvg = 0.0;
		passydspg = rushydspg = totalydspg = passydsdefpg = rushydsdefpg = totalydsdefpg = ppgfor = ppgagainst = teamypc = teamypca = teamypa = teamypaa = thirdrateOffense = thirdrateDefense = 0.0;

		NpgsqlCommand command = new NpgsqlCommand(sqlString, conn);
		try
		{
			conn.Open();
			//console.text += "Connected to db...\n";
			NpgsqlDataReader dr = command.ExecuteReader();

			while(dr.Read())
			{
				games = (int)dr[32];
				double gamesDouble = (double)games;
				string passydspgString = ((double) ((int)dr[5] / (double)games)).ToString ("N" , nfi);
				passydspg = double.Parse (passydspgString);
				//Debug.Log (dr[5] + " : " + games);
				string rushydspgString = ((double) ((int)dr[6] / (double)games)).ToString ("N" , nfi);
				rushydspg = double.Parse (rushydspgString);
				string totalydspgString = ((double) ((int)dr[31] / (double)games)).ToString ("N" , nfi);
				totalydspg = double.Parse (totalydspgString);

				string passydsdefpgString = ((double) ((int)dr[14] / (double)games)).ToString ("N" , nfi);
				passydsdefpg = double.Parse (passydsdefpgString);
				string rushydsdefpgString = ((double) ((int)dr[15] / (double)games)).ToString ("N" , nfi);
				rushydsdefpg = double.Parse (rushydsdefpgString);
				string totalydsdefpgString = ((double) ((int)dr[16] / (double)games)).ToString ("N" , nfi);
				totalydsdefpg = double.Parse (totalydsdefpgString);

				pointdif = (int)dr[2] - (int)dr[3];

				teamPointsForHOME = (int)dr[2];
				teamPointsAgainstHOME = (int)dr[3];

				string ppgstring = ((double)((int)dr[2] / (double)games)).ToString ("N", nfi);
				ppgfor = double.Parse (ppgstring);

				string ppgagainststring = ((double)((int)dr[3] / (double)games)).ToString ("N", nfi);
				ppgagainst = double.Parse (ppgagainststring);

				if(teamPointsForHOME != 0)
				{
					string oyppstring = ((double) ((int)dr[31] / (double)teamPointsForHOME)).ToString ("N" , nfi);
					offensiveYPP = double.Parse (oyppstring);
				}
				else
				{
					offensiveYPP = 0.0;
				}

				if(teamPointsAgainstHOME != 0)
				{
					string dyppstring = ((double) ((int)dr[16] / (double)teamPointsAgainstHOME)).ToString ("N" , nfi);
					defensiveYPP = double.Parse (dyppstring);
				}
				else
				{
					defensiveYPP = 0.0;
				}

				string olinestring = ((double)dr[38] / gamesDouble).ToString ("N" , nfi);
				olineAvg = double.Parse (olinestring);

				string puntstring =  ((double)dr[39] / gamesDouble).ToString ("N" , nfi);
				puntAvg = double.Parse (puntstring);

				double carries = (double)((int)dr[40]);
				double attempts = (double)((int)dr[41]);
				double carriestAgainst = (double)((int)dr[42]);
				double attemptsAgainst = (double)((int)dr[43]);

				string ypcstring =  ((double) ((int)dr[6] / carries)).ToString ("N" , nfi);
				teamypc = double.Parse (ypcstring);

				string ypastring =  ((double) ((int)dr[5] / attempts)).ToString ("N" , nfi);
				teamypa = double.Parse (ypastring);

				string ypcastring =  ((double) ((int)dr[15] / carriestAgainst)).ToString ("N" , nfi);
				teamypca = double.Parse (ypcastring);

				string ypaastring =  ((double) ((int)dr[14] / attemptsAgainst)).ToString ("N" , nfi);
				teamypaa = double.Parse (ypaastring);

				double sacksOff = (double)dr[20], sacksDef = (double)dr[21];
				int sacksTaken = (int)Math.Ceiling (sacksOff);
				int sacks = (int)Math.Ceiling (sacksDef);

				totalplays = (int)dr[40] + (int)dr[41] + sacksTaken;
				totalplaysagainst = (int)dr[42] + (int)dr[43] + sacks;

				double thirdDownAttempts = (double)((int)dr[56]);
				//double thirdDownConversions = (double)((int)dr[55]);
				string thirdDownString = ((double) (((int)dr[55] / thirdDownAttempts)* 100)).ToString("N" , nfi);
				thirdrateOffense = double.Parse(thirdDownString);
				//thirdrateOffense = thirdrateOffense;

				double thirdDownAttemptsDefense = (double)((int)dr[59]);
				string thirdDownDefenseString = ((double) (((int)dr[58] / thirdDownAttemptsDefense)* 100)).ToString("N" , nfi);
				thirdrateDefense = double.Parse(thirdDownDefenseString);

			}
		}

		finally
		{
			conn.Close();
		}

		try
		{
			conn.Open();
			Debug.Log ("UPDATE teams SET passydspg = " + passydspg + ", rushydspg = " + rushydspg + ", totalydspg = " + totalydspg + ", passydsdefpg = " + passydsdefpg + ", rushydsdefpg = " + rushydsdefpg 
				+ ", totalydsdefpg = " + totalydsdefpg + ", pointdif = " + pointdif + ", ppgfor = " + ppgfor + ", ppgagainst = " + ppgagainst + ", oypp = " + offensiveYPP + ", dypp = " + defensiveYPP
				+ ", olinerating = " + olineAvg + ", puntavg = " + puntAvg + ", totalplays = " + totalplays + ", totalplaysagainst = " + totalplaysagainst + ", teamypc = " + teamypc 
				+ ", teamypa = " + teamypa + ", teamypca = " + teamypca + ", teamypaa = " + teamypaa +", thirdrate = " + thirdrateOffense + " WHERE id = " + index + ";");

			string sqlString2 = "UPDATE teams SET passydspg = " + passydspg + ", rushydspg = " + rushydspg + ", totalydspg = " + totalydspg + ", passydsdefpg = " + passydsdefpg + ", rushydsdefpg = " + rushydsdefpg 
				+ ", totalydsdefpg = " + totalydsdefpg + ", pointdif = " + pointdif + ", ppgfor = " + ppgfor + ", ppgagainst = " + ppgagainst + ", oypp = " + offensiveYPP + ", dypp = " + defensiveYPP
				+ ", olinerating = " + olineAvg + ", puntavg = " + puntAvg + ", totalplays = " + totalplays + ", totalplaysagainst = " + totalplaysagainst + ", teamypc = " + teamypc 
				+ ", teamypa = " + teamypa + ", teamypca = " + teamypca + ", teamypaa = " + teamypaa + ", thirdrate = " + thirdrateOffense + ", thirdratedef = " + thirdrateDefense + " WHERE id = " + index + ";";

			NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);

			int rowsaffected;
			rowsaffected = WriteCommand.ExecuteNonQuery();
			//Debug.Log (rowsaffected);
		}
		finally
		{
			conn.Close();
		}

	}

	public void parseAllGames(string year)
	{
		int x = 1;
		string file = Application.dataPath + "/GameReports/" + x + ".txt";
		while (File.Exists (file))
		{
			//Debug.Log ("Game: " + x);
			Debug.Log ("Parsing log #" + x);
			parseGameLog(x);
			x++;
			file = Application.dataPath + "/GameReports/" + x + ".txt";
		}
		Debug.Log ("Calculating team rate stats");
		for(int y = 1; y < 33; y++)
		{
			doTeamRateStats(y);
		}
		Debug.Log ("DONE!");
	}

	// Use this for initialization
	void Start () {
		populateTeamArrays();
		//		Debug.Log (teamAbrevs[16]);
		//		Debug.Log (teamNamesFull[16]);
		//		for(int x = 0; x < teamAbrevs.Length; x++)
		//			Debug.Log (teamAbrevs[x]);
	}

	// Update is called once per frame
	void Update () {

	}
}