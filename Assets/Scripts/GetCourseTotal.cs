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

		Dictionary<string, Dictionary<string, int>> data = LevelHighScores.Instance.LoadAllScores();

		data.TryGetValue("Begin", out Dictionary<string, int> courseData);

		int totalScore = 0;
		int levelCount = 0;

		if (courseData == null)
		{
			text.text = "Course Total: 0 (Not Yet Done)";
			return;
		}

		foreach (KeyValuePair<string, int> kvp in courseData)
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
