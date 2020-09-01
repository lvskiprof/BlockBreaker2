using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    int blocks;     // Count for how many blocks are on this level
    [SerializeField]
    AudioClip[] blockDestroyedSounds;

    /***
	*		Cached componenet references.
	***/

    GameStatus  gameStatus;             // GameStatus object for this level the block is on

    /***
    *       Start is used to cache the Level object.
    ***/
    void Start()
    {
        gameStatus = FindObjectOfType<GameStatus>();
    }   // Start()

    /***
    *       CountBreakableBlocks() will count up how many blocks are on this level.
    ***/
    public void CountBlocks()
    {
        blocks++;
        if (blocks == 1)
            gameStatus.SetAllBlocksDestroyed(false);   // Only need to reset this if there are breakable blocks
    }   // CountBreakableBlocks()

    /***
    *       BlockDestroyed() will decrement how many blocks are on this level and will load the
    *   next level when it reaches 0.
    ***/
    public void BlockDestroyed()
    {
        blocks--;
        if (blocks <= 0)
        {   // Set the flag to show all blocks for this level have been destroyed and load the next level
            gameStatus.SetAllBlocksDestroyed(true);
            FindObjectOfType<SceneLoader>().LoadNextScene();
        }   // if
    }   // BlockDestroyed()
}   // class Level