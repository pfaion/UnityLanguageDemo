using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AgentScript : MonoBehaviour {

	public Canvas canvas;
	public Text uiText;
	string currentPhrase = "";

	// Use this for initialization
	void Start () {
		uiText.text = "";
	}

	public void SetCurrentPhrase(string phrase) {
		currentPhrase = phrase;
	}
	
	// Update is called once per frame
	void Update () {
		uiText.text = currentPhrase;
	}
}
