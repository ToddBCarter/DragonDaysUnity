using UnityEngine;
using TMPro;

public class PlayerBuild : MonoBehaviour
{
    public GameObject objectToPlacePrefab; //Assign in Inspector; this should change to buttons on menu
    public float gridSize = 1f;
	
	private void Update()
	{		
		if (Input.GetMouseButtonDown(0)) //Left click
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 objectSize = GetPrefabSize(objectToPlacePrefab);
            Vector2 gridPos = SnapToGrid(mouseWorldPos, objectSize);

			if(!IsSpaceOccupied(gridPos, objectSize))
			{
				//Instantiate the object at the snapped grid position
				Instantiate(objectToPlacePrefab, gridPos, Quaternion.identity);	
				//Debug.Log("Plank drop test.");
			}
        }
	}
	
	Vector2 GetPrefabSize(GameObject prefab)
	{
		Collider2D col = prefab.GetComponent<Collider2D>();
		if (col != null)
		{
			//
			//Adjusting the size of the box collider allows for some overlap... sort of
			if(col is BoxCollider2D box)
			{
				Debug.Log("Prefab size collider test " + box.size.x);
				Debug.Log("Prefab size collider test " + box.size.y);
				return box.size;
			}
		}
		
		SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
		if (sr != null)
		{
			return sr.bounds.size;
		}
		
		return Vector2.one; //Default if no renderer
	}
	
    Vector2 SnapToGrid(Vector2 rawWorldPos, Vector2 size)
    {
        float x = Mathf.Round(rawWorldPos.x / size.x) * size.x;
        float y = Mathf.Round(rawWorldPos.y / size.y) * size.y;
		
		Vector2 snapPos = new Vector2(x, y);
		
		/*Collider2D[] hits = Physics2D.OverlapBoxAll(snapPos, size, 0f);

		foreach (Collider2D hit in hits)
		{
			if (hit.CompareTag("WoodenPlank")) // Replace with the tag you care about
			{
				// Space is considered occupied only if a specific tag is found
				//return true;
				float overlapAmount = 0.5f;
				snapPos.y += overlapAmount;
			}
		}*/		
		
        return snapPos;
    }

	private bool IsSpaceOccupied(Vector2 position, Vector2 size)
	{
		Vector2 adjustedSize = size * 0.9f;
		Collider2D hit = Physics2D.OverlapBox(position, adjustedSize, 0f);
		
		//Debug.Log("Space occupy test");
		//Debug.Log("Hit x coord is " + hit.size.x);
		//Debug.Log("Hit y coord is " + hit.size.y);
		
		//Collider2D[] objects = Physics2D.OverlapBoxAll(position, adjustedSize, 0f);

		return hit != null;
	}


	void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;
		if (objectToPlacePrefab == null) return;

		Vector2 size = GetPrefabSize(objectToPlacePrefab);
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 gridPos = SnapToGrid(mouseWorldPos, size);
		Vector2 adjustedSize = size * 0.9f;

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(gridPos, adjustedSize);
	}
}
