//using System.Diagnostics;
using UnityEngine;

public class Block : MonoBehaviour
{
	/***
    *		Enumerated definitions and constant definitions.
    ***/
	public enum BlockColor
	{
		Red,
		Green,
		White,
		Yellow,
		Grey
	}   // enum BlockColor

	const string breakable = "Breakable";       // Tag value for a breakable block
	const string unbreakable = "Unbreakable";   // Tag value for an unbreakable block

	/***
	*		Configurable parameters set in Unity Editor.
	***/

	[SerializeField]
	public BlockColor   blockColor;             // Set in the prefab for each color of block
	[SerializeField]
	GameObject          blockSparklesVFX;       // Prefab that provides a cute visual effect when a block is destroyed
	[SerializeField]
	AudioClip[]         blockDestroyedSounds;   // Assigned in Unity Editor
	[SerializeField]
	Sprite[]            hitSprites;             // List of sprites to use to show the damage level of a block (may be empty)
	[SerializeField]
	float               stickiness = 1.0f;		// Adjust ball velocity plus or minus when hit.  A value of 1.0 leaves it as is, < 1.0 slows it down, > 1.0 speeds it up
	[SerializeField]
	int                 timesHit;               // Only serialized for debugging

	/***
	*		Cached componenet references.
	***/

	Level       currentLevel;           // Level object for this level the block is on
	GameStatus  gameStatus;             // GameStatus object for this level the block is on
	Ball        ball;					// The ball we are using for this level

	/***
    *       Start() is used to cache the Level object and add this block to the total count.
    ***/
	void Start()
	{
		gameStatus = FindObjectOfType<GameStatus>();
		ball = FindObjectOfType<Ball>();
		blockDestroyedSounds[0] = blockDestroyedSounds[0];
		currentLevel = FindObjectOfType<Level>();
		if (CompareTag(breakable))
		{   // Only count the "Breakable" blocks
			currentLevel.CountBlocks();
		}   // if
	}   // Start()

	/***
    *       Update() is not used yet.
    ***/
	void Update()
	{
		// Reserved for Future Use.
	}   // Update()

	/***
    *       DestroyBlock() will handle the housekeeping needed when we destroy a block.
    ***/
	private void DestroyBlock()
	{
		gameStatus.AddToScore(this);
		PlayBlockDestroySFX();
		TriggerSparklesVFX();
		Destroy(gameObject);
		currentLevel.BlockDestroyed();
	}   // DestroyBlock()

	/***
    *       PlayBlockDestroySFX() will play the sound effects (SFX) when a block is destroyed.
    ***/
	private void PlayBlockDestroySFX()
	{
		AudioSource.PlayClipAtPoint(blockDestroyedSounds[0], Camera.main.transform.position);
	}

	/***
    *       TriggerSparklesVFX() will create the visual effects (VFX) when a block is destroyed.  The
    *   visual effect object will be destroyed after a second.
    ***/
	private void TriggerSparklesVFX()
	{
		GameObject sparkles = Instantiate(blockSparklesVFX, gameObject.transform.position, gameObject.transform.rotation);
		Destroy(sparkles, 1f);  // If you do not destroy the instantiated object, it stays around and you will eventually run out of resources
	}   // TriggerSparklesVFX()

	/***
    *       OnCollisionEnter2D() will handle the housekeeping when there is a collision with
    *   a block.
    ***/
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (CompareTag(breakable))
		{
			HandleHit();
		}   // if
		//else if (CompareTag(unbreakable))
		{	// If something special needs to be done for unbreakable blocks, uncomment else-if above and put it here

		}   // else

		//Debug.Log(collision.gameObject.name);   // Log what collided with the block
	}   // OnCollisionEnter2D()

	/***
    *       HandleHit() will determine if the block is destroyed, based on how many times it can be
    *   hit.  If it is destroyed it will call DestroyBlock(). but if it takes more hits it will call
    *   ShowNextHitSprite() to display the next damage level image.
    ***/
	private void HandleHit()
	{
		int		maxHits = hitSprites.Length + 1;    // The number of Sprites will cause this to be set

		if (stickiness != 1.0f)
		{   // Change the ball velocity by the stickiness multiple
			Rigidbody2D myRigidBody2D = ball.GetComponent<Rigidbody2D>();
			Debug.Log("Applying stickiness factor of " + stickiness);
			Debug.Log("Before magnitude = " + myRigidBody2D.velocity.magnitude +
	", x = " + myRigidBody2D.velocity.x + ", y = " + myRigidBody2D.velocity.y);

			ball.ChangeVelocity(stickiness);
			Debug.Log("After magnitude = " + myRigidBody2D.velocity.magnitude +
				", x = " + myRigidBody2D.velocity.x + ", y = " + myRigidBody2D.velocity.y);
			if (stickiness > 1.0f)
			{
				stickiness -= 0.1f; //	Make a damaged block a little less hard each time it is hit
				Debug.Log("New stickiness = " + stickiness);
			}	// if
		}   // if

		timesHit++;
		if (timesHit >= maxHits)
		{
			DestroyBlock();
		}   // if
		else
		{   // Show next hit sprite
			ShowNextHitSprite();
		}   // else
	}   // HandleHit()

	/***
    *       ShowNextHitSprite() will show the next Sprite image for this block.  Each one should
    *   show more damage.  It also checks to verify that a Sprite image has been assigned to the
    *   entry it is being asked to show.  If there isn't one it will log it as an error and do nothing.
    ***/
	private void ShowNextHitSprite()
	{
		int     spriteIndex = timesHit - 1;

		if (hitSprites[spriteIndex] != null)
		{   // This element has a Sprite set in it
			GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
		}   // if
		else
		{   // This element did not have a Sprite assign to it
			UnityEngine.Debug.LogError("Block Sprite is missing from array at element " + spriteIndex + " for " + gameObject.name);
		}   // else
	}   // ShowNextHitSprite()
}   // class Block