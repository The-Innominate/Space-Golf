using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Dan.Main;
using UnityEngine.Android;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names;
	[SerializeField] private List<TextMeshProUGUI> scores;

	[SerializeField] private TMP_InputField usernameInputField;

	private string publicKey = "eca8d9679804a56f824e768a1d7ca7f23e62324fa6703737c67018057299a5b1";

	private void Start()
	{
		getLeaderboard();
	}

	public void getLeaderboard()
	{
		LeaderboardCreator.GetLeaderboard(publicKey, (result) =>
		{
			if (result != null)
			{
				int loopLength = Mathf.Min(names.Count, result.Length);
				for (int i = 0; i < loopLength; i++)
				{
					names[i].text = result[i].Username;
					scores[i].text = result[i].Score.ToString();
				}
			}
			else
			{
				Debug.Log("No leaderboard data found.");
			}
		});
	}

	public void SetLeaderboardEntry(string unsername, int score)
	{
		LeaderboardCreator.UploadNewEntry(publicKey, unsername, score, (result) =>
		{
			if (result != null)
			{
				Debug.Log("Leaderboard entry set successfully.");
			}
			else
			{
				Debug.Log("Failed to set leaderboard entry.");
			}

			getLeaderboard();
		});
	}

	public void OnLeaderboardEnter()
	{
		Dictionary<string, Dictionary<string, int>> data = LevelHighScores.Instance.LoadAllScores();

		if (data == null)
		{
			Debug.Log("No data found.");
			return;
		}

		data.TryGetValue("Begin", out Dictionary<string, int> courseData);

		int totalScore = 0;
		int levelCount = 0;

		foreach (KeyValuePair<string, int> kvp in courseData)
		{
			totalScore += kvp.Value;
			levelCount++;
		}

		//TODO: check if the username is empty and check if all 18 levels are completed

		SetLeaderboardEntry(usernameInputField.text, totalScore);
	}
}
