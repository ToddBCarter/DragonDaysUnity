using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float speed = 5;
	public int facingDirection = 1;
	public Rigidbody2D rb;
	public Animator anim;
	public SpriteRenderer sr;

	private bool isKnockedBack;

	public PlayerCombat playerCombat;
	
	//This should probably be updated to be a part of the event system.
	//That means the calls in update become more complex and use GameEventsManager.

	//Update currently used to acquire player attack input, as set in 
	// Edit -> Project Manager -> Input Manager -> Axes
	private void Update()
	{
		if(Input.GetButtonDown("PlayerAttack"))
		{
			playerCombat.Attack();
		}
		if(Input.GetButtonDown("PlayerActivate"))
		{
			playerCombat.activateEnemy();
		}
		
		sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100); //Multiply for more granularity
	}


    // Update is called once per frame
	//FixedUpdate runs 50 times per second, which makes it more reliable for physics
    void FixedUpdate()
    {
		//If not being knocked back, then the player moves normally.
		if(isKnockedBack == false)
		{
			//Get player controls:
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");

			//Flip the sprite animation.
			if(horizontal > 0 && transform.localScale.x < 0 || 
				horizontal < 0 && transform.localScale.x > 0){
				Flip();
			}

			anim.SetFloat("horizontal", Mathf.Abs(horizontal));
			anim.SetFloat("vertical", Mathf.Abs(vertical));
			
			//anim.SetFloat("horizontal", horizontal); //This gives -1 to 1 for x
			//anim.SetFloat("vertical", vertical); //This gives -1 to 1 for y
			
			rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
		}
    }

	//Flip the player animation.
	void Flip()
	{
		facingDirection *= -1;
		transform.localScale = new Vector3 (transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
	}

	//Knock back the player.
	public void Knockback(Transform enemy, float force, float stunTime)
	{
		isKnockedBack = true;
		Vector2 direction = (transform.position - enemy.position).normalized;
		rb.linearVelocity = direction * force;
		StartCoroutine(KnockbackCounter(stunTime));
	}

	//Run a coroutine (c# thing) for player stun time after being hit.
	//This is why using Systems.Collections
	IEnumerator KnockbackCounter(float stunTime)
	{
		yield return new WaitForSeconds(stunTime);
		rb.linearVelocity = Vector2.zero;
		isKnockedBack = false;
	}
}

public enum PlayerState //May not need this.
{
	Idle,
	Right,
	Left,
	Up,
	Down
}