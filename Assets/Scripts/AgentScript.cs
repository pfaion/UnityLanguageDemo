using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AgentScript : MonoBehaviour {

	public Canvas canvas;
	public Text uiText;
	string currentPhrase = "";

	void Start () {
		uiText.text = "";
	}

	public void SetCurrentPhrase(string phrase) {
		currentPhrase = phrase;
	}

	void Update () {
		uiText.text = currentPhrase;

		// Billboarding function for the canvas
		Camera cam = Camera.current;
		if (cam != null) canvas.transform.forward = cam.transform.forward;

		// Hide canvas if empty
		canvas.gameObject.SetActive (currentPhrase != "");
	}

}
