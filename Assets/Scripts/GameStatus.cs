using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStatus : MonoBehaviour
{
	[Range(0.1f, 3.0f)] [SerializeField]
	float   gameSpeed = 1f;
	[SerializeField]
	int     pointsPerBlockDestroyed = 10;	// This is set in the Unity Editor, so this value is overridden
	[SerializeField]
	TextMeshProUGUI score;

	/***
	*		Current state values.
	***/

	[SerializeField]
	int     currentScore = 0;

	/***
	*		Cached componenet references.
	***/

	Level   currentLevel;   // Level object for this level the game status will work on.

	/***
	*		Awake() runs before anything else, including Start().
	*		Note: DestroyImmediate() is used instead of Destroy(), because there can be a delay with
	*	Destroy() that can cause problems if some code accesses the wrong the newer instance before
	*	it is destroyed.  However, I was told that DestroyImmediate() can have bad side effects, so I
	*	am not using it.  Another option can be to set the gameObject to be inactive before it is
	*	destroyed.  That code would look like this:
	*		gameObject.SetActive(false);
	*		Destroy(gameObject);
	*		
	*		See: http://docs.unity3d.com/Manual/ExecutionOrder.html
	***/
	private void Awake()
	{
		int gameStatusCount = FindObjectsOfType<GameStatus>().Length;
		if (gameStatusCount > 1)
		{   // We only want to keep the first one to preserve game status between scenes
			gameObject.SetActive(false);	// Prevent anything from being able to find or interact with this instance of GameStatus
			Destroy(gameObject);    // Destroy this secondary version of GameStatus
			score.text = "Score:\n" + currentScore.ToString();
		}   // if
		else
		{	// Tell Unity not to destroy this gameObject when another scene loads in the future
			DontDestroyOnLoad(gameObject);
		}	// else
	}   // Awake()

	/***
	*       DisplayScore() displays currentScore on the current game screen.
	***/
	public void DisplayScore()
	{
		score.text = "Score:\n" + currentScore.ToString();
	}   // DisplayScore()

	/***
    *       Start is used to cache the Level object.
    ***/
	void Start()
	{
		currentLevel = FindObjectOfType<Level>();
		DisplayScore();
	}   // Start()

	/***
    *       Update sets our time scale for this level.
    ***/
	void Update()
	{
		Time.timeScale = gameSpeed;
	}   // Update()

	/***
	*       AddToScore() adds to the currentScore the points for the block that was destroyed.
	*       Note:  Later we may want to pass a value based on the type of block that was destroyed.
	*	Right now we are using the blockColor value (a number from 0 to N), adding 1 to it, and using
	*	it to multiply the point value.  If we want, this could be change to be a blockValue, but
	*	tying it to the color will make visual sense to the player.
	***/
	public void AddToScore(Block block)
	{
		//currentScore += pointsPerBlockDestroyed;
		currentScore += pointsPerBlockDestroyed * ((int)block.blockColor + 1);
		DisplayScore();
	}   // AddToScore()

	/***
	*       ResetScore() resets the currentScore to 0 for the start of the game.
	***/
	public void ResetScore()
	{
		currentScore = 0;
		DisplayScore();
	}	// ResetScore()
}   // class GameStatus