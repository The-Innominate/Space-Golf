using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json; // Ensure you have the Newtonsoft.Json package installed

public sealed class LevelHighScores
{
    private static LevelHighScores instance;

	public static LevelHighScores Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new LevelHighScores();
			}
			return instance;
		}
	}

	private LevelHighScores()
	{
		// Constructor logic here

		if(!File.Exists(filePath))
		{
			File.WriteAllText(filePath, "{}"); // Empty JSON object
			Debug.Log("File created at: " + filePath);
		}
	}

	private string filePath = Application.persistentDataPath + "/levelHighScores.json";

	public void SaveToJson(string levelName, int score)
	{
		var data = LoadAllScores();

		int existingScore;
		existingScore = data.TryGetValue(levelName, out existingScore) ? existingScore : 0;
		if (existingScore == 0)
		{
			data.Add(levelName, score);
			string json = JsonConvert.SerializeObject(data, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}
		else if (existingScore > score)
		{
			data[levelName] = score;
			string json = JsonConvert.SerializeObject(data, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}
		else
		{
			Debug.Log("Score not saved, existing score is lower.");
		}
	}

	public Dictionary<string, int> LoadAllScores()
	{
		if (!File.Exists(filePath))
			return new Dictionary<string, int>();

		string json = File.ReadAllText(filePath);
		return JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
	}

	public int LoadFromJson(string levelName)
	{
		var data = LoadAllScores();
		return data.TryGetValue(levelName, out int score) ? score : 0;
	}

	//public void SaveToJson(string levelName, int Score)
	//{
	//	var theData = LoadFromJson();
	//	string data;
	//	int previousScore = LoadFromJson(levelName); // Load existing data to avoid overwriting
	//	if (previousScore == 0)
	//	{
	//		theData.Add(levelName, Score.ToString()); // Add new level and score
	//		data = JsonUtility.ToJson(theData, true);
	//		System.IO.File.WriteAllText(filePath, data); // Save the updated data
	//	}
	//	else if(previousScore > Score)
	//	{
	//		theData[levelName] = Score.ToString();
	//		data = JsonUtility.ToJson(theData, true);
	//		System.IO.File.WriteAllText(filePath, data); // Save the updated data
	//	}
	//}

	//private Dictionary<string, string> LoadFromJson()
	//{
	//	// Load the JSON data from the file
	//	string json = System.IO.File.ReadAllText(filePath);
	//	// Deserialize the JSON data into a dictionary
	//	var levelHighScores = JsonUtility.FromJson<Dictionary<string, string>>(json);
	//	return levelHighScores;
	//}

	//public int LoadFromJson(string levelName)
	//{
	//	// Load the JSON data from the file
	//	string json = System.IO.File.ReadAllText(filePath);
	//	// Deserialize the JSON data into a dictionary
	//	var levelHighScores = JsonUtility.FromJson<Dictionary<string, string>>(json);

	//	string highScore = "0"; // Default value if the level name does not exist
	//							// Check if the level name exists in the dictionary
	//	if (levelHighScores.ContainsKey(levelName))
	//	{
	//		// Get the high score for the level
	//		highScore = levelHighScores[levelName];
	//		Debug.Log("High Score for " + levelName + ": " + highScore);
	//	}
	//	else
	//	{
	//		Debug.Log("No high score found for " + levelName);
	//		return 0; // Return 0 if the level name does not exist
	//	}

	//	// Return the high score as an integer

	//	try
	//	{
	//		return int.Parse(highScore);
	//	}
	//	catch (System.Exception e)
	//	{
	//		Debug.LogError("Error parsing high score: " + e.Message);
	//	}

	//	// If the level name does not exist, return 0 or some default value
	//	return 0;
	//}
}
