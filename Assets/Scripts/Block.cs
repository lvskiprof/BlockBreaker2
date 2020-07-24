using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    AudioClip[] blockDestroyedSounds;   // Assigned in Unity Editor

    /***
	*		Cached componenet references.
	***/

    Level       currentLevel;           // Level object for this level the block is on
    GameStatus  gameStatus;             // GameStatus object for this level the block is on

    /***
    *       Start is used to cache the Level object and add this block to the total count.
    ***/
    void Start()
    {
        currentLevel = FindObjectOfType<Level>();
        gameStatus = FindObjectOfType<GameStatus>();
        currentLevel.CountBreakableBlocks();
        blockDestroyedSounds[0] = blockDestroyedSounds[0];
    }   // Start()

    /***
    *       Update is not used yet.
    ***/
    void Update()
    {

    }   // Update()

    /***
    *       DestroyBlock() will handle the housekeeping needed when we destroy a block.
    ***/
    private void DestroyBlock()
    {
        AudioSource.PlayClipAtPoint(blockDestroyedSounds[0], Camera.main.transform.position);
        Destroy(gameObject);
        currentLevel.BlockDestroyed();
        gameStatus.AddToScore();
    }   // DestroyBlock()

    /***
    *       OnCollisionEnter2D() will handle the housekeeping when there is a collision with
    *   a block.  Later on this will probably change it to broken blocks until it is destroyed.
    ***/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyBlock();
        //Debug.Log(collision.gameObject.name);   // Log what collided with the block
    }   // OnCollisionEnter2D()
}   // class Block