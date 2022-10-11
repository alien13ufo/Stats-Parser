using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;
using Npgsql;
using System.Linq;

public class ParsePlayerLine : MonoBehaviour {

	public Text playerLine;
	public Text playerTeam;
	public Toggle passing;
	public Toggle rushing;
	public Toggle receiving;
	public Toggle defense;
	public Toggle kicking;
	public Toggle returns;
	public Text console;
	public Text fileNameInput;

	private string fullPlayerLine;
	private string position;
	private string firstName;
	private string lastName;
	private string height;
	private int weight;
	private string year;
	private int skill;
	private string team;
	private string fileName;
	private string fileDirectory;
	private string[] results;
	private string[] offenseLines;
	private string[] defenseLines;
	private string[] stLines;

	// Use this for initialization
	void Start () {
	
	}

	public void parsePlayer()
	{
		fullPlayerLine = playerLine.text;
		console.text += fullPlayerLine + " being parsed...\n";
		//format: QB Jason Johnson 5-11 190 R UCLA [Pocket] 83
		string[] parsedText = fullPlayerLine.Split (' ');
		Debug.Log (parsedText[1] + " " + parsedText[2]);
		position = parsedText[0];
		if(parsedText[3].IndexOf ("-") != -1)
		{
			firstName = parsedText[1];
			lastName = parsedText[2];
			height = parsedText[3];
			weight = int.Parse(parsedText[4]);
			year = parsedText[5];
			skill = int.Parse(parsedText[parsedText.Length -1]);
		}
		else
		{
			lastName = parsedText[3];
			height = parsedText[4];
			weight = int.Parse(parsedText[5]);
			year = parsedText[6];
			skill = int.Parse(parsedText[parsedText.Length -1]);
		}
		team = playerTeam.text;
//		console.text += "Pos: " + position + "\n";
//		console.text += "First: " + firstName + "\n";
//		console.text += "Last: " + lastName + "\n";
//		console.text += "Height: " + height + "\n";
//		console.text += "Weight: " + weight + "\n";
//		console.text += "Year: " + year + "\n";
//		console.text += "Skill: " + skill + "\n";
//		console.text += "Team: " + team + "\n";
		bool send = true;
		if(send)
		{
			sendToDB("passing");
		}
	}

	public void sendToDB(string tableName)
	{
		NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=postgres;Password=mighty;Database=2024 NFL Stats;");
		conn.Open();
		//console.text += "Connected to db...\n";

		string sqlString = "INSERT INTO " + tableName + "(Pos, First, Last, Height, Weight, Year, Skill, Team) VALUES('" + position +"', '" + firstName + "', '" + lastName + "', '" + height + "', " + weight + ", '" + year + "', " + skill + ", '" + team + "');";
		Debug.Log (sqlString);
		NpgsqlCommand command = new NpgsqlCommand(sqlString, conn);

		//console.text += "Data submitted\n";
		int rowsaffected;
		try
		{
			rowsaffected = command.ExecuteNonQuery();
			//Console.WriteLine("It was added {0} lines in table table1", rowsaffected);
		}
		
		finally
		{
			conn.Close();
		}
	}

	public void parseTeam()
	{
		fileName = fileNameInput.text;
		fileDirectory = Application.dataPath + "/2024 teams/" + fileName + ".txt";
		//StreamReader reader=new  StreamReader(fileDirectory);
		Debug.Log(fileDirectory);
		results = System.IO.File.ReadAllLines(fileDirectory);
		results = results.Where(x => x != "").ToArray(); // remove empty lines

		for(int x=0; x < results.Length; x++)
		{
			
			if(results[x].Contains("?"))
			{
				results[x] = results [x].Replace ("?", string.Empty);
				Debug.Log (results [x]);
			}
			char end = results[x].Last();
			//Debug.Log ("End of this line: " + end);
			if (end == ' ') {
				results [x] = results[x].Remove (results[x].Length - 1);
			}
			//Debug.Log (results [x]);
			string[] parsedText = results[x].Split (' ');
			int lineLength = parsedText.Length;
			position = parsedText[0];
			//Debug.Log (position + " " + parsedText[1] + " " + parsedText[2] + " " + parsedText[3] + " " + parsedText[4] + " " + parsedText[5] + " " + parsedText[8]);
			//Debug.Log(lineLength);
			if(parsedText[3].IndexOf ("-") != -1)
			{
				firstName = parsedText[1];
				lastName = parsedText[2];
				height = parsedText[3];
				weight = int.Parse(parsedText[4]);
				year = parsedText[5];
				skill = int.Parse(parsedText[lineLength -1]);
			}
			else
			{
				firstName = parsedText[1] + " " + parsedText[2];
				lastName = parsedText[3];
				height = parsedText[4];
				weight = int.Parse(parsedText[5]);
				year = parsedText[6];
				skill = int.Parse(parsedText[lineLength -1]);
			}
			team = fileName;

			if(firstName.IndexOf ("'") != -1)
			{
				firstName = firstName.Insert (firstName.IndexOf ("'"), "\'");
				//Debug.Log (firstName);
			}
			if(lastName.IndexOf ("'") != -1)
			{
				lastName = lastName.Insert (lastName.IndexOf ("'"), "\'");
				//Debug.Log (lastName);
			}
//			console.text += "Pos: " + position + "\n";
//			console.text += "First: " + firstName + "\n";
//			console.text += "Last: " + lastName + "\n";
//			console.text += "Height: " + height + "\n";
//			console.text += "Weight: " + weight + "\n";
//			console.text += "Year: " + year + "\n";
//			console.text += "Skill: " + skill + "\n";
//			console.text += "Team: " + team + "\n";

			if(position == "K" || position == "P")
			{
				sendToDB("kicking");
			}
			else if(position == "QB" || position == "RB" || position == "WR" || position == "TE" || position == "FB" || position == "KR" || position == "PR" )
			{
				sendToDB("returns");
				sendToDB("passing");
				sendToDB("rushing");
				sendToDB("receiving");
			}
			else if(position == "DE" || position == "DT" || position == "SS" || position == "FS" || position == "ILB" || position == "OLB" || position == "CB")
			{
				sendToDB("defense");
				if(position != "DE" || position != "DT")
					sendToDB("returns");
			}
		}
		

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
