using UnityEngine;

public class TrailColor : MonoBehaviour
{
	public int x, y;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		Color pixelColor = new Color(PlayerPrefs.GetFloat("SelectedBallSkinR", 1f),
			PlayerPrefs.GetFloat("SelectedBallSkinG", 1f),
			PlayerPrefs.GetFloat("SelectedBallSkinB", 1f),
			1f);

		TrailRenderer trailRenderer = GetComponent<TrailRenderer>();

		if(trailRenderer != null)
		{
			trailRenderer.startColor = new Color(pixelColor.r, pixelColor.g, pixelColor.b, 1f);
			trailRenderer.endColor = new Color(pixelColor.r, pixelColor.g, pixelColor.b, 0f);
		}
		else
		{
			Debug.LogError("TrailRenderer component is missing");
		}
	}
}
