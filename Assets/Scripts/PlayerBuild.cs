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

            //Instantiate the object at the snapped grid position
            Instantiate(objectToPlacePrefab, gridPos, Quaternion.identity);			
        }
	}
	
	Vector2 GetPrefabSize(GameObject prefab)
	{
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
        return new Vector2(x, y);
    }	

}
