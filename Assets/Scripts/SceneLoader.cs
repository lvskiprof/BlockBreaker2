using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
/***
*		LoadNextScene(0 will load whatever the next numbered scene in Build Settings.
***/
	public void LoadNextScene()
	{
		int     currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

		SceneManager.LoadScene(currentSceneIndex + 1);
	}   // LoadNextScene()

	/***
	*		LoadStartScene will load the first scene in the Build Settings.
	***/
	public void LoadStartScene()
	{
		SceneManager.LoadScene(0);
	}   // LoadStartScene()

/***
*       ExitGame() is called to exit the game.  It handles the two cases of
*   running in standalone mode or in the Unity Editor.
***/
	public void ExitGame()
	{
		Application.Quit(); // We should not return from this, but will if in the editor
		UnityEditor.EditorApplication.isPlaying = false;    // Handle being in the editor
	}   // ExitGame()
}   // class SceneLoader