using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    public GameObject objectToPlacePrefab; //Assign in Inspector
    public float gridSize = 1f;
	
	private void Update()
	{
		if (Input.GetMouseButtonDown(0)) //Left click
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPos = SnapToGrid(mouseWorldPos);

            //Instantiate the object at the snapped grid position
            Instantiate(objectToPlacePrefab, gridPos, Quaternion.identity);
        }
	}
	
    Vector2 SnapToGrid(Vector2 rawWorldPos)
    {
        float x = Mathf.Round(rawWorldPos.x / gridSize) * gridSize;
        float y = Mathf.Round(rawWorldPos.y / gridSize) * gridSize;
        return new Vector2(x, y);
    }	

}
