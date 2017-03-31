using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEEDSIM;
using Simulation;
using ContextualDialogue.WorldManager;
using ContextualDialogue.DialogueGenerator;


public class ConversationController : MonoBehaviour {

	public enum State {
		NoConversation,
		ConversationRunning,
		ConversationEnding
	}

	public int turnTime = 3;



	List<NEEDSIMNode> agents;

	NEEDSIMNode agent1;
	NEEDSIMNode agent2;

	State state;

	System.DateTime endTimer;


	WorldEngine worldEngine;
	DialogueGenerator dialogueGenerator;
	ConversationalParamaters cParams;




	// Use this for initialization
	void Start () {

		agents = new List<NEEDSIMNode> ();
		agent1 = null;
		agent2 = null;

		state = State.NoConversation;


		worldEngine = new WorldEngine ();
		worldEngine.loadWorldFile ();

		dialogueGenerator = new DialogueGenerator (worldEngine);

		cParams = new ConversationalParamaters(ConversationalParamaters.conversationType.helloOnly, "<agent1>", "<agent2>");
		cParams.greetingMode = ConversationalParamaters.GreetingMode.fourTurn;
		cParams.farewellMode = ConversationalParamaters.FarewellMode.simple;
		cParams.conversationLocation = worldEngine.world.findByProperNoun("Germany");

		QUDitem q = new QUDitem(QUDitem.ExchangeTypeEnum.where);
		q.subject = worldEngine.world.findByProperNoun("Westerberg Campus");

		cParams.addQUDitem(q);
	}

	void SetAgentColors(Color c) {
		Renderer rend1 = agent1.gameObject. GetComponentInChildren<Renderer> ();
		rend1.material.shader = Shader.Find ("Standard");
		rend1.material.SetColor ("_Color", c);
		Renderer rend2 = agent2.gameObject. GetComponentInChildren<Renderer> ();
		rend2.material.shader = Shader.Find ("Standard");
		rend2.material.SetColor ("_Color", c);
	}

	IEnumerator displayDialog () {
		while (dialogueGenerator.hasNextOutput()) {
			Turn turn = dialogueGenerator.getOutput ();
			string utterance = turn.utterance;
			utterance = utterance.Replace ("<agent1>", agent1.gameObject.GetComponent<AgentScript>().name);
			utterance = utterance.Replace ("<agent2>", agent2.gameObject.GetComponent<AgentScript>().name);
			if ((string)(turn.participant) == "<agent1>") {
				agent1.gameObject.GetComponent<AgentScript>().SetCurrentPhrase (utterance);
				agent2.gameObject.GetComponent<AgentScript>().SetCurrentPhrase ("");
			} else {
				agent1.gameObject.GetComponent<AgentScript>().SetCurrentPhrase ("");
				agent2.gameObject.GetComponent<AgentScript>().SetCurrentPhrase (utterance);
			}
			yield return new WaitForSeconds (turnTime);
		}
		agent1.gameObject.GetComponent<AgentScript>().SetCurrentPhrase ("");
		agent2.gameObject.GetComponent<AgentScript>().SetCurrentPhrase ("");
	}


	void Update() {

		string log = agents.Count + " agents registered: ";
		foreach(NEEDSIMNode agent in agents) {
			log += agent.name + ", ";
		}



		if (state == State.NoConversation) {
			if (agents.Count >= 2) {
				PickRandomPair ();
				state = State.ConversationRunning;
				dialogueGenerator.newConversation(cParams);
				StartCoroutine(displayDialog ());
			}
		} else if (state == State.ConversationEnding) {
			SetAgentColors (Color.red);
			if (!dialogueGenerator.hasNextOutput()) {
				state = State.NoConversation;
				SetAgentColors (Color.white);
			}
		} else if (state == State.ConversationRunning) {
			SetAgentColors (Color.green);
		}
			

	}

	public bool AgentRegistered(NEEDSIMNode agent) {
		return agents.Contains (agent);
	}
		

	public void RegisterAgent(NEEDSIMNode agent) {
		Debug.Log ("Registering agent");
		agents.Add (agent);
		
	}

	public bool DeregisterAgent(NEEDSIMNode agent) {
		Debug.Log ("Agent wants deregister: " + agent.name);
		if (AgentRegistered (agent)) {
			agents.Remove (agent);
		}
		if (state == State.NoConversation || (agent != agent1 && agent != agent2)) {
			Debug.Log ("Deregistering Agent savely: " + agent.name);
			return true;
		}
		if (state == State.ConversationRunning) {
			EndConversation ();
		}
		return false;
	}


	public void EndConversation() {
		if (state == State.NoConversation) {
			return;
		}
		Debug.Log ("Ending Conversation first...");

		dialogueGenerator.conversation.closeConversation ();
		state = State.ConversationEnding;

	}
		

	void PickRandomPair() {

		int i1 = Random.Range (0, agents.Count - 1);
		int i2 = Random.Range (0, agents.Count - 2);

		if (i2 >= i1) {
			i2 += 1;
		}

		agent1 = agents [i1];
		agent2 = agents [i2];

	}
}
