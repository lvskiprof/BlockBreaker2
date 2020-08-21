using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEditor;
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
    int                 maxHits;                // The maximum number of times a block must be hit to be destroyed
    [SerializeField]
    int                 timesHit;               // Only serialized for debugging

    /***
	*		Cached componenet references.
	***/

    Level       currentLevel;           // Level object for this level the block is on
    GameStatus  gameStatus;             // GameStatus object for this level the block is on

    /***
    *       Start() is used to cache the Level object and add this block to the total count.
    ***/
    void Start()
	{
        maxHits = hitSprites.Length + 1;    // The number of Sprites will cause this to be set
		gameStatus = FindObjectOfType<GameStatus>();
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
    *   a block.  Later on this will probably change it to broken blocks until it is destroyed.
    ***/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CompareTag(breakable))
		{
			HandleHit();
		}   // if
		else if (CompareTag(unbreakable))
		{

		}   // else

        //Debug.Log(collision.gameObject.name);   // Log what collided with the block
    }   // OnCollisionEnter2D()

	private void HandleHit()
	{
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

	private void ShowNextHitSprite()
	{
        int     spriteIndex = timesHit - 1;

        GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
    }   // ShowNextHitSprite()
}   // class Block