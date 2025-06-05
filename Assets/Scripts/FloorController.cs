using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorController : MonoBehaviour
{
    [SerializeField] private Tilemap landBoundary;
    [SerializeField] private Tilemap openAir;	
    private Rigidbody2D _playerRigidbody;
	
	private static int floorContactCount;
	
	void Start()
    {
        // Find the tilemaps
		var allTilemaps = Resources.FindObjectsOfTypeAll<Tilemap>();
		foreach (Tilemap tilemap in allTilemaps)
		{
			if (tilemap.name == "Open_Air")
			{
				openAir = tilemap;
			}
			if (tilemap.name == "Land_Boundary")
			{
				landBoundary = tilemap;
			}
		}
    }

    // Detect when the player steps onto the floor, the dangerous tilemap should go away
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            _playerRigidbody = collider.GetComponent<Rigidbody2D>();
			
			//Keep track of the floor count so as to protect the player moving from
			//one collider to the next.
			floorContactCount++;
			
			Debug.Log("Enter floor count is " + floorContactCount);
			
			if (floorContactCount == 1)
			{
							
				if (landBoundary != null && landBoundary.gameObject.activeSelf)
				{
					landBoundary.gameObject.SetActive(false); // Hide the tilemap layer
				}
				if (openAir != null && openAir.gameObject.activeSelf)
				{
					openAir.gameObject.SetActive(false); // Hide the tilemap layer
				}
				
			}
        }        
    }

    // Detect when the player leaves the floor, the tilemap should return, allowing them to fall
	//This currently is killing the player when they step off a floor and onto a moving platform.
	//Which raises the need for a TilemapManager which will keep track of floorContactCount
	//and allow moving plaforms to add to the count.
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (collider.GetComponent<Rigidbody2D>() == _playerRigidbody)
            {
                _playerRigidbody = null;
            }
			
			floorContactCount = Mathf.Max(0, floorContactCount - 1);
			
			Debug.Log("Exit floor count is " + floorContactCount);
			
			if(floorContactCount == 0)
			{
				if (landBoundary != null && !landBoundary.gameObject.activeSelf)
				{
					landBoundary.gameObject.SetActive(true); // Show the tilemap layer
				}
				if (openAir != null && !openAir.gameObject.activeSelf)
				{
					openAir.gameObject.SetActive(true); // Hide the tilemap layer
				}
			}
        }        
    }
}
