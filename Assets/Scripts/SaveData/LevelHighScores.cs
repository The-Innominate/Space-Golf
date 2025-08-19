using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

public sealed class LevelHighScores
{
	private static LevelHighScores instance;
	private string filePath = Application.persistentDataPath + "/levelHighScores.json";

	// === Key Obfuscation (Split + Base64 + Scrambled + Decoy Junk) ===
	private static string GetEncryptionKey()
	{
		// Base64 encoded parts (some are fake junk!)
		string[] scrambledParts = new string[]
		{
			"QmFkUGFydDE=",        // "BadPart1" (junk)
            "UzNjcjN0",            // "S3cr3t" (real, 2nd piece)
            "U3VwM3I=",            // "Sup3r" (real, 1st piece)
            "SmFuazEyMw==",        // "Jank123" (junk)
            "SzN5MTIzNDU2Nzg5MCE=", // "K3y1234567890!" (real, 3rd piece)
            "RmFrZUtleQ=="         // "FakeKey" (junk)
        };

		// The correct order to reassemble only the REAL ones
		int[] indexMap = { 2, 1, 4 };

		StringBuilder sb = new StringBuilder();
		foreach (int i in indexMap)
		{
			string decoded = Encoding.UTF8.GetString(System.Convert.FromBase64String(scrambledParts[i]));
			sb.Append(decoded);
		}

		string fullKey = sb.ToString(); // "Sup3rS3cr3tK3y1234567890!"
		return fullKey;
	}

	// 16 bytes IV (AES block size)
	private static readonly byte[] iv = new byte[16];

	public static LevelHighScores Instance
	{
		get
		{
			if (instance == null)
				instance = new LevelHighScores();
			return instance;
		}
	}

	private LevelHighScores()
	{
		if (!File.Exists(filePath))
		{
			SaveEncrypted("{}"); // write empty encrypted JSON
			Debug.Log("File created at: " + filePath);
		}
	}

	public void SaveToJson(string course, string levelName, int score)
	{
		var data = LoadAllScores();
		if (data == null)
			data = new Dictionary<string, Dictionary<string, int>>();

		if (!data.ContainsKey(course))
			data[course] = new Dictionary<string, int>();

		if (!data[course].ContainsKey(levelName))
			data[course][levelName] = 0;

		int existingScore = data[course][levelName];
		if (existingScore > score || existingScore == 0)
		{
			data[course][levelName] = score;
			string json = JsonConvert.SerializeObject(data, Formatting.Indented);
			SaveEncrypted(json);
			Debug.Log("Score saved.");
		}
		else
		{
			Debug.Log("Score not saved, existing score is lower.");
		}
	}

	public Dictionary<string, Dictionary<string, int>> LoadAllScores()
	{
		if (!File.Exists(filePath))
			return new Dictionary<string, Dictionary<string, int>>();

		string json = LoadDecrypted();
		var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(json);
		if (data == null) data = new Dictionary<string, Dictionary<string, int>>();
		return data;
	}

	public int LoadFromJson(string course, string levelName)
	{
		var data = LoadAllScores();
		if (!data.ContainsKey(course)) return 0;
		if (!data[course].ContainsKey(levelName)) return 0;
		return data[course][levelName];
	}

	// ====================
	// 🔒 Encryption Helpers
	// ====================
	private void SaveEncrypted(string plainText)
	{
		using (Aes aes = Aes.Create())
		{
			aes.Key = Encoding.UTF8.GetBytes(GetEncryptionKey().PadRight(32).Substring(0, 32));
			aes.IV = iv;

			ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
			using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
			using (CryptoStream cs = new CryptoStream(fs, encryptor, CryptoStreamMode.Write))
			using (StreamWriter sw = new StreamWriter(cs))
			{
				sw.Write(plainText);
			}
		}
	}

	private string LoadDecrypted()
	{
		using (Aes aes = Aes.Create())
		{
			aes.Key = Encoding.UTF8.GetBytes(GetEncryptionKey().PadRight(32).Substring(0, 32));
			aes.IV = iv;

			ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
			using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			using (CryptoStream cs = new CryptoStream(fs, decryptor, CryptoStreamMode.Read))
			using (StreamReader sr = new StreamReader(cs))
			{
				return sr.ReadToEnd();
			}
		}
	}
}