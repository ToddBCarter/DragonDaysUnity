using UnityEngine;

public class WallController : MonoBehaviour
{
	public SpriteRenderer sr;
    // Update is called once per frame
    void Update()
    {	
		//y-filtering for rendering
		sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
