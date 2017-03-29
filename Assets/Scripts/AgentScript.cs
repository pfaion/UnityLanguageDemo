using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AgentScript : MonoBehaviour {

	public string name;
	public Canvas canvas;
	public Text uiText;
	public Text nameText;
	public GameObject nameRotator;
	string currentPhrase = "";

	void Start () {
		uiText.text = "";
		nameText.text = name;
	}

	public void SetCurrentPhrase(string phrase) {
		currentPhrase = phrase;
	}

	void Update () {
		uiText.text = currentPhrase;

		// Billboarding function for the canvases
		Camera cam = Camera.current;
		if (cam != null) {
			// Full transform for conversation bubble
			Vector3 f = cam.transform.forward;
			canvas.transform.forward = f;

			// Only y-rotation for name label
			f.y = 0.0f;
			nameRotator.transform.forward = f;
		}
			
		// Hide canvas if empty
		canvas.gameObject.SetActive (currentPhrase != "");
	}

}
