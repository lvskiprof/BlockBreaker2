using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
	[Range(0.1f, 3.0f)]
	[SerializeField]
	float   gameSpeed = 1f;
	[SerializeField]
	int     pointsPerBlockDestroyed = 10;   // This is set in the Unity Editor, so this value is overridden
	[SerializeField]
	TextMeshProUGUI score;
	[SerializeField]
	bool    isAutoPlayEnabled = false;

	/***
	*		Current state values.
	***/

	[SerializeField]
	int     currentScore = 0;
	bool    allBlocksDestroyed = false;    // This can be used to tell if all the blocks on the last level have been destroyed

	/***
	*		Cached component references.
	***/

	Scene   currentScene;	// When this doesn't match the current scene, save it and check if we are in Game Over scene

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
			gameObject.SetActive(false);    // Prevent anything from being able to find or interact with this instance of GameStatus
			Destroy(gameObject);    // Destroy this secondary version of GameStatus
			score.text = "Score:\n" + currentScore.ToString();
		}   // if
		else
		{   // Tell Unity not to destroy this gameObject when another scene loads in the future
			DontDestroyOnLoad(gameObject);
		}   // else
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
		DisplayScore();
	}   // Start()

	/***
    *       Update sets our time scale for this level.
    *       It also detects if we are on the last scene by checking for a Text object with a tag of
    *    "Finish".    This is the text that defaults to "You Lost!!!".  If the flag is set that you
    *    destroiyed all the balls on the last level it will change the text to "You Won!!!".
    ***/
	void Update()
	{
		Scene thisScene = SceneManager.GetActiveScene();

		Time.timeScale = gameSpeed;
		if (thisScene != currentScene)
		{   // This is the first time in this scene
			currentScene = thisScene;   // Save it so we don't do this again for this scene
			Text[] resultText = FindObjectsOfType<Text>();
			for (int i = 0; i < resultText.Length; i++)
			{   // See if the text that defaults to "You Lost!" message is present
				if (resultText[i].tag == "Finish" && allBlocksDestroyed)
				{   // Change the text to show that you won
					resultText[i].text = "You Won!!!!";
				}   // if
			}   // for
		}   // if
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
	}   // ResetScore()

	/***
	*       IsAutoPlayEnabled() returns the state of isAutoPlayEnabled, so we don't have
	*   to make the flag public.
	***/
	public bool IsAutoPlayEnabled()
	{
		return isAutoPlayEnabled;
	}   // bool IsAutoPlayEnabled()

	/***
	*       SetAllBlocksDestroyed() sets  allBlockssDestroyed to the passed state.
	***/
	public void SetAllBlocksDestroyed(bool state)
	{
		allBlocksDestroyed = state;
	}   // ResetAllBlocksDestroyed()

	/***
	*       AllBlocksDestroyed() returns the currentScore to 0 for the start of the game.
	***/
	public bool AllBlocksDestroyed()
	{
		return allBlocksDestroyed;
	}   // AllBlocksDestroyed()
}   // class GameStatus