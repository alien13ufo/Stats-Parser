using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using System.Globalization;

public class ParseGameReport : MonoBehaviour {

	public Text fileNameInput;
	public Text console;
	private string fileName;
	private string fileDirectory;
	private string[] results;
	private string[] passingLines;
	private string[] rushingLines;
	private string[] receivingLines;
	private string[] defenseLines;
	private string[] kickingLines;
	private string[] returnLines;
	private string scoreLine;
	private int olineLine, puntingLine, penaltyLine, sacksLine, thirdDownLine;
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
	private int tackles;
	private double sacks;
	private int intDef;
	private int ff;
	private int fr;
	private int safety;
	private int defTD;
	private int block;
	private int kickAttempts = 0;
	private int kickMiss = 0;
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

	public void parseGameLog(int file = -1)
	{
		if(file == -1)
		{
			fileName = fileNameInput.text;
		}
		else
		{
			fileName = file.ToString ();
		}
		fileDirectory = Application.dataPath + "/GameReports/" + fileName + ".txt";
		//StreamReader reader=new  StreamReader(fileDirectory);

		results = System.IO.File.ReadAllLines(fileDirectory);
		results = results.Where(x => x != "").ToArray(); // remove empty lines
		results = results.Where (x => x!= " ").ToArray (); // sometimes they have a space

//		for(int x = 0; x < results.Length; x++)
//		{
//			console.text += results[x] + "\n";
//		}
		int passIndex = 0;
		int rushIndex = 0;
		int receivingIndex = 0;
		int defenseIndex = 0;
		int injuriesIndex = 0;
		int numKicks = 0;
		int numReturns = 0;
		int kickingIndex = 0;

		gameLine = results[0];
		for(int x = 0; x < results.Length; x++)
		{
			if (results [x] == "Passing Leaders") {
				passIndex = x;
			} else if (results [x] == "Rushing Leaders") {
				rushIndex = x;
			} else if (results [x] == "Receiving Leaders") {
				receivingIndex = x;
			} else if (results [x] == "Defensive Leaders") {
				defenseIndex = x;
			} else if (results [x] == "Injuries") {
				injuriesIndex = x;
			} else if (results [x].IndexOf ("yd FG") != -1) {
				numKicks++;
			} else if (results [x].IndexOf ("punt return") != -1 || results [x].IndexOf ("kickoff return") != -1) {
				numReturns++;
			}
			else if(results[x].IndexOf("Kicking Leaders") != -1) {
				kickingIndex = x;
			}
			//Debug.Log (passIndex + " : " + rushIndex + " : " + receivingIndex + " : " + defenseIndex + " : " + injuriesIndex);
		}
		//Debug.Log (passIndex + " : " + rushIndex + " : " + receivingIndex + " : " + defenseIndex + " : " + injuriesIndex);

		returnLines = new string[numReturns];
		int i = 0;
		for(int x = 0; x < passIndex; x++)
		{
			if(results[x].IndexOf ("punt return") != -1 || results[x].IndexOf ("kickoff return") != -1)
			{
				returnLines[i] = results[x];
				i++;
			}
		}
		
//		kickingLines = new string[numKicks];
//		int j = 0;
//		for(int x = 0; x < passIndex; x++)
//		{
//			if(results[x].IndexOf ("yd FG") != -1)
//			{
//				kickingLines[j] = results[x];
//				//Debug.Log (kickingLines[j]);
//				j++;
//			}
//		}

		for(int x = 0; x < results.Length; x++)
		{
			if(results[x].StartsWith ("FINAL SCORE:"))
			{
				scoreLine = results[x];
//				olineLine = results[x+1];
//				puntingLine = results[x+2];
				//break;
			}
			else if(results[x].StartsWith ("Offensive Line"))
			{
				olineLine = x;
			}

			else if(results[x].StartsWith ("Punting"))
			{
				puntingLine = x;
			}
			else if(results[x].StartsWith ("Penalties"))
			{
				penaltyLine = x;
			}
			else if(results[x].StartsWith ("Sacks"))
			{
				sacksLine = x;
			}
			else if (results[x].StartsWith("3rd Down")) {
				thirdDownLine = x;
			}

		}
		Debug.Log (rushIndex + " - " + passIndex);
		Debug.Log (rushIndex - passIndex -1);
		Debug.Log (receivingIndex - rushIndex -1);
		Debug.Log (injuriesIndex - defenseIndex - 1);
//		Debug.Log (injuriesIndex - passIndex -1);
		passingLines = new string[rushIndex - passIndex -1];
		rushingLines = new string[receivingIndex - rushIndex -1];
		receivingLines = new string[defenseIndex - receivingIndex - 1];
		defenseLines = new string[kickingIndex - defenseIndex - 1];
		kickingLines = new string[injuriesIndex - kickingIndex - 1];
		allLines = new string[injuriesIndex - passIndex -1];
		//Debug.Log(injuriesIndex + " " + kickingIndex);

		for(int x =0; x < allLines.Length; x++)
		{
			allLines[x] = results[x+passIndex+1];
		}
		Debug.Log ("all lines in");
		for(int x = 0; x < passingLines.Length; x++)
		{
			passingLines[x] = results[x+passIndex +1];
			//console.text += passingLines[x] + "\n";
		}
		Debug.Log ("pass lines in");
		for(int x = 0; x < rushingLines.Length; x++)
		{
			rushingLines[x] = results[x+rushIndex +1];
			//console.text += rushingLines[x] + "\n";
		}
		Debug.Log ("rush lines in");
		for(int x = 0; x < receivingLines.Length; x++)
		{
			receivingLines[x] = results[x+receivingIndex +1];
			//console.text += receivingLines[x] + "\n";
		}
		Debug.Log ("receiving lines in");
		for(int x = 0; x < defenseLines.Length; x++)
		{
			defenseLines[x] = results[x+defenseIndex +1];
			//console.text += defenseLines[x] + "\n";
		}
		Debug.Log ("defense lines in");
		for (int x = 0; x < kickingLines.Length; x++) {
			kickingLines [x] = results [x + kickingIndex + 1];
		}

		parseLines();

//		for(int x = 0; x < results.Length; x++)
//		{
//			console.text += passingLines[x] + "\n";
//		}

	}

