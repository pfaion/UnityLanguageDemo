using UnityEngine;
using System.Collections;
using ContextualDialogue.WorldManager;
using ContextualDialogue.DialogueGenerator;

public class Test : MonoBehaviour {

	WorldEngine worldEngine;
	DialogueGenerator dialogueGenerator;

	public AgentScript agent1;
	public AgentScript agent2;


	// Use this for initialization
	void Start () {
		worldEngine = new WorldEngine ();
		worldEngine.loadWorldFile ();

		dialogueGenerator = new DialogueGenerator (worldEngine);

		ConversationalParamaters cParams = new ConversationalParamaters(ConversationalParamaters.conversationType.helloOnly, "Agent 1", "Agent 2");
		cParams.greetingMode = ConversationalParamaters.GreetingMode.fourTurn;
		cParams.farewellMode = ConversationalParamaters.FarewellMode.simple;//by default conversation type helloOnly doesnt have a farewell
		cParams.conversationLocation = worldEngine.world.findByProperNoun("Germany");

		QUDitem q = new QUDitem(QUDitem.ExchangeTypeEnum.where);
		q.subject = worldEngine.world.findByProperNoun("Westerberg Campus");

		cParams.addQUDitem(q);



		if (dialogueGenerator.conversation == null)
			dialogueGenerator.newConversation(cParams);

		StartCoroutine(displayDialog ());

		dialogueGenerator.conversation.closeConversation ();

		StartCoroutine(displayDialog ());
	}

	IEnumerator displayDialog () {
		while (dialogueGenerator.hasNextOutput()) {
			Turn turn = dialogueGenerator.getOutput ();
			if (turn.participant == "Agent 1") {
				agent1.SetCurrentPhrase (turn.utterance);
			} else {
				agent2.SetCurrentPhrase (turn.utterance);
			}
			yield return new WaitForSeconds (4);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
