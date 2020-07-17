using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
	/***
	*		OnTriggerEnter2D() handles the case where the ball falls below the paddle.
	***/
	private void OnTriggerEnter2D(Collider2D collision)
	{
		SceneManager.LoadScene("Game Over");
	}   // OnTriggerEnter2D()
}	// class LoseCollider