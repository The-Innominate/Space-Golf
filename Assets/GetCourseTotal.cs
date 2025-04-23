using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GetCourseTotal : MonoBehaviour
{
	private void Awake()
	{
		TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

		Dictionary<string, int> data = LevelHighScores.Instance.LoadAllScores();

		int totalScore = 0;
		int levelCount = 0;

		foreach (KeyValuePair<string, int> kvp in data)
		{
			totalScore += kvp.Value;
			levelCount++;
		}

		if (levelCount < 18)
		{
			text.text = "Course Total: " + totalScore + " (Not Yet Done)";
		}
		else
		{
			text.text = "Course Total: " + totalScore.ToString();
		}
	}
}
