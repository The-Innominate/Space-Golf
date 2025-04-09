using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StrokeTextUpdate : MonoBehaviour
{
    TextMeshProUGUI StrokeText;

    [SerializeField] PlayerScript Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		StrokeText = GetComponent<TextMeshProUGUI>();
	}

    // Update is called once per frame
    void Update()
    {
        StrokeText.text = "Strokes: " + Player.getStrokes().ToString();
	}
}
