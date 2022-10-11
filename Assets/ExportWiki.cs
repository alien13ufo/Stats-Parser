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

public class ExportWiki : MonoBehaviour {

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
	private int qbFumbles, qbFumblesLost;
	private int fumblesRec, fumblesRecLost;
	private int fumblesRush, fumblesRushLost;
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
//	private int kickTD;
//	private int puntTD;
	private double kickPercent;
	private int kickFifty = 0;
	private int kickFourty = 0;
	private int kickThirty = 0;
	private int kickTwenty = 0;
	private int kickZero = 0;
	private int fgDistance = 0;
	private int drops;
	private int pd;
	private int TFL;
	private int dropsTeam, fumblesTeam, fumblesLostTeam;
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
	private string height;
	private int weight;
	private string year;
	private int skill;
	private string position;
	private int pointsfor, pointsagainst, pointdif, passYardsTeam, rushYardsTeam, totalYardsTeam, passYardsDefTeam, rushYardsDefTeam, totalYardsDefTeam, offTDTeam, defTDTeam, stTD, totalTD, safetyOff, safetyDef, intOffTeam, intDefTeam, ffTeam, frTeam, tdGivenUp;
	private double ypg, ypga, passypg, passypga, rushypg, rushypga, sacksOff, sacksDef, ppg, ppga, oypp, dypp, olinerating, puntavg;
	private int tp, tpa, ra, pa, paa, raa;
	private double teamypc, teamypca, teamypa, teamypaa;
	private int penaltyNum, penaltyYards;
	private double thirdrate, thirdratedef;
	private int krYards, prYards, krTD, prTD, krYardsAgainst, prYardsAgainst, krTDAgainst, prTDAgainst;

	public void ExportDB()
	{
		string exportFolder = Application.dataPath + "/WikiExports/";

		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "passing.txt", true))
		{
			file.WriteLine ("=Passing Stats=");
			file.WriteLine ("{| class=\"wikitable sortable\" style=\"text-align: center");
			file.WriteLine ("! Position !! Forename !! Surname !! Height !! Weight !! Year !! Skill !! Team !! Games !! Completions !! Attempts !! Completion PCT !! Yards !! TDs !! INTs !! QB Rating !! YPA !! YPG !! TD PCT !! INT PCT !! F !! FL");
		}

		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "rushing.txt", true))
		{
			file.WriteLine ("=Rushing Stats=");
			file.WriteLine ("{| class=\"wikitable sortable\" style=\"text-align: center");
			file.WriteLine ("! Position !! Forename !! Surname !! Height !! Weight !! Year !! Skill !! Team !! Games !! Attempts !! Yards !! TDs !! F !! FL !! 100yd Games !! YPC !! YPG");
		}

		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "receiving.txt", true))
		{
			file.WriteLine ("=Receiving Stats=");
			file.WriteLine ("{| class=\"wikitable sortable\" style=\"text-align: center");
			file.WriteLine ("! Position !! Forename !! Surname !! Height !! Weight !! Year !! Skill !! Team !! Games !! Catches !! Yards !! TDs !! F !! FL !! Drops !! YPC !! YPG");
		}

		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "defense.txt", true))
		{
			file.WriteLine ("=Defensive Stats=");
			file.WriteLine ("{| class=\"wikitable sortable\" style=\"text-align: center");
			file.WriteLine ("! Position !! Forename !! Surname !! Height !! Weight !! Year !! Potential !! Team !! Games !! Tackles !! INTs !! PD !! Sacks !! TFL !! FF !! FR !! TDs !! Safeties");
		}
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "kicking.txt", true))
		{
			file.WriteLine ("=Kicking Stats=");
			file.WriteLine ("{| class=\"wikitable sortable\" style=\"text-align: center");
			file.WriteLine ("! Position !! Forename !! Surname !! Height !! Weight !! Year !! Skill !! Team !! Attempts !! Missed !! FG PCT !! +0 !! +20 !! +30 !! +40 !! +50");
		}
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (exportFolder + "teamST.txt", true)) {
			file.WriteLine ("=Special Teams Stats=");
			file.WriteLine ("{| class=\"wikitable sortable\" style=\"text-align: center");
			file.WriteLine ("! Team !! Games !! KR Yards !! PR Yards || KR TD || PR TD || KR YA || PR YA || KR TDA || PR TDA || Punt AVG");
		}
