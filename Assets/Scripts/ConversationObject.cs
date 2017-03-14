using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEEDSIM;
using Simulation;

public static class IListExtensions {
	/// <summary>
	/// Shuffles the element order of the specified list with Fisher-Yates algorithm.
	/// </summary>
	public static void Shuffle<T>(this IList<T> ts) {
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}
}

public class ConversationObject : MonoBehaviour {

	NEEDSIMNode needsimNode;

	// Use this for initialization
	void Start () {

		needsimNode = gameObject.GetComponent<NEEDSIMNode> ();
	}
	
	// Update is called once per frame
	void Update () {

		NEEDSIMNode agent1 = null;
		NEEDSIMNode agent2 = null;

		string children = "";
		Debug.Log (GetComponentInChildren<NEEDSIMNode>());

//		// shuffle the list and take first two agents interacting with it
//		List<Slot> slots = new List<Slot> (needsimNode.AffordanceTreeNode.Slots);
//		slots.Shuffle();
//		foreach(Slot slot in slots) {
//			if(slot.SlotState == Slot.SlotStates.ReadyCharacter) {
//				int a = 42;
//			}
//		}

		
	}


}
