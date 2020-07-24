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
	int     pointsPerBlockDestroyed = 83;
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
    *       Start is used to cache the Level object.
    ***/
	void Start()
	{
		currentLevel = FindObjectOfType<Level>();
		score.text = "Score: " + currentScore.ToString();
	}   // Start()

	/***
    *       Update sets our time scale for this level.
    ***/
	void Update()
	{
		Time.timeScale = gameSpeed;
	}   // Update()

	/***
	*       AddToScore() adds tot he currentScore the points for the block that was destroyed.
	***/

	public void AddToScore()
	{
		currentScore += pointsPerBlockDestroyed;
		score.text = "Score: " + currentScore.ToString();
	}   // AddToScore()
}   // class GameStatus