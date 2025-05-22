using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;

public class GameEventsManager : MonoBehaviour
{
	public static GameEventsManager instance { get; private set; }

    public DialogueEvents dialogueEvents;
	public PlayerEvents playerEvents;

	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogError("More than one Game Events Manager in that scene.");
		}
		instance = this;

		//Debug.Log("Gameeventsmanager test");
		dialogueEvents = new DialogueEvents();
		playerEvents = new PlayerEvents();
	}
}
