using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;

public class PlayerEvents
{
	/* Template for events:
	public event Action<string> onEnterDialogue;
	
	public void EnterDialogue(string knotName)
	{
		//onEnterDialogue?.Invoke(knotName);

		//Debug.Log("Test " + onEnterDialogue);
		//Debug.Log("1st Test " + knotName);
		
		if (onEnterDialogue != null)
		{
			//Debug.Log("2nd Test " + knotName);
			onEnterDialogue(knotName);
		}
	}
	*/
	
	public event Action<string> onBuildObject;
	
	public void BuildObject(string objectTag)
	{
		if (onBuildObject != null)
		{
			onBuildObject(objectTag);
		}
	}
}
