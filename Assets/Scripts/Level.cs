using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField]
    int blocks;     // Count for how many blocks are on this level
    [SerializeField]
    AudioClip[] blockDestroyedSounds;

    /***
	*		Cached componenet references.
	***/

    SceneLoader sceneLoader;

    /***
    *       Start is used to cache the Level object.
    ***/
    void Start()
    {
        if (sceneLoader == null)
        {
            sceneLoader = FindObjectOfType<SceneLoader>();
        }   // if
    }   // Start()

    /***
    *       CountBreakableBlocks() will count up how many blocks are on this level.
    ***/
    public void CountBreakableBlocks()
    {
        blocks++;
    }   // CountBreakableBlocks()

    /***
    *       BlockDestroyed() will decrement how many blocks are on this level and will load the
    *   next level when it reaches 0.
    ***/
    public void BlockDestroyed()
    {
        blocks--;
        if (blocks <= 0)
            sceneLoader.LoadNextScene();
    }   // BlockDestroyed()
}   // class Level