	public void parseLines()
	{
		string[] teamParse = gameLine.Split ('(',')' );
		awayTeam = teamParse[0];
		string tempHomeTeam = teamParse[teamParse.Length - 3];
		string[] homeParse = tempHomeTeam.Split (' ');
	
		homeTeam = String.Join (" ", homeParse, 2, homeParse.Length - 2);
		homeTeam = convertTeam(homeTeam);
		awayTeam = convertTeam(awayTeam);

		for(int x = 0; x < passingLines.Length; x++)
		{
			completions = 0;
			passAttempts = 0;
			passYards = 0;
			passTD = 0;
			interceptions = 0;
			safety = 0;
			qbFumbles = 0;
			qbFumblesLost = 0;

			string[] parsedText = passingLines[x].Split (' ' , ',', '/');
			parsedText = parsedText.Where(j => j != "").ToArray();

//			for (int y = 0; y < parsedText.Length; y++) {
//				Debug.Log ("[" + y + "] " + parsedText [y]);
//			}

			firstName = parsedText[0];
			lastName = parsedText[1];
			team = parsedText[2];
			completions = int.Parse (parsedText[3]);
			passAttempts = int.Parse(parsedText[4]);
			passYards = int.Parse(parsedText[5]);
			passTD = int.Parse(parsedText[7]);
			interceptions = int.Parse (parsedText[9]);
			qbFumbles = int.Parse (parsedText [11]);
			qbFumblesLost = int.Parse (parsedText [12]);

			sendToDB("passing");
		}
		for(int x = 0; x < rushingLines.Length; x++)
		{
			rushAttempts = 0;
			rushYards = 0;
			rushTD = 0;
			fumblesRush = 0;
			fumblesRushLost = 0;
			hundredYardGames = 0;

			string[] parsedText = rushingLines[x].Split (' ' , ',', '/');
			parsedText = parsedText.Where(j => j != "").ToArray();

//			for (int y = 0; y < parsedText.Length; y++) {
//				Debug.Log ("[" + y + "] " + parsedText [y]);
//			}

			if(parsedText[5] == "for")
			{
				//Debug.Log (parsedText[1] + " " + parsedText[2]);
				firstName = parsedText[0] + " " + parsedText[1];
				lastName = parsedText[2];
				team = parsedText [3];
				rushAttempts = int.Parse (parsedText[4]);
				rushYards = int.Parse (parsedText[6]);
				rushTD = int.Parse (parsedText[8]);
				fumblesRush = int.Parse (parsedText[10]);
				fumblesRushLost = int.Parse(parsedText[11]);
			}
			else
			{
				//Debug.Log (parsedText[1] + " " + parsedText[2]);
				firstName = parsedText[0];
				lastName = parsedText[1];
				team = parsedText [2];
				rushAttempts = int.Parse (parsedText[3]);
				rushYards = int.Parse (parsedText[5]);
				rushTD = int.Parse (parsedText[7]);
				fumblesRush = int.Parse (parsedText[9]);
				fumblesRushLost = int.Parse(parsedText[10]);
			}
			if(rushYards >= 100)
				hundredYardGames = 1;

			sendToDB("rushing");
		}
		for(int x = 0; x < receivingLines.Length; x++)
		{
			fumblesRec = 0;
			fumblesRecLost = 0;
			receivingYards = 0;
			catches = 0;
			receivingTD = 0;
			hundredYardGames = 0;
			drops = 0;
			string[] parsedText = receivingLines[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
			if(parsedText[5] == "for")
			{
				firstName = parsedText[0] + " " + parsedText[1];
				lastName = parsedText[2];
				if(firstName.IndexOf ("'") != -1)
				{
					firstName = firstName.Insert (firstName.IndexOf ("'") , "\'");
					//Debug.Log (firstName);
				}
				if(lastName.IndexOf ("'") != -1)
				{
					lastName = lastName.Insert (lastName.IndexOf ("'") , "\'");
					//Debug.Log (lastName);
				}
				team = parsedText [3];
				catches = int.Parse (parsedText[4]);
				receivingYards = int.Parse (parsedText[6]);
				receivingTD = int.Parse (parsedText[8]);
				drops = int.Parse (parsedText [10]);
				fumblesRec = int.Parse (parsedText[12]);
				fumblesRecLost = int.Parse (parsedText [13]);
			}
			else
			{
				firstName = parsedText[0];
				lastName = parsedText[1];
				if(firstName.IndexOf ("'") != -1)
				{
					firstName = firstName.Insert (firstName.IndexOf ("'") , "\'");
					//Debug.Log (firstName);
				}
				if(lastName.IndexOf ("'") != -1)
				{
					lastName = lastName.Insert (lastName.IndexOf ("'") , "\'");
					//Debug.Log (lastName);
				}
				team = parsedText[2];
				catches = int.Parse (parsedText[3]);
				receivingYards = int.Parse (parsedText[5]);
				receivingTD = int.Parse (parsedText[7]);
				drops = int.Parse (parsedText[9]);
				fumblesRec = int.Parse (parsedText [11]);
				fumblesRecLost = int.Parse (parsedText [12]);

			}

			sendToDB("receiving");
		}
		for(int x = 0; x < defenseLines.Length; x++)
		{
			string[] parsedText = defenseLines[x].Split (' ' , ',');
			parsedText = parsedText.Where(j => j != "").ToArray();
			firstName = parsedText[0];
			lastName = parsedText[1];
			if(firstName.IndexOf ("'") != -1)
			{
				firstName = firstName.Insert (firstName.IndexOf ("'") , "\'");
				//Debug.Log (firstName);
			}
			if(lastName.IndexOf ("'") != -1)
			{
				lastName = lastName.Insert (lastName.IndexOf ("'") , "\'");
				//Debug.Log (lastName);
			}
			team = parsedText[2];

			int tacklesIndex = (Array.IndexOf(parsedText, "Tackles") -1);
			if(tacklesIndex < 0)
				tacklesIndex = (Array.IndexOf(parsedText, "Tackle") -1);
			if(tacklesIndex >=0)
				tackles = int.Parse (parsedText[tacklesIndex]);
			else
				tackles = 0;

			int intIndex = (Array.IndexOf(parsedText, "INT") -1);
			if(intIndex >=0)
				intDef = int.Parse (parsedText[intIndex]);
			else
				intDef = 0;

			int ffIndex = (Array.IndexOf(parsedText, "FF") -1);
			if(ffIndex >= 0)
				ff = int.Parse (parsedText[ffIndex]);
			else
				ff = 0;

			int frIndex = (Array.IndexOf(parsedText, "FR") -1);
			if(frIndex >= 0)
				fr = int.Parse (parsedText[frIndex]);
			else
				fr = 0;

			int safetyIndex = (Array.IndexOf(parsedText, "Safety") -1);
			if(safetyIndex >= 0)
				safety = int.Parse (parsedText[safetyIndex]);
			else
				safety = 0;

			int defTDIndex = (Array.IndexOf(parsedText, "TD") -1);
			if(defTDIndex >= 0)
				defTD = int.Parse (parsedText[defTDIndex]);
			else
				defTD = 0;

			int sacksIndex = (Array.IndexOf(parsedText, "Sacks") -1);
			if(sacksIndex < 0)
				sacksIndex = (Array.IndexOf(parsedText, "Sack") -1);
			if(sacksIndex >= 0)
				sacks = double.Parse (parsedText[(sacksIndex)]);
			else
				sacks = 0;

			Debug.Log (firstName + " " + lastName + " Tackles: " + tackles + " - Sacks: " + sacks + " - INTS: " + intDef);
			sendToDB("defense");
		}

		for(int x = 0; x < kickingLines.Length; x++)
		{
			kickAttempts = 0;
			kickMiss = 0;
			kickPercent = 0;
			kickFifty = 0;
			kickFourty = 0;
			kickThirty = 0;
			kickTwenty = 0;
			kickZero = 0;
			fgDistance = 0;
			string[] parsedText = kickingLines[x].Split (' ' , ',' , '-' , '/', '(', ')' );
			parsedText = parsedText.Where(j => j != "").ToArray();
			firstName = parsedText[0];
			lastName = parsedText[1];
			if(firstName.IndexOf ("'") != -1)
			{
				firstName = firstName.Insert (firstName.IndexOf ("'") , "\'");
				//Debug.Log (firstName);
			}
			if(lastName.IndexOf ("'") != -1)
			{
				lastName = lastName.Insert (lastName.IndexOf ("'") , "\'");
				//Debug.Log (lastName);
			}
			kickAttempts = int.Parse(parsedText[4]);
			int kicksMade = int.Parse(parsedText[3]);
			kickMiss = kickAttempts - kicksMade;
			Debug.Log (firstName + " " + lastName + " - " + kicksMade + "/" + kickAttempts);
			if(kickAttempts != 0)
			{
				for (int y = 0; y < kickAttempts - kickMiss; y++) {
					fgDistance = int.Parse(parsedText [y + 5]);
					if(fgDistance < 20)
					{
						kickZero +=1;
					}
					else if(fgDistance < 30)
					{
						kickTwenty +=1;
					}
					else if(fgDistance < 40)
					{
						kickThirty += 1;
					}
					else if(fgDistance < 50)
					{
						kickFourty +=1;
					}
					else
					{
						kickFifty +=1;
					}

				}
				sendToDB("kicking");
			}
			//Debug.Log (firstName + " " + lastName + " - " + kicksMade + "/" + kickAttempts);
//			int missIndex = (Array.IndexOf(parsedText , "missed") -1);
//
//			if(missIndex >= 0)
//				kickMiss += 1;

//			if(kickMiss == 1)
//			{
//				fgDistance = int.Parse(parsedText[4]);
//
//			}
//			else
//			{
//				fgDistance = int.Parse (parsedText[3]);
//				if(fgDistance < 20)
//				{
//					kickZero +=1;
//				}
//				else if(fgDistance < 30)
//				{
//					kickTwenty +=1;
//				}
//				else if(fgDistance < 40)
//				{
//					kickThirty += 1;
//				}
//				else if(fgDistance < 50)
//				{
//					kickFourty +=1;
//				}
//				else
//				{
//					kickFifty +=1;
//				}
//			}
		


		}

		for(int x = 0; x < returnLines.Length; x++)
		{
			puntTD = 0;
			kickTD = 0;
			string[] parsedText = returnLines[x].Split (' ' , ',');
			parsedText = parsedText.Where(j => j != "").ToArray();
			firstName = parsedText[1];
			lastName = parsedText[2];
			if(firstName.IndexOf ("'") != -1)
			{
				firstName = firstName.Insert (firstName.IndexOf ("'") , "\'");
				//Debug.Log (firstName);
			}
			if(lastName.IndexOf ("'") != -1)
			{
				lastName = lastName.Insert (lastName.IndexOf ("'") , "\'");
				//Debug.Log (lastName);
			}

			if(parsedText[5] == "punt")
			{
				puntTD += 1;
			}
			else if(parsedText[5] == "kickoff")
			{
				kickTD += 1;
			}
			sendToDB("returns");
		}
		doTeamStats();
		Debug.Log ("TEAM STATS DONE");

	}

	public void sendToDB(string tableName)
	{
		NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=mighty;Database=2020 NFL Stats;");
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
				}
				
			}
			
