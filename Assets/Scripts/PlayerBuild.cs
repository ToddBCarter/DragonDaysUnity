using UnityEngine;
using TMPro;

public class PlayerBuild : MonoBehaviour
{
    public GameObject objectToPlacePrefab; //Assign in Inspector; this should change to buttons on menu
    public float gridSize = 1f;
	
	//private bool x;
	
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
				GameObject newObject = Instantiate(objectToPlacePrefab, gridPos, Quaternion.identity);
				
				//Debug.Log("Plank drop test.");
				
				//This is Y filtering to guarantee sprites appear in front/behind correctly.
				//This works by inverting the Y coordinate, making objects with lower Y,
				//values appear above those with higher Y.
				//This works along with the sorting layers to keep sprites in place.
				SpriteRenderer sr = newObject.GetComponent<SpriteRenderer>();
				if (sr != null)
				{
					//Invert Y so lower objects are drawn in front
					sr.sortingOrder = Mathf.RoundToInt(-gridPos.y * 10); // Multiply for more granularity
				}
			}
        }
	}
	
	Vector2 GetPrefabSize(GameObject prefab)
	{
		GameObject temp = Instantiate(prefab); //Temporary object to get accurate size
		temp.SetActive(false);
		
		Collider2D col = prefab.GetComponent<Collider2D>();
		Vector2 size = Vector2.one; //Set as default
		
		if (col != null)
		{
			if(col is BoxCollider2D box)
			{
				size = box.size;
				//Debug.Log("Box collider.");
			}
			else
			{
				size = col.bounds.size; //Get the size of the collider directly
			}
			//Debug.Log("Prefab size collider test x " + size.x);
			//Debug.Log("Prefab size collider test y " + size.y);			
		}
		
		DestroyImmediate(temp); //Cleanup
		return size;
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
		//Collider2D hit = Physics2D.OverlapBox(position, adjustedSize, 0f);
		
		Collider2D[] hits = Physics2D.OverlapBoxAll(position, adjustedSize, 0f);
		
		//This boolean prevents collision with ANY colliders,
		//so no more building over terrain or whatever.
		bool x = false;

		foreach (Collider2D hit in hits)
		{		
			if(hit is BoxCollider2D box)
			{
				//Debug.Log("Space occupy test");
				Debug.Log("Hit x coord is " + box.size.x);
				Debug.Log("Hit y coord is " + box.size.y);
			}
			if (hit.CompareTag("WoodenPlank"))
			{
				//Space is considered occupied only if a specific tag is found
				//return true;
				//float overlapAmount = 0.5f;
				//snapPos.y += overlapAmount;
				Debug.Log("Plank hit.");
				//return true;
			}
			x = true;
		}
			
		//Collider2D[] objects = Physics2D.OverlapBoxAll(position, adjustedSize, 0f);

		return x;
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