//		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "returns.txt", true))
//		{
//			file.WriteLine ("=Return Stats=");
//			file.WriteLine ("{| class=\"wikitable sortable\" style=\"text-align: center");
//			file.WriteLine ("! Position !! Forename !! Surname !! Height !! Weight !! Year !! Skill !! Team !! Kick-off TDs !! Punt TDs");
//		}
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "teamOffense.txt", true))
		{
			file.WriteLine ("=Team Stats Offense=");
			file.WriteLine ("{| class=\"wikitable sortable\" style=\"text-align: center");
			file.WriteLine ("! Team !! Games !! Points For !! Points Against !! Point Diff !! PPG !! Pass Yards !! Rush Yards !! Total Yards !! Pass YPG !! Rush YPG !! YPG !! TDs !! TDs Total !! INT !! Drops !! F !! FL !! Sacks !! Safeties !! OYPP !! RA !! PA !! YPC !! YPA !! 3D% !! Plays !! Line AVG");
		}
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "teamDefense.txt", true))
		{
			file.WriteLine ("=Team Stats Defense=");
			file.WriteLine ("{| class=\"wikitable sortable\" style=\"text-align: center");
			file.WriteLine ("! Team !! Games !! PPGA !! PYA !! RYA !! TYA !! Pass YPGA !! Rush YPGA !! YPGA !! Def TDs !! ST TDs !! INT !! FF !! FR !! Sacks !! Safeties !! TDs Allowed !! DYPP || 3D% !! RAA !! PAA !! YPCA !! YPAA !! Plays Against !! Pen !! Pen YDS");
		}

		NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=mighty;Database=2024 NFL Stats;");

		string sqlString = "SELECT * FROM passing WHERE games != 0;";
		
		NpgsqlCommand command = new NpgsqlCommand(sqlString, conn);

		try
		{
			conn.Open();
			NpgsqlDataReader dr = command.ExecuteReader();
				
			while(dr.Read())
			{
				position = (string)dr[1];
				firstName = (string)dr[2];
				lastName = (string)dr[3];
				height = (string)dr[4];
				weight = (int)dr[5];
				year = (string)dr[6];
				skill = (int)dr[7];
				team  = (string)dr[8];
				games = (int)dr[9];
				completions = (int) dr[10];
				passAttempts = (int)dr[11];
				completionPercent = (double)dr[12];	
				passYards = (int)dr[13];
				passTD = (int)dr[14];
				interceptions = (int)dr[15];
				rating = (double)dr[16];
				YPA = (double)dr[17];
				YPGpass = (double)dr[18];
				tdPercent = (double)dr[19];
				intPercent = (double)dr[20];
				qbFumbles = (int)dr[21];
				qbFumblesLost = (int)dr[22];

				using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "passing.txt", true))
				{
					file.WriteLine("|-style=\"text-align: center\"");
					file.WriteLine("|" + position + "||" + firstName + "||" + lastName + "||" + height + "||" + weight + "||" + year + "||" + skill + "||" + team + "||" + games + "||" + completions + "||" + passAttempts + "||" + completionPercent + "||" + passYards + "||" + passTD + "||" + interceptions + "||" + rating + "||" + YPA + "||" + YPGpass + "||" + tdPercent + "||" + intPercent + "||" + qbFumbles + "||" + qbFumblesLost);
				}
			}
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "passing.txt", true))
			{
				file.WriteLine("|}");
			}
			Debug.Log ("Passing done!");
	
		}
			
		
		finally
		{
			conn.Close();
		}

		sqlString = "SELECT * FROM rushing WHERE games != 0;";
		command = new NpgsqlCommand(sqlString, conn);

		try
		{
			conn.Open();
			NpgsqlDataReader dr = command.ExecuteReader();
			
			while(dr.Read())
			{
				position = (string)dr[1];
				firstName = (string)dr[2];
				lastName = (string)dr[3];
				height = (string)dr[4];
				weight = (int)dr[5];
				year = (string)dr[6];
				skill = (int)dr[7];
				team  = (string)dr[8];
				games = (int)dr[9];
				rushAttempts = (int) dr[10];
				rushYards = (int) dr[11];
				rushTD = (int) dr[12];	

				fumblesRush = (int)dr[13];
				hundredYardGames = (int)dr[14];
				YPCarry = (double)dr[15];
				YPGrun = (double)dr[16];
				fumblesRushLost = (int)dr[17];
								
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "rushing.txt", true))
				{
					file.WriteLine("|-style=\"text-align: center\"");
					file.WriteLine("|" + position + "||" + firstName + "||" + lastName + "||" + height + "||" + weight + "||" + year + "||" + skill + "||" + team + "||" + games + "||" + rushAttempts + "||" + rushYards + "||" + rushTD + "||" + fumblesRush + "||" + fumblesRushLost + "||" + hundredYardGames + "||" + YPCarry + "||" + YPGrun);
				}
			}
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "rushing.txt", true))
			{
				file.WriteLine("|}");
			}
			Debug.Log ("Rushing done!");
			
		}

		finally
		{
			conn.Close();
		}

		sqlString = "SELECT * FROM receiving WHERE games != 0;";
		command = new NpgsqlCommand(sqlString, conn);
		
		try
		{
			conn.Open();
			NpgsqlDataReader dr = command.ExecuteReader();
			
			while(dr.Read())
			{
				position = (string)dr[1];
				firstName = (string)dr[2];
				lastName = (string)dr[3];
				height = (string)dr[4];
				weight = (int)dr[5];
				year = (string)dr[6];
				skill = (int)dr[7];
				team  = (string)dr[8];
				games = (int)dr[9];
				catches = (int) dr[10];
				receivingYards = (int) dr[11];
				receivingTD = (int) dr[12];	
				fumblesRec = (int)dr[13];
				YPCatch = (double)dr[14];
				YPGreceiving = (double)dr[15];
				//fucked up the table in 2024, these were reversed before that
				drops = (int)dr[17];
				fumblesRecLost = (int)dr[16];
				
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "receiving.txt", true))
				{
					file.WriteLine("|-style=\"text-align: center\"");
					file.WriteLine("|" + position + "||" + firstName + "||" + lastName + "||" + height + "||" + weight + "||" + year + "||" + skill + "||" + team + "||" + games + "||" + catches + "||" + receivingYards + "||" + receivingTD + "||" + fumblesRec + "||" + fumblesRecLost + "||" + drops + "||" + YPCatch + "||" + YPGreceiving);
				}
			}
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "receiving.txt", true))
			{
				file.WriteLine("|}");
			}
			Debug.Log ("Receiving done!");
			
		}
		
		finally
		{
			conn.Close();
		}

		sqlString = "SELECT * FROM defense WHERE games != 0;";
		command = new NpgsqlCommand(sqlString, conn);
		
		try
		{
			conn.Open();
			NpgsqlDataReader dr = command.ExecuteReader();
			
			while(dr.Read())
			{
				position = (string)dr[1];
				firstName = (string)dr[2];
				lastName = (string)dr[3];
				height = (string)dr[4];
				weight = (int)dr[5];
				year = (string)dr[6];
				skill = (int)dr[7];
				team  = (string)dr[8];
				games = (int)dr[9];
				tackles = (int) dr[10];
				intDef = (int) dr[11];
				sacks = (double) dr[12];	
				ff = (int)dr[13];
				fr = (int)dr[14];
				block = (int)dr[15];
				defTD = (int)dr[16];
				safety = (int)dr[17];
				pd = (int)dr[18];
				TFL = (int)dr[19];
				
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "defense.txt", true))
				{
					file.WriteLine("|-style=\"text-align: center\"");
					file.WriteLine("|" + position + "||" + firstName + "||" + lastName + "||" + height + "||" + weight + "||" + year + "||" + skill + "||" + team + "||" + games + "||" + tackles + "||" + intDef + "||" + pd + "||" + sacks + "||" + TFL + "||" + ff + "||" + fr + "||" + defTD + "||" + safety);
				}
			}
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "defense.txt", true))
			{
				file.WriteLine("|}");
			}
			Debug.Log ("Defense done!");
			
		}
		
		finally
		{
			conn.Close();
		}
		sqlString = "SELECT * FROM kicking WHERE attempts != 0;";
		command = new NpgsqlCommand(sqlString, conn);
		
		try
		{
			conn.Open();
			NpgsqlDataReader dr = command.ExecuteReader();
			
			while(dr.Read())
			{
				position = (string)dr[1];
				firstName = (string)dr[2];
				lastName = (string)dr[3];
				height = (string)dr[4];
				weight = (int)dr[5];
				year = (string)dr[6];
				skill = (int)dr[7];
				team  = (string)dr[8];
				kickAttempts = (int)dr[9];
				kickMiss = (int) dr[10];
				kickPercent = (double) dr[11];
				kickFifty = (int) dr[12];	
				kickFourty = (int)dr[13];
				kickThirty = (int)dr[14];
				kickTwenty = (int)dr[15];
				kickZero = (int)dr[16];
				
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "kicking.txt", true))
				{
					file.WriteLine("|-style=\"text-align: center\"");
					file.WriteLine("|" + position + "||" + firstName + "||" + lastName + "||" + height + "||" + weight + "||" + year + "||" + skill + "||" + team + "||" + kickAttempts + "||" + kickMiss + "||" + kickPercent + "||" + kickZero + "||" + kickTwenty + "||" + kickThirty + "||" + kickFourty + "||" + kickFifty );
				}
			}
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "kicking.txt", true))
			{
				file.WriteLine("|}");
			}
			Debug.Log ("Kicking done!");
			
		}
		
		finally
		{
			conn.Close();
		}

