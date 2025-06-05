using UnityEngine;
using TMPro;

public class PlayerBuild : MonoBehaviour
{
    public GameObject objectToPlacePrefab; //Assign in Inspector; this should change to buttons on menu
    public float gridSize = 1f;
	
	private float snapRange = 2f;
	
	//private bool x;
	
	private void Update()
	{		
		if (Input.GetMouseButtonDown(0)) //Left click
        {
			//This needs to change a bit for dynamic/snapping placement.
			//SnapToGrid needs to check if its within range of a snap point.
			//Then it needs to determine which snap position is the closest.
			//			
			
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 objectSize = GetPrefabSize(objectToPlacePrefab);
            //Vector2 gridPos = SnapToGrid(mouseWorldPos, objectSize);
			
			Vector2 gridPos = GetSnapPosition(mouseWorldPos);

			if(!IsSpaceOccupied(gridPos, objectSize))
			{
				//Instantiate the object at the snapped grid position
				GameObject newObject = Instantiate(objectToPlacePrefab, gridPos, Quaternion.identity);
				
				//Debug.Log("Plank drop test.");
				
				//This is Y filtering to guarantee sprites appear in front/behind correctly.
				//This works by inverting the Y coordinate, making objects with lower Y
				//values appear on higher layers than those with higher Y.
				//This works along with the sorting layers to keep sprites in place.
				SpriteRenderer sr = newObject.GetComponent<SpriteRenderer>();
				if (sr != null)
				{
					//Invert Y so lower objects are drawn in front
					sr.sortingOrder = Mathf.RoundToInt(-gridPos.y * 100); //Multiply for more granularity
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
	
	//
	//This function is useful for aligning to a world grid.
	//
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
				//Space is considered occupied only if a specific tag is found
				//return true;
				float overlapAmount = 0.5f;
				snapPos.y += overlapAmount;
			}
		}*/		
		
        return snapPos;
    }
	
	//
	//This function dynamically snaps an object to a detected object by tag
	//
	Vector2 GetSnapPosition(Vector2 mousePos)
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(mousePos, snapRange);
		Collider2D closest = null;
		float closestDist = Mathf.Infinity;

		//Compare hits from overlap circle for closest distance
		foreach (var hit in hits)
		{
			if (hit.CompareTag(objectToPlacePrefab.tag))
			{
				float dist = Vector2.Distance(mousePos, hit.transform.position);
				if (dist < closestDist)
				{
					closest = hit;
					closestDist = dist;
				}
			}
		}

		if (closest != null)
		{
			Vector2 size = GetPrefabSize(objectToPlacePrefab);
			Vector2 basePos = closest.transform.position;
			Vector2 diff = mousePos - basePos;
			Vector2 offset = Vector2.zero;

			//Get the direction of the detected object, then
			//normalize the direction for a snap effect
			Vector2 dir = new Vector2(
				Mathf.RoundToInt(diff.x / size.x),
				Mathf.RoundToInt(diff.y / size.y)
			);

			//Clamp to -1, 0, 1 to avoid large jumps
			//This becomes a multiplier for the size of the object to create the offset
			//This effectively creates local grid positioning for the snap effect
			dir.x = Mathf.Clamp(dir.x, -1, 1);
			dir.y = Mathf.Clamp(dir.y, -1, 1);

			//If no direction detected (mouse is directly over), default to placing above
			if (dir == Vector2.zero)
			{
				dir = Vector2.up; //shorthand 0,1
			}
			
			/*if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
			{
				// Snap left or right
				offset.x = Mathf.Sign(diff.x) * size.x;
			}
			else
			{
				// Snap top or bottom
				offset.y = Mathf.Sign(diff.y) * size.y;
			}Frankenstein this for above/below positioning.*/

			//Calculate the offset and add it to the basePos
			offset = new Vector2(dir.x * size.x, dir.y * size.y);
			
			//Find a different offset for different tags using an if statement
			//if(objectToPlacePrefab.CompareTag("WoodenPlank"){
			//if(objectToPlacePrefab.CompareTag("WoodenWall"){
				
			//HOWEVER
			//This NEEDS a way to change what prefab is being used.
			//So
			//Button time...?
			
			
			
			return basePos + offset;
		}

		//If no nearby plank, just return the mouse position (or optionally SnapToGrid fallback)
		//return SnapToGrid(mousePos, GetPrefabSize(objectToPlacePrefab));
		return mousePos;
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
		
		//Instead of snap to grid, need a dynamic option
		//Vector2 gridPos = SnapToGrid(mouseWorldPos, size);
		Vector2 gridPos = GetSnapPosition(mouseWorldPos);
		Vector2 adjustedSize = size * 0.9f;

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(gridPos, adjustedSize);
	}
	
	public void SetObjectToPlace(GameObject newPrefab)
	{
		objectToPlacePrefab = newPrefab;
	}
}