			finally
			{
				conn.Close();
			}
			
			try
			{
				conn.Open();
				string sqlString2 = "UPDATE defense SET games = " + games + ",  tackles = " + tackles + ",  int = " + intDef + ",  td = " + defTD + ",  safety = " + safety + ",  sack = " + sacks + ",  ff = " + ff + ",  fr = " + fr + ",  block = " + block + " WHERE last = '" + lastName + "' AND first = '" + firstName + "';";
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
				Debug.Log ("Adding kickers");
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

		else if(tableName == "returns")
		{
			try
			{
				conn.Open();
				//console.text += "Connected to db...\n";
				Debug.Log ("Adding returners");
				NpgsqlDataReader dr = command.ExecuteReader();
				
				while(dr.Read())
				{
					kickTD += (int)dr[9];
					puntTD += (int)dr[10];
				}
			}
			
			finally
			{
				conn.Close();
			}
			
			try
			{
				conn.Open();
				string sqlString2 = "UPDATE returns SET kicktd = " + kickTD + ", punttd = " + puntTD + " WHERE last = '" + lastName + "' AND first = '" + firstName + "';";
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
		Debug.Log ("TEAM STATS START");
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

		for(int x = 0; x < passingLines.Length; x++)
		{
			passYards = 0;
			passTD = 0;
			interceptions = 0;
			qbFumbles = 0;
			qbFumblesLost = 0;
			
			string[] parsedText = passingLines[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();

			team = parsedText[2];
			passYards = int.Parse(parsedText[5]);
			passTD = int.Parse(parsedText[7]);
			interceptions = int.Parse (parsedText[9]);
			qbFumbles = int.Parse (parsedText [11]);
			qbFumblesLost = int.Parse (parsedText [12]);
			//Debug.Log ("Home: " + homeTeam + " --- Away: " + awayTeam);
			if(team == homeTeam)
			{
				passAttemptsHOME += int.Parse(parsedText[4]);
				passAttemptsAgainstAWAY += int.Parse(parsedText[4]);
				teamPassYardsHOME += passYards;
				teamPassYardsDefAWAY += passYards;
				teamTdHOME += passTD;
				teamTdDefAWAY += passTD;
				teamIntOffHOME += interceptions;
				teamIntDefAWAY += interceptions;
				totalFumblesHOME += qbFumbles;
				totalFumblesLostHOME += qbFumblesLost;

			}
			else if(team == awayTeam)
			{
				passAttemptsAWAY += int.Parse(parsedText[4]);
				passAttemptsAgainstHOME += int.Parse(parsedText[4]);
				teamPassYardsAWAY += passYards;
				teamPassYardsDefHOME += passYards;
				teamTdAWAY += passTD;
				teamTdDefHOME += passTD;
				teamIntOffAWAY += interceptions;
				teamIntDefHOME += interceptions;
				totalFumblesAWAY += qbFumbles;
				totalFumblesLostAWAY += qbFumblesLost;
			}
			//Debug.Log ("Passer " + firstName + " " + lastName + " added.");
		}
		for(int x = 0; x < rushingLines.Length; x++)
		{
			rushYards = 0;
			rushTD = 0;
			fumblesRush = 0;
			fumblesRushLost = 0;
			
			string[] parsedText = rushingLines[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();
//			Debug.Log (parsedText[5]);
			if(parsedText[5] == "for")
			{
				team = parsedText[3];
				rushYards = int.Parse (parsedText[6]);
				rushTD = int.Parse (parsedText[8]);
				fumblesRush = int.Parse (parsedText[10]);
				fumblesRushLost = int.Parse(parsedText[11]);
				//fumblesRush = int.Parse (parsedText[10]);
				if(team == homeTeam)
				{
					rushAttemptsHOME += int.Parse (parsedText[4]);
					rushAttemptsAgainstAWAY += int.Parse (parsedText[4]);
					teamRushYardsHOME += rushYards;
					teamRushYardsDefAWAY += rushYards;
					teamTdHOME += rushTD;
					teamTdDefAWAY += rushTD;
					totalFumblesHOME += fumblesRush;
					totalFumblesLostHOME += fumblesRushLost;
				}
				else if (team == awayTeam)
				{
					rushAttemptsAWAY += int.Parse (parsedText[4]);
					rushAttemptsAgainstHOME += int.Parse (parsedText[4]);
					teamRushYardsAWAY += rushYards;
					teamRushYardsDefHOME += rushYards;
					teamTdAWAY += rushTD;
					teamTdDefHOME += rushTD;
					totalFumblesAWAY += fumblesRush;
					totalFumblesLostAWAY += fumblesRushLost;
				}

			}
			else
			{
//				firstName = parsedText[0];  // not needed because this is team stats
//				lastName = parsedText[1];
				team = parsedText[2];
				rushYards = int.Parse (parsedText[5]);
				rushTD = int.Parse (parsedText[7]);
				fumblesRush = int.Parse (parsedText[9]);
				fumblesRushLost = int.Parse(parsedText[10]);
				//fumblesRush = int.Parse (parsedText[9]);

				if(team == homeTeam)
				{
					rushAttemptsHOME += int.Parse (parsedText[3]);
					rushAttemptsAgainstAWAY += int.Parse (parsedText[3]);
					teamRushYardsHOME += rushYards;
					teamRushYardsDefAWAY += rushYards;
					teamTdHOME += rushTD;
					teamTdDefAWAY += rushTD;
					totalFumblesHOME += fumblesRush;
					totalFumblesLostHOME += fumblesRushLost;
				}
				else if (team == awayTeam)
				{
					rushAttemptsAWAY += int.Parse (parsedText[3]);
					rushAttemptsAgainstHOME += int.Parse (parsedText[3]);
					teamRushYardsAWAY += rushYards;
					teamRushYardsDefHOME += rushYards;
					teamTdAWAY += rushTD;
					teamTdDefHOME += rushTD;
					totalFumblesAWAY += fumblesRush;
					totalFumblesLostAWAY += fumblesRushLost;
				}
			}
			//Debug.Log ("Rusher " + firstName + " " + lastName + " added.");

		}

		for (int x = 0; x < receivingLines.Length; x++) {
			string[] parsedText = receivingLines[x].Split (' ' , ',' , '/');
			parsedText = parsedText.Where(j => j != "").ToArray();

			drops = 0;
			if (parsedText [5] == "for") {
				team = parsedText[3];
				drops = int.Parse (parsedText [10]);
				fumblesRec = int.Parse (parsedText[12]);
				fumblesRecLost = int.Parse (parsedText [13]);
				if (team == homeTeam) {
					totalDropsHOME += drops;
					totalFumblesHOME += fumblesRec;
					totalFumblesLostHOME += fumblesRecLost;
				} else if (team == awayTeam) {
					totalDropsAWAY += drops;
					totalFumblesAWAY += fumblesRec;
					totalFumblesLostAWAY += fumblesRecLost;
				}
			} else {
				team = parsedText[2];
				drops = int.Parse (parsedText [9]);
				fumblesRec = int.Parse (parsedText[11]);
				fumblesRecLost = int.Parse (parsedText [12]);
				if (team == homeTeam) {
					totalDropsHOME += drops;
					totalFumblesHOME += fumblesRec;
					totalFumblesLostHOME += fumblesRecLost;
				} else if (team == awayTeam) {
					totalDropsAWAY += drops;
					totalFumblesAWAY += fumblesRec;
					totalFumblesLostAWAY += fumblesRecLost;
				}
			}
		}
		for(int x = 0; x < defenseLines.Length; x++)
		{
			string[] parsedText = defenseLines[x].Split (' ' , ',');
			parsedText = parsedText.Where(j => j != "").ToArray();
			team = parsedText[2];
//			Debug.Log("TEAM = " + team);
			int ffIndex = (Array.IndexOf(parsedText, "FF") -1);
			if(ffIndex >= 0)
				ff = int.Parse (parsedText[ffIndex]);
			else
				ff = 0;
//			Debug.Log ("FF = " + ff);
			int frIndex = (Array.IndexOf(parsedText, "FR") -1);
			if(frIndex >= 0)
				fr = int.Parse (parsedText[frIndex]);
			else
				fr = 0;
//			Debug.Log ("FR = " + fr);
			int safetyIndex = (Array.IndexOf(parsedText, "Safety") -1);
			if(safetyIndex >= 0)
				safety = int.Parse (parsedText[safetyIndex]);
			else
				safety = 0;
//			Debug.Log ("Safety = " + safety);
			int defTDIndex = (Array.IndexOf(parsedText, "TD") -1);
			if(defTDIndex >= 0)
				defTD = int.Parse (parsedText[defTDIndex]);
			else
				defTD = 0;
//			Debug.Log ("DefTD = " + defTD);
//			int sacksIndex = (Array.IndexOf(parsedText, "Sacks") -1);
//			if(sacksIndex < 0)
//				sacksIndex = (Array.IndexOf(parsedText, "Sack") -1);
//			if(sacksIndex >= 0)
//				sacks = double.Parse (parsedText[(sacksIndex)]);
//			else
//				sacks = 0;



//			Debug.Log ("Sacks = " + sacks);
			if(team == homeTeam)
			{
				
				teamSafetyHOME += safety;
				teamSafetyOffenseAWAY += safety;
				teamFfHOME += ff;
				teamFrHOME += fr;
				teamDefensiveTdHOME += defTD;
				teamTdDefAWAY += defTD;
			}
			else if(team == awayTeam)
			{
				
				teamSafetyAWAY += safety;
				teamSafetyOffenseHOME += safety;
				teamFfAWAY += ff;
				teamFrAWAY += fr;
				teamDefensiveTdAWAY += defTD;
				teamTdDefHOME += defTD;
			}
			//Debug.Log ("Defender added.");
		}

		teamSacksHOME += double.Parse(results[sacksLine +2]);
		teamSacksOffenseAWAY += double.Parse(results[sacksLine +2]);
		teamSacksAWAY += double.Parse(results[sacksLine +1]);
		teamSacksOffenseHOME += double.Parse(results[sacksLine +1]);

		for(int x = 0; x < returnLines.Length; x++)
		{
			puntTD = 0;
			kickTD = 0;

			string[] parsedText = returnLines[x].Split (' ' , ',' , '(' , ')');
			parsedText = parsedText.Where(j => j != "").ToArray();
			team = convertTeamFromIcon(parsedText[0]);
			//team = parsedText[parsedText.Length -1];
			//Debug.Log (team);

			if(parsedText[5] == "punt")
			{
				puntTD += 1;
			}
			else if(parsedText[5] == "kickoff")
			{
				kickTD += 1;
			}
			if(team == homeTeam)
			{
				teamStTdHOME += (kickTD + puntTD);
				teamTdDefAWAY += (kickTD + puntTD);
			}
			else if (team == awayTeam)
			{
				teamStTdAWAY += (kickTD + puntTD);
				teamTdDefHOME += (kickTD + puntTD);
			}
			//Debug.Log ("Returner " + firstName + " " + lastName + " added.");
		}

		totalYardsHOME += (teamPassYardsHOME + teamRushYardsHOME);
		totalYardsAWAY += (teamPassYardsAWAY + teamRushYardsAWAY);
		totalYardsDefHOME += (teamPassYardsDefHOME + teamRushYardsDefHOME);
		totalYardsDefAWAY += (teamPassYardsDefAWAY + teamRushYardsDefAWAY);
		totalTdsHOME += (teamTdHOME + teamStTdHOME + teamDefensiveTdHOME);
		totalTdsAWAY += (teamTdAWAY + teamStTdAWAY + teamDefensiveTdAWAY);

		string[] scoreLineParse = scoreLine.Split (' ');
		scoreLineParse = scoreLineParse.Where(k => k != "").ToArray();
//		string[] dashes = new string[3];
		int dashPosition = 0;
		//int y = 0;
		for(int x = 0; x < scoreLineParse.Length; x++)
		{
			if(scoreLineParse[x].Contains ("-"))
			{
				dashPosition = x;
			}
		}
//		for(int x = 0; x < scoreLineParse.Length; x++)
//		{
//			if(scoreLineParse[x].Contains("-"))
//			{
//				dashes[y] = scoreLineParse[x];
//				y++;
//			}
//			//Debug.Log (scoreLineParse[x]);
//		}
//		string[] scores = dashes[1].Split ('-');
		string[] scores = scoreLineParse[dashPosition].Split ('-');
		
		homeScore = int.Parse (scores[1]);
		awayScore = int.Parse (scores[0]);
//		Debug.Log ("Score: " + awayScore + "-" + homeScore);
//		Debug.Log (olineLine);
		//string[] olineParse = olineLine.Split (' ', '-');
		//olineParse = olineParse.Where(k => k != "").ToArray();

		olineRatingAWAY = double.Parse (results[olineLine +1]);
		olineRatingHOME = double.Parse (results[olineLine +2]);

//		Debug.Log ("Home: " + olineRatingHOME);
//		Debug.Log ("Away: " + olineRatingAWAY);

//		string[] puntParse = puntingLine.Split (' ', '-');
//		puntParse = puntParse.Where(k => k != "").ToArray();
		
		puntAvgAWAY = double.Parse (results[puntingLine+1]);
		puntAvgHOME = double.Parse (results[puntingLine+2]);

		string[] penaltyParseHome = results[penaltyLine+2].Split (' ', '-');
		string[] penaltyParseAway = results[penaltyLine+1].Split (' ', '-');
//		olineParse = penaltyParse.Where(k => k != "").ToArray();

//		for(int x = 0; x < penaltyParse.Length; x++)
//		{
//			Debug.Log ("PENALTY LINE: " + x + " : " + penaltyParse[x]);
//		}
		penaltyNumberHOME = int.Parse (penaltyParseHome[0]);
		penaltyYardsHOME = int.Parse (penaltyParseHome[1]);
		penaltyNumberAWAY = int.Parse (penaltyParseAway[0]);
		penaltyYardsAWAY = int.Parse (penaltyParseAway[1]);

		string[] thirdDownParseHome = results[thirdDownLine+2].Split (' ', '-');
		string[] thirdDownParseAway = results[thirdDownLine+1].Split (' ', '-');

		thirdconvertedHOME = int.Parse (thirdDownParseHome[0]);
		thirdtotalHOME = int.Parse (thirdDownParseHome[1]);
		thirdconvertedAWAY = int.Parse (thirdDownParseAway[0]);
		thirdtotalAWAY = int.Parse (thirdDownParseAway[1]);

		thirdconverteddefHOME = thirdconvertedAWAY;
		thirdconverteddefAWAY = thirdconvertedHOME;
		thirdtotaldefHOME = thirdtotalAWAY;
		thirdtotaldefAWAY = thirdtotalHOME;

//		Debug.Log ("Penalties - H: " + penaltyNumberHOME + " | Yards: " + penaltyYardsHOME);
//		Debug.Log ("Penalties - A: " + penaltyNumberAWAY + " | Yards: " + penaltyYardsAWAY);

		//		Debug.Log ("Home: " + puntAvgHOME);
//		Debug.Log ("Away: " + puntAvgAWAY);

		NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=mighty;Database=2020 NFL Stats;");
		string sqlString = "SELECT * FROM teams WHERE team = '" + homeTeam + "';";
		
		NpgsqlCommand command = new NpgsqlCommand(sqlString, conn);
		try
		{
			conn.Open();
			//console.text += "Connected to db...\n";
			Debug.Log ("Adding team stats");
			NpgsqlDataReader dr = command.ExecuteReader();
			
			while(dr.Read())
			{
				homeScore += (int)dr[2];
				awayScore += (int)dr[3];
				teamPassYardsHOME += (int)dr[5];
				teamRushYardsHOME += (int)dr[6];
				teamPassYardsDefHOME += (int)dr[14];
				teamRushYardsDefHOME += (int)dr[15];
				totalYardsDefHOME += (int)dr[16];
				teamTdHOME += (int)dr[10];
				teamDefensiveTdHOME += (int)dr[11];
				teamStTdHOME += (int)dr[12];
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
				", thirdconverteddef = " + thirdconverteddefHOME + ", thirdtotaldef = " + thirdtotaldefHOME + " WHERE team = '" + homeTeam + "';";

			NpgsqlCommand WriteCommand = new NpgsqlCommand(sqlString2, conn);
			int rowsaffected;
			rowsaffected = WriteCommand.ExecuteNonQuery();
			//Debug.Log (rowsaffected);
		}
		finally
		{
			conn.Close();
		}

		homeScore = int.Parse (scores[1]);
		awayScore = int.Parse (scores[0]);

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
				teamStTdAWAY += (int)dr[12];
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
				+ ", drops = " + totalDropsAWAY + ", fumbles = " + totalFumblesAWAY + ", fumlost = " + totalFumblesLostAWAY + ", thirdconverted = " + thirdconvertedAWAY + ", thirdtotal = " + thirdtotalAWAY + ", thirdconverteddef = " + thirdconverteddefAWAY + ", thirdtotaldef = " + thirdtotaldefAWAY + " WHERE team = '" + awayTeam + "';";
			
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
		NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=mighty;Database=2020 NFL Stats;");
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
				Debug.Log (dr[5] + " : " + games);
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

	public string convertTeam(string teamName)
	{
//		Debug.Log (teamName);
		int index = Array.FindIndex (teamNamesFull, x => x == teamName);
//		Debug.Log (index);
//		Debug.Log (teamName + ":" + teamAbrevs[index]);
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
		teamAbrevs = new string[]{"ARI" , "ATL" , "BAL" , "BUF" , "CAR" , "CHI" , "CIN" , "CLE" , "DAL" , "DEN" , "DET" , "GB" , "HOU" , "IND" , "JAX" , "KC" , "LAR", "MIA" , "MIN" , "NE" , "NO" , "NYG" , "NYJ" , "OAK" , "PHI" , "PIT" , "LAC" , "SEA" , "SF" , "TB" , "TEN" , "WAS"};
		//  	THERE NEEDS TO BE A SPACE AFTER THE TEAM NAME FOR THIS TO WORK
		teamNamesFull = new string[]{"Arizona Cardinals " , "Atlanta Falcons " , "Baltimore Ravens " , "Buffalo Bills " , "Carolina Panthers " , "Chicago Bears " , "Cincinnati Bengals " , "Cleveland Browns " , "Dallas Cowboys " , "Denver Broncos " , "Detroit Lions " , "Green Bay Packers " ,
			"Houston Texans " , "Indianapolis Colts " , "Jacksonville Jaguars " , "Kansas City Chiefs " , "Los Angeles Rams " , "Miami Dolphins " , "Minnesota Vikings " , "New England Patriots " , "New Orleans Saints " , "New York Giants " , "New York Jets " , "Oakland Raiders " ,
									"Philadelphia Eagles " , "Pittsburgh Steelers " , "Los Angeles Chargers " , "Seattle Seahawks " , "San Francisco 49ers " , "Tampa Bay Buccaneers " , "Tennessee Titans " , "Washington Redskins "} ;

		teamIcons = new string[]{":cardinals:" , ":falcons:" , ":ravens:" , ":bills:" , ":panthers:" , ":bears:" , ":bengals:" , ":browns:" , ":cowboys:" , ":broncos:" , ":fibur:" , ":packers:" , ":texans:" , ":colts:" , ":jaguars:" , ":chiefs:" , ":rams:", ":dolphins:" , ":vikings:" , ":patriots:" , ":saints:" , ":giants:" , ":jets:" , ":raiders:" , ":eagles:" , ":steelers:" , ":chargers:" , ":seahawks:" , ":49ers:" , ":buccaneers:" , ":titans:" , ":redskins:"};

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