//		sqlString = "SELECT * FROM returns WHERE kicktd != 0 OR punttd != 0;";
//		command = new NpgsqlCommand(sqlString, conn);
//		
//		try
//		{
//			conn.Open();
//			NpgsqlDataReader dr = command.ExecuteReader();
//			
//			while(dr.Read())
//			{
//				position = (string)dr[1];
//				firstName = (string)dr[2];
//				lastName = (string)dr[3];
//				height = (string)dr[4];
//				weight = (int)dr[5];
//				year = (string)dr[6];
//				skill = (int)dr[7];
//				team  = (string)dr[8];
//				kickTD = (int)dr[9];
//				puntTD = (int) dr[10];
//				
//				using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "returns.txt", true))
//				{
//					file.WriteLine("|-style=\"text-align: center\"");
//					file.WriteLine("|" + position + "||" + firstName + "||" + lastName + "||" + height + "||" + weight + "||" + year + "||" + skill + "||" + team + "||" + kickTD + "||" + puntTD );
//				}
//			}
//			using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "returns.txt", true))
//			{
//				file.WriteLine("|}");
//			}
//			Debug.Log ("Returns done!");
			
//		}
//		
//		finally
//		{
//			conn.Close();
//		}

		for(int x = 1; x < 33; x++)
		{
			sqlString = "SELECT * FROM teams WHERE id = " + x +";";
			command = new NpgsqlCommand(sqlString, conn);
			
			try
			{
				conn.Open();
				NpgsqlDataReader dr = command.ExecuteReader();
				
				while(dr.Read())
				{
					team = (string)dr[1];
					pointsfor = (int)dr[2];
					pointsagainst = (int)dr[3];
					pointdif = (int)dr[4];
					passYardsTeam = (int)dr[5];
					rushYardsTeam = (int)dr[6];
					passypg = (double)dr[7];
					rushypg = (double)dr[8];
					ypg = (double)dr[9];
					offTDTeam = (int)dr[10];
					defTDTeam = (int)dr[11];
					stTD = (int)dr[12];
					totalTD = (int)dr[13];
					passYardsDefTeam = (int)dr[14];
					rushYardsDefTeam = (int)dr[15];
					totalYardsDefTeam = (int)dr[16];
					passypga = (double)dr[17];
					rushypga = (double)dr[18];
					ypga = (double)dr[19];
					sacksOff = (double)dr[20];
					sacksDef = (double)dr[21];
					safetyOff = (int)dr[22];
					safetyDef = (int)dr[23];
					intOffTeam = (int)dr[24];
					intDefTeam = (int)dr[25];
					ffTeam = (int)dr[26];
					frTeam = (int)dr[27];
					totalYardsTeam = (int)dr[31];
					games = (int)dr[32];
					tdGivenUp = (int)dr[33];
					ppg = (double)dr[34];
					ppga = (double)dr[35];
					oypp = (double)dr[36];
					dypp = (double)dr[37];
					olinerating = (double)dr[38];
					puntavg = (double)dr[39];
					ra = (int)dr[40];
					pa = (int)dr[41];
					raa = (int)dr[42];
					paa = (int)dr[43];
					teamypc = (double)dr[44];
					teamypa = (double)dr[45];
					teamypca = (double)dr[46];
					teamypaa = (double)dr[47];
					tp = (int)dr[48];
					tpa = (int)dr[49];
					penaltyNum = (int)dr[50];
					penaltyYards = (int)dr[51];
					dropsTeam = (int)dr[52];
					fumblesTeam = (int)dr[53];
					fumblesLostTeam = (int)dr[54];
					thirdrate = (double)dr[57];
					thirdratedef = (double)dr[60];
					prYards = (int)dr[61];
					prYardsAgainst = (int)dr[62];
					krYards = (int)dr[63];
					krYardsAgainst = (int)dr[64];
					prTD = (int)dr[65];
					prTDAgainst = (int)dr[66];
					krTD = (int)dr[67];
					krTDAgainst = (int)dr[68];


					//file.WriteLine ("! Team !! Games !! Points For !! Points Against !! Point Diff !! PPG !! Pass Yards !! Rush Yards !! Total Yards !! Pass YPG !! Rush YPG !! Total YPG !! TDs !! TDs Total !! INT !! Sacks !! Safeties ");
					//file.WriteLine ("! Team !! Games !! PPGA !! Pass Yards Againts !! Rush Yards Against !! Total Yards Against !! Pass YPGA !! Rush YPGA !! Total YPGA !! Def TDs !! ST TDs !! INT !! FF !! FR !! Sacks !! Safeties !! TDs Allowed");

					using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "teamOffense.txt", true))
					{
						file.WriteLine("|-style=\"text-align: center\"");
						file.WriteLine("|" + team + "||" + games + "||" + pointsfor + "||" + pointsagainst + "||" + pointdif + "||" + ppg + "||" + passYardsTeam + "||" + rushYardsTeam + "||" + totalYardsTeam + "||" + passypg +
						               "||" + rushypg + "||" + ypg + "||" + offTDTeam + "||" + totalTD + "||" + intOffTeam + "||" + dropsTeam + "||" + fumblesTeam + "||" + fumblesLostTeam + "||" + sacksOff + "||" + safetyOff + "||" + oypp + "||" + ra + "||" + pa + "||" + teamypc + "||" + 
						               teamypa + "||" + thirdrate + "||" + tp + "||" + olinerating);
					}

					using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "teamDefense.txt", true))
					{
						file.WriteLine("|-style=\"text-align: center\"");
						file.WriteLine("|" + team + "||" + games + "||" + ppga + "||" + passYardsDefTeam + "||" + rushYardsDefTeam + "||" + totalYardsDefTeam + "||" + passypga + "||" + rushypga + "||" + ypga + "||" + defTDTeam +
						               "||" + stTD + "||" + intDefTeam + "||" + ffTeam + "||" + frTeam + "||" + sacksDef + "||" + safetyDef + "||" + tdGivenUp + "||" + dypp + "||" + thirdratedef + "||" + raa + "||" + paa + "||" + teamypca + "||" + 
						               teamypaa + "||" + tpa + "||" + penaltyNum + "||" + penaltyYards);
					}

					//file.WriteLine ("! Team !! Games !! KR Yards !! PR Yards || KR TD || PR TD || KR YA || PR YA || KR TDA || PR TDA || Punt AVG");
					using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "teamST.txt", true))
						{
							file.WriteLine("|-style=\"text-align: center\"");
							file.WriteLine("|" + team + "||" + games + "||" + krYards + "||" + prYards + "||" + krTD + "||" + prTD + "||" + krYardsAgainst + "||" + prYardsAgainst + "||" + krTDAgainst + "||" + prTDAgainst + "||" + puntavg);
						}
				}

				Debug.Log ("Teams done!");
				
			}
			
			finally
			{
				conn.Close();
			}
		}
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "teamOffense.txt", true))
		{
			file.WriteLine("|}");
		}
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFolder + "teamDefense.txt", true))
		{
			file.WriteLine("|}");
		}
	}
}
