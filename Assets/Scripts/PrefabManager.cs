using UnityEngine;
using System.Collections.Generic;

//
//The purpose of this manager may shift
//Might be better to have it provide a way to change
//the current build options, such as material type.
//
//This may be better for the Build Menu, as this may
//organize the assignments.
public class PrefabManager : MonoBehaviour
{
	[System.Serializable]
    public struct PrefabEntry
    {
        public PrefabType type;
        public GameObject prefab;
    }

    public List<PrefabEntry> prefabList;

    private Dictionary<PrefabType, GameObject> prefabDict;

    public static PrefabManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        prefabDict = new Dictionary<PrefabType, GameObject>();
        foreach (var entry in prefabList)
        {
            prefabDict[entry.type] = entry.prefab;
        }
    }

    public GameObject GetPrefab(PrefabType type)
    {
        if (prefabDict.TryGetValue(type, out GameObject prefab))
        {
            return prefab;
        }

        Debug.LogWarning($"Prefab of type {type} not found.");
        return null;
    }
	
	public enum PrefabType
	{
		WoodenFloorPlank,
		WoodenWallSide,
		WoodenWallForward
	}
	
}
