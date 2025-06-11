using UnityEngine;

[CreateAssetMenu(fileName = "PrefabLibrary", menuName = "Scriptable Objects/PrefabLibrary")]
public class PrefabLibrary : ScriptableObject
{
	//This should be a list of all prefabs to be used.
	//These are assigned through the editor to the PrefabLibrary scriptable object (for now).
	//To use them, the scriptable object PrefabLibrary must be assigned to the PlayerBuild script (for now).
    public GameObject WoodenWallForward;
	public GameObject WoodenWallSide;
	public GameObject WoodenPlank;
	public GameObject WoodenStairsForward;
}
