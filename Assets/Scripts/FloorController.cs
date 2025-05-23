using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorController : MonoBehaviour
{
    private Tilemap landBoundary;
    private Tilemap openAir;	
    private Rigidbody2D _playerRigidbody;
	
	void Start()
    {
        // Find the tilemaps
        landBoundary = GameObject.Find("Land_Boundary").GetComponent<Tilemap>();
		openAir = GameObject.Find("Open_Air").GetComponent<Tilemap>();
    }

    // Detect when the player steps onto the floor, the dangerous tilemap should go away
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            _playerRigidbody = collider.GetComponent<Rigidbody2D>();
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

    // Detect when the player leaves the floor, the tilemap should return, allowing them to fall
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (collider.GetComponent<Rigidbody2D>() == _playerRigidbody)
            {
                _playerRigidbody = null;
            }
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